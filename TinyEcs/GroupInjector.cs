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
        private readonly Action injector;
        private readonly Type[] writes;

        private struct DummyComponent : IComponent { }

        public GroupInjector(World world, object targetObject)
        {
            var targetType = targetObject.GetType();
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            var fields = targetType.GetFields(bindingFlags);
            var lengthField = fields
                .Where(fi => fi.FieldType == typeof(int))
                .SingleOrDefault();
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

            var otherFields = new FieldInfo[] { lengthField, entitiesField }.Where(fi => fi != null);

            var unknownFields = fields.Except(writeFields).Except(readFields).Except(tagFields).Except(otherFields);
            if (unknownFields.Count() > 0)
            {
                throw new UnknownFieldsException(unknownFields);
            }

            componentGroup = world.CreateComponentGroup(includes, excludes, entitiesField != null);
            writes = writeFields.Select(GetComponentType).ToArray();

            var expressions = new List<Expression>();
            if (lengthField != null)
            {
                var expr = Expression.Assign(
                    Expression.Field(Expression.Constant(targetObject, targetType), lengthField),
                    Expression.Property(Expression.Constant(componentGroup), "Count"));
                expressions.Add(expr);
            }
            if (entitiesField != null)
            {
                var expr = Expression.Assign(
                    Expression.Field(Expression.Constant(targetObject), entitiesField),
                    Expression.Property(Expression.Constant(componentGroup), "Entities"));
                expressions.Add(expr);
            }

            var readMethod = new Func<RoDataStream<DummyComponent>>(componentGroup.GetRead<DummyComponent>).Method.GetGenericMethodDefinition();
            var writeMethod = new Func<RwDataStream<DummyComponent>>(componentGroup.GetWrite<DummyComponent>).Method.GetGenericMethodDefinition();

            var injectorBody =
                CreateInjectors(targetObject, readFields, readMethod)
                .Concat(CreateInjectors(targetObject, writeFields, writeMethod))
                .Concat(expressions);

            injector = Expression.Lambda<Action>(Expression.Block(injectorBody)).Compile();
        }

        private static Type GetComponentType(FieldInfo fi) => fi.FieldType.GetGenericArguments()[0];

        private IEnumerable<Expression> CreateInjectors(object targetObject, FieldInfo[] fields, MethodInfo methodInfo)
        {
            return fields.Select(field =>
            {
                var method = methodInfo.MakeGenericMethod(GetComponentType(field));
                return Expression.Assign(
                    Expression.Field(Expression.Constant(targetObject), field),
                    Expression.Call(Expression.Constant(componentGroup), method));
            });
        }

        public void Inject()
        {
            componentGroup.UpdateStream();
            injector();
        }

        public void WriteAndUnlock() => componentGroup.WriteAndUnlock(writes);

    }
}
