using System;

namespace TinyEcs
{
    /// <summary>
    /// Mark a field as an injected component group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GroupAttribute : Attribute
    {
    }
    /// <summary>
    /// Exclude entities that are marked by a tag. (Not yet implemented)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ExcludeAttribute : Attribute
    {
    }
}