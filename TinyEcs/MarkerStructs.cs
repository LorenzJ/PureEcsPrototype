namespace TinyEcs
{
    /// <summary>
    /// Mark a component as a tag
    /// </summary>
    /// <typeparam name="T">Type of component</typeparam>
    public struct Tag<T> where T : struct, IComponent 
    {
    }

    /// <summary>
    /// Mark a component for exclusion
    /// </summary>
    /// <typeparam name="T">Type of component</typeparam>
    public struct Exclude<T> where T : struct, IComponent
    {
    }
}
