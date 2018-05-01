namespace TinyEcs
{
    /// <summary>
    /// Implement this interface to mark a struct as a component type.
    /// Allows the struct to be wrapped by a Ro/Rw-DataStream.
    /// </summary>
    public interface IComponent : IData { }

    /// <summary>
    /// Implement this interface to mark a component as a tag. (Marker component)
    /// </summary>
    public interface ITag { }

    /// <summary>
    /// Interface to mark a struct as usable for a RoDataStream.
    /// </summary>
    public interface IData { }

    /// <summary>
    /// Implement this for messages.
    /// </summary>
    public interface IMessage { }
}
