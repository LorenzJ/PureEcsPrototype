using System;

namespace TinyEcs
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ResourceAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class GroupAttribute : Attribute
    {
    }
}