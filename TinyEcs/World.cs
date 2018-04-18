using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace TinyEcs
{
    public class World
    {
        private EntityManager entityManager;

        private Dictionary<Type, IComponentContainer> componentContainerMap;
        private Lookup<Type, ISystem> systemMap;
        private List<Unpacker> unpackers = new List<Unpacker>();

        private Dictionary<Type[], Entity[]> entityCache = new Dictionary<Type[], Entity[]>(new EqualityComparer());

        private bool dirty;

        private const int initialSize = 1024;
        private int size = initialSize;

        private class EqualityComparer : IEqualityComparer<Type[]>
        {
            public bool Equals(Type[] x, Type[] y)
                => x.Length == y.Length && x.Intersect(y).Count() == x.Length;

            public int GetHashCode(Type[] obj)
            {
                unchecked
                {
                    var result = 17;
                    for (int i = 0; i < obj.Length; i++)
                    {
                        result = result * 23 + obj[i].GetHashCode();
                    }
                    return result;
                }
            }
        }

        private World(EntityManager entityManager, Dictionary<Type, IComponentContainer> componentContainerMap, Lookup<Type, ISystem> systemMap)
        {
            this.entityManager = entityManager;
            this.componentContainerMap = componentContainerMap;
            this.systemMap = systemMap;
        }

        public Entity CreateEntity()
        {
            dirty = true;
            var entity = entityManager.CreateEntity();
            if (entity.id >= size)
            {
                Resize();
            }
            return entity;
        }

        private void Resize()
        {
            size *= 2;
            foreach (var entry in componentContainerMap)
            {
                entry.Value.Resize(size);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            dirty = true;
            entityManager.RemoveEntity(entity);
            foreach (var entry in componentContainerMap)
            {
                entry.Value.Remove(entity);
            }
        }

        public void Post(Message message)
        {
            Parallel.ForEach(systemMap[message.GetType()], system =>
            {
                InjectComponents(system);
                system.Do(this, message);
            });
            Parallel.ForEach(unpackers, unpacker => unpacker.Unpack());
            unpackers.Clear();
        }

        private void InjectComponents(ISystem system)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var fields = system.GetType().GetFields(bindingFlags)
                .Where(f => f.IsDefined(typeof(InjectComponentsAttribute)));

            foreach (var injectionTarget in fields)
            {
                var injectionRef = injectionTarget.GetValue(system);
                var injectionFields = injectionTarget.FieldType.GetFields();

                var lengths = injectionFields.Where(f => f.IsDefined(typeof(LengthAttribute)));
                var reads = injectionFields
                    .Where(f => f.IsDefined(typeof(ReadAttribute)));
                var writes = injectionFields
                    .Where(f => f.IsDefined(typeof(WriteAttribute)));
                var componentArrays = reads.Union(writes).ToArray();
                var componentTypes = componentArrays
                    .Select(f => f.FieldType.GetElementType())
                    .ToArray();

                var entities = GetEntitiesWith(componentTypes);

                for (int i = 0; i < componentTypes.Length; i++)
                {
                    var group = componentTypes[i];
                    var container = componentContainerMap[group];
                    var packMethod = container.GetType().GetMethod("Pack");
                    var packed = packMethod.Invoke(container, new object[] { entities });
                    componentArrays[i].SetValue(injectionRef, packed);
                    if (writes.Contains(componentArrays[i]))
                    {
                        var unpackMethod = container.GetType().GetMethod("Unpack");
                        unpackers.Add(new Unpacker(container, unpackMethod, entities, packed));
                    }
                }

                foreach (var length in lengths)
                {
                    length.SetValue(injectionRef, entities.Length);
                }
            }
        }

        public void Flush()
        {
            foreach (var componentContainer in componentContainerMap)
            {
                componentContainer.Value.Flush();
            }
            dirty = false;
        }

        public void SetComponent<T>(Entity entity, in T value)
            where T: struct, IComponent
        {
            dirty = true;
            var container = componentContainerMap[typeof(T)] as ComponentContainer<T>;
            container.Set(entity, value);
        }

        public ref T GetComponent<T>(Entity entity)
            where T: struct, IComponent
        {
            dirty = true;
            var container = componentContainerMap[typeof(T)] as ComponentContainer<T>;
            return ref container.Get(entity);
        }

        public void RemoveComponent<T>(Entity entity)
            where T: struct, IComponent
        {
            dirty = true;
            var container = componentContainerMap[typeof(T)] as ComponentContainer<T>;
            container.Remove(entity);
        }

        public Entity[] GetEntitiesWith(Type[] components)
        {
            if (!dirty && entityCache.TryGetValue(components, out var entities_))
            {
                return entities_;
            }
            var containers = components.Select(t => componentContainerMap[t]);
            var entities = new List<Entity>();
            for (int i = 0; i < entityManager.Count; i++)
            {
                if (containers.All(c => c.Contains(entityManager.entities[i])))
                {
                    entities.Add(entityManager.entities[i]);
                }
            }
            var array = entities.ToArray();
            if (entityCache.ContainsKey(components))
            {
                entityCache[components] = array;
            }
            else
            {
                entityCache.Add(components, array);
            }
            return entities.ToArray();
        }

        public static World Create()
        {
            var assembly = Assembly.GetCallingAssembly();
            var systemMap = GetSystemMap(assembly);
            var componentContainerMap = GetComponentContainerMap(assembly, initialSize);
            return new World(new EntityManager(), componentContainerMap, systemMap);
        }

        public void Load(Assembly assembly)
        {
            var systemMap = GetSystemMap(assembly);
            var componentContainerMap = GetComponentContainerMap(assembly, size);
            var systems = new List<ISystem>();
            foreach (var entry in systemMap)
            {
                foreach (var item in entry)
                {
                    systems.Add(item);
                }
            }
            foreach (var entry in this.systemMap)
            {
                foreach (var item in entry)
                {
                    systems.Add(item);
                }
            }
            this.systemMap = systems.ToLookup(x => x.MessageType) as Lookup<Type, ISystem>;
            foreach (var entry in componentContainerMap)
            {
                this.componentContainerMap.Add(entry.Key, entry.Value);
            }
        }

        private static Dictionary<Type, IComponentContainer> GetComponentContainerMap(Assembly assembly, int size)
        {
            var componentContainerMap = new Dictionary<Type, IComponentContainer>();
            {
                var componentTypes = assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IComponent)));
                var containerType = typeof(ComponentContainer<>);
                var parameterTypes = new Type[] { typeof(int) };
                var arguments = new object[] { size };
                foreach (var componentType in componentTypes)
                {
                    var componentTypeContainer = containerType.MakeGenericType(componentType);
                    var constructor = componentTypeContainer.GetConstructor(parameterTypes);
                    componentContainerMap.Add(componentType, constructor.Invoke(arguments) as IComponentContainer);
                }
            }

            return componentContainerMap;
        }

        private static Lookup<Type, ISystem> GetSystemMap(Assembly assembly)
        {
            return assembly.DefinedTypes
                .Where(t => t.ImplementedInterfaces.Contains(typeof(ISystem)))
                .Select(t => (ISystem)t.GetConstructor(Type.EmptyTypes).Invoke(null))
                .ToLookup(s => s.MessageType) as Lookup<Type, ISystem>;
        }
    }

    internal struct Unpacker
    {
        private IComponentContainer container;
        private MethodInfo unpackMethod;
        private Entity[] entities;
        private object values;

        public Unpacker(IComponentContainer container, MethodInfo unpackMethod, Entity[] entities, object values)
        {
            this.container = container;
            this.unpackMethod = unpackMethod;
            this.entities = entities;
            this.values = values;
        }

        public void Unpack()
        {
            unpackMethod.Invoke(container, new object[] { entities, values });
        }
    }
}
