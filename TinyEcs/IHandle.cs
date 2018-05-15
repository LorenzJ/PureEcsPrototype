namespace TinyEcs
{
    /// <summary>
    /// Represents a handle to some resource
    /// </summary>
    /// <typeparam name="T">A type that can represent the handle</typeparam>
    public interface IHandle<T>
    {
        /// <summary>
        /// Getter for the Handle
        /// </summary>
        T Handle { get; }
    }
}