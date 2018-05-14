using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TinyEcs
{ 
    internal class GroupInjector
    {
        private readonly ComponentGroup componentGroup;
        private readonly Action<int> setLength;
        private readonly Action<RoDataStream<Entity>> setEntities;
        private readonly Action[] injectors;
        private readonly Type[] writes;

        private struct DummyComponent : IComponent { }

        public GroupInjector(World world, object targetObject)
        {
            var targetType = targetObject.GetType();
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            var fields = targetType.GetFields(bindingFlags);
            var lengthField = targetType.GetField("Length", bindingFlags);
            var entitiesField = fields
                .Where(fi => fi.FieldType == typeof(RoDataStream<Entity>))
                .SingleOrDefault();
            var readFields = fields
                .Where(fi => fi.FieldType.IsGenericType
                    && fi.FieldType.GetGenericTypeDefinition() == typeof(RoDataStream<>)
                    && fi.FieldType != typeof(RoDataStream<Entity>))
                .ToArray();
            var writeFields = fields
                .Where(fi => fi.FieldType.IsGenericType
                    && fi.FieldType.GetGenericTypeDefinition() == typeof(RwDataStream<>))
                .ToArray();
            var tagFields = fields
                .Where(fi => fi.FieldType.GetInterfaces().Contains(typeof(ITag)));
            var excludedTagFields = tagFields
                .Where(fi => fi.GetCustomAttribute(typeof(ExcludeAttribute)) != null);
            var includedTagFields = tagFields.Except(excludedTagFields);

            
            var includes = readFields.Select(GetComponentType)
                .Concat(writeFields.Select(GetComponentType))
                .Concat(includedTagFields.Select(fi => fi.FieldType))
                .ToArray();
            var excludes = excludedTagFields.Select(fi => fi.FieldType).ToArray();

            componentGroup = world.CreateComponentGroup(includes, excludes);
            writes = writeFields.Select(GetComponentType).ToArray();

            if (lengthField != null)
            {
                var parameter = Expression.Parameter(typeof(int));
                var expr = Expression.Lambda<Action<int>>(
                    Expression.Assign(
                            Expression.Field(Expression.Constant(targetObject, targetType), lengthField),
                            parameter), parameter);
                setLength = expr.Compile();
            }

            if (entitiesField != null)
            {
                var parameter = Expression.Parameter(typeof(RoDataStream<Entity>));
                var expr = Expression.Lambda<Action<RoDataStream<Entity>>>(
                    Expression.Assign(
                        Expression.Field(Expression.Constant(targetObject, targetType), entitiesField),
                        parameter), parameter);
                setEntities = expr.Compile();
            }

            var readMethod = new Func<RoDataStream<DummyComponent>>(componentGroup.GetRead<DummyComponent>).Method.GetGenericMethodDefinition();
            var writeMethod = new Func<RwDataStream<DummyComponent>>(componentGroup.GetWrite<DummyComponent>).Method.GetGenericMethodDefinition();
           
            injectors =
                CreateInjectors(targetObject, readFields, readMethod)
                .Concat(CreateInjectors(targetObject, writeFields, writeMethod))
                .ToArray();
        }

        private static Type GetComponentType(FieldInfo fi) => fi.FieldType.GetGenericArguments()[0];

        private List<Action> CreateInjectors(object targetObject, FieldInfo[] fields, MethodInfo methodInfo)
        {
            var injectors = new List<Action>();
            foreach (var field in fields)
            {
                var type = GetComponentType(field);
                var method = methodInfo.MakeGenericMethod(type);

                var expr = Expression.Lambda<Action>(
                    Expression.Assign(
                        Expression.Field(Expression.Constant(targetObject, targetObject.GetType()), field),
                        Expression.Call(Expression.Constant(componentGroup), method)));

                injectors.Add(expr.Compile());
            }
            return injectors;
        }

        public void Inject()
        {
            componentGroup.UpdateStream();
            setLength?.Invoke(componentGroup.Count);
            setEntities?.Invoke(componentGroup.GetEntities());
            foreach (var injector in injectors)
            {
                injector.Invoke();
            }
        }

        public void WriteAndUnlock() => componentGroup.WriteAndUnlock(writes);

    }
}
