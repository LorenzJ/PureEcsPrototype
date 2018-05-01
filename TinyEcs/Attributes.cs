using System;

namespace TinyEcs
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GroupAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ExcludeAttribute : Attribute
    {
    }
}