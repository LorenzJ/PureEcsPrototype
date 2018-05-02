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
            public Type Type;
            public FieldInfo Field;
            public ConstructorInfo Constructor;

            public Injector(Type type, FieldInfo field, ConstructorInfo constructor)
            {
                Type = type;
                Field = field;
                Constructor = constructor;
            }
        }

        // The object to inject the components into
        private object targetObject;
        // The backing component group
        private ComponentGroup componentGroup;
        FieldInfo lengthField;
        private FieldInfo entitiesField;
        private Injector[] readInjectors;
        private Injector[] writeInjectors;
        private Type[] includeTags;
        private Type[] excludeTags;

        internal GroupInjector(World world, object targetObject)
        {
            this.targetObject = targetObject;
            var targetType = targetObject.GetType();

            var fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            lengthField = targetType.GetField("Length", bindingFlags);
            entitiesField = fields
                .Where(fi => fi.FieldType == typeof(RoDataStream<Entity>))
                .SingleOrDefault();
           
            readInjectors = CreateReadInjectors(fields);
            writeInjectors = CreateWriteInjectors(fields);
            includeTags = FindIncludeTags(fields);
            excludeTags = FindExcludeTags(fields);

            var includes =
                readInjectors.Select(ri => ri.Type)
                .Union(writeInjectors.Select(wi => wi.Type))
                .Union(includeTags).ToArray();

            componentGroup = world.CreateComponentGroup(includes, excludeTags);

            // Check if all fields were handled, otherwise throw an exception.
            {
                var fieldCount = 0;
                if (lengthField != null) fieldCount++;
                if (entitiesField != null) fieldCount++;
                fieldCount += readInjectors.Length;
                fieldCount += writeInjectors.Length;
                fieldCount += includeTags.Length;
                fieldCount += excludeTags.Length;
                if (fieldCount != fields.Length)
                {
                    throw new UnknownFieldsException($"{fields.Length - fieldCount} unknown fields in group");
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
                var stream = injector.Constructor.Invoke(new object[] { componentGroup.GetWrite(injector.Type) });
                injector.Field.SetValue(targetObject, stream);
            }

            // Do the same for the read injectors
            foreach (var injector in readInjectors)
            {
                var stream = injector.Constructor.Invoke(new object[] { componentGroup.GetRead(injector.Type) });
                injector.Field.SetValue(targetObject, stream);
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
                componentGroup.WriteAndUnlock(injector.Type);
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
                    && fi.FieldType != typeof(RoDataStream<Entity>) // Make sure not to include entities in the search
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
                injectors[i].Type = fields[i].FieldType.GetGenericArguments()[0];
                injectors[i].Field = fields[i];

                // Get the generic type of the data stream
                var genericDataStreamType = dataStreamType.MakeGenericType(injectors[i].Type);
                // Get the constructor
                injectors[i].Constructor = genericDataStreamType.GetConstructor(new Type[] { injectors[i].Type.MakeArrayType() });
            }
            return injectors;
        }

        internal Type[] FindIncludeTags(FieldInfo[] fields)
            => fields
                .Where(fi => fi.FieldType.GetInterfaces().Contains(typeof(ITag))
                    && fi.GetCustomAttribute(typeof(ExcludeAttribute)) == null)
                .Select(fi => fi.FieldType).ToArray();

        internal Type[] FindExcludeTags(FieldInfo[] fields)
            => fields
                .Where(fi => fi.FieldType.GetInterfaces().Contains(typeof(ITag))
                    && fi.GetCustomAttribute(typeof(ExcludeAttribute)) != null)
                .Select(fi => fi.FieldType).ToArray();

    }
}
