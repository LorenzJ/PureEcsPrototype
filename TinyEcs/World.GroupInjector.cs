using System;
using System.Collections.Generic;
using System.Reflection;

namespace TinyEcs
{
    public partial class World
    {
        private class GroupInjector
        {
            private object groupObject;
            private (Type, FieldInfo, ConstructorInfo)[] writeFields;
            private (Type, FieldInfo, ConstructorInfo)[] readFields;
            private FieldInfo entityField;
            private FieldInfo lengthField;
            internal ComponentGroup componentGroup;

            private object[] arguments = new object[] { null };

            public GroupInjector(object group, (Type, FieldInfo)[] writeFields, (Type, FieldInfo)[] readFields, FieldInfo entityField, FieldInfo lengthField, ComponentGroup componentGroup)
            {
                this.groupObject = group;
                this.entityField = entityField;
                this.lengthField = lengthField;
                this.componentGroup = componentGroup;

                var RwArrayType = typeof(RwArray<>);
                this.writeFields = new(Type, FieldInfo, ConstructorInfo)[writeFields.Length];
                for (var i = 0; i < writeFields.Length; i++)
                {
                    var (type, fieldInfo) = writeFields[i];
                    var rawArrayType = type.MakeArrayType();
                    var wrappedArrayType = RwArrayType.MakeGenericType(type);
                    var constructor = wrappedArrayType.GetConstructor(new Type[] { rawArrayType });
                    this.writeFields[i] = (type, fieldInfo, constructor);
                }
                var RArrayType = typeof(RArray<>);
                this.readFields = new(Type, FieldInfo, ConstructorInfo)[readFields.Length];
                for (var i = 0; i < readFields.Length; i++)
                {
                    var (type, fieldInfo) = readFields[i];
                    var rawArrayType = type.MakeArrayType();
                    var wrappedArrayType = RArrayType.MakeGenericType(type);
                    var constructor = wrappedArrayType.GetConstructor(new Type[] { rawArrayType });
                    this.readFields[i] = (type, fieldInfo, constructor);
                }

            }

            public void Inject(Dictionary<Type, IComponentContainer> containers)
            {
                componentGroup.Inject(containers);
                foreach (var (type, field, constructor) in writeFields)
                {
                    arguments[0] = componentGroup.GetDirectWrite(type);
                    field.SetValue(groupObject, constructor.Invoke(arguments));
                }
                foreach (var (type, field, constructor) in readFields)
                {
                    arguments[0] = componentGroup.GetDirectRead(type);
                    field.SetValue(groupObject, constructor.Invoke(arguments));
                }
                if (entityField != null)
                {
                    entityField.SetValue(groupObject, componentGroup.Entities);
                }
                if (lengthField != null)
                {
                    lengthField.SetValue(groupObject, componentGroup.Length);
                }
            }

            public void Unpack(Dictionary<Type, IComponentContainer> containers)
            {
                componentGroup.Unpack(containers);
            }

        }


    }


}
