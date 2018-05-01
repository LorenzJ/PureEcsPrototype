using System;
using System.Linq;
using System.Reflection;

namespace TinyEcs
{
    internal class GroupInjector
    {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private struct Injector
        {
            public Type type;
            public FieldInfo field;
            public ConstructorInfo constructor;

            public Injector(Type type, FieldInfo field, ConstructorInfo constructor)
            {
                this.type = type;
                this.field = field;
                this.constructor = constructor;
            }
        }

        private object targetObject;
        private ComponentGroup componentGroup;
        FieldInfo lengthField;
        private FieldInfo entitiesField;
        private Injector[] readInjectors;
        private Injector[] writeInjectors;
        private Type[] tags;

        internal GroupInjector(World world, object targetObject)
        {
            this.targetObject = targetObject;
            var targetType = targetObject.GetType();

            var fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            lengthField = targetType.GetField("length", bindingFlags);
            entitiesField = fields
                .Where(fi => fi.FieldType == typeof(RoDataStream<Entity>))
                .SingleOrDefault();
           
            readInjectors = CreateReadInjectors(fields);
            writeInjectors = CreateWriteInjectors(fields);
            tags = FindTags(fields);

            var types =
                readInjectors.Select(ri => ri.type)
                .Union(writeInjectors.Select(wi => wi.type))
                .Union(tags).ToArray();

            componentGroup = world.CreateComponentGroup(types);

            // Check if all fields were handled, otherwise throw an exception.
            {
                var fieldCount = 0;
                if (lengthField != null) fieldCount++;
                if (entitiesField != null) fieldCount++;
                fieldCount += readInjectors.Length;
                fieldCount += writeInjectors.Length;
                fieldCount += tags.Length;
                if (fieldCount != fields.Length)
                {
                    throw new UnknownFieldsException($"{fields.Length - fieldCount} uknown fields in group");
                }
            }
        }

        public void Inject()
        {
            // Make sure the data is up to date.
            componentGroup.UpdateStream();
            // Inject the write injectors
            foreach (var injector in writeInjectors)
            {
                var stream = injector.constructor.Invoke(new object[] { componentGroup.GetWrite(injector.type) });
                injector.field.SetValue(targetObject, stream);
            }

            // Do the same for the read injectors
            foreach (var injector in readInjectors)
            {
                var stream = injector.constructor.Invoke(new object[] { componentGroup.GetRead(injector.type) });
                injector.field.SetValue(targetObject, stream);
            }

            // Inject length and entities
            if (entitiesField != null)
            {
                entitiesField.SetValue(targetObject, componentGroup.GetEntities());
            }
            if (lengthField != null)
            {
                lengthField.SetValue(targetObject, componentGroup.Count);
            }
        }

        public void WriteAndUnlock()
        {
            foreach (var injector in writeInjectors)
            {
                componentGroup.WriteAndUnlock(injector.type);
            }
        }

        private Injector[] CreateWriteInjectors(FieldInfo[] fields)
        {
            var writeFields = fields
                .Where(fi => fi.FieldType.IsGenericType 
                    && fi.FieldType.GetGenericTypeDefinition() == typeof(RwDataStream<>))
                .ToArray();

            var rwDataStreamType = typeof(RwDataStream<>);
            return CreateInjectors(writeFields, rwDataStreamType); ;
        }

        private Injector[] CreateReadInjectors(FieldInfo[] fields)
        {
            var readFields = fields
                .Where(fi => fi.FieldType.IsGenericType 
                    && fi.FieldType != typeof(RoDataStream<Entity>)
                    && fi.FieldType.GetGenericTypeDefinition() == typeof(RoDataStream<>))
                .ToArray();

            var roDataStreamType = typeof(RoDataStream<>);
            return CreateInjectors(readFields, roDataStreamType);
        }

        private static Injector[] CreateInjectors(FieldInfo[] fields, Type dataStreamType)
        {
            var injectors = new Injector[fields.Length];
            for (var i = 0; i < injectors.Length; i++)
            {
                injectors[i].type = fields[i].FieldType.GetGenericArguments()[0];
                injectors[i].field = fields[i];

                // Get the generic type of the data stream
                var genericDataStreamType = dataStreamType.MakeGenericType(injectors[i].type);
                // Get the constructor
                injectors[i].constructor = genericDataStreamType.GetConstructor(new Type[] { injectors[i].type.MakeArrayType() });
            }
            return injectors;
        }

        internal Type[] FindTags(FieldInfo[] fields)
            => fields
                .Where(fi => fi.FieldType.GetInterfaces().Contains(typeof(ITag)))
                .Select(fi => fi.FieldType).ToArray();
         
    }
}
