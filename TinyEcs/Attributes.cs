using System;

namespace TinyEcs
{
    /// <summary>
    /// Mark a field as a target for simple dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ResourceAttribute : Attribute
    {
    }
    /// <summary>
    /// Mark a field as an injected component group.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class GroupAttribute : Attribute
    {
    }
}