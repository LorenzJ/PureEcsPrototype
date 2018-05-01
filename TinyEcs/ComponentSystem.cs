namespace TinyEcs
{
    internal interface ISystem
    {
        void Execute(World world, IMessage message);
    }
    /// <summary>
    /// Base class for systems.
    /// </summary>
    /// <typeparam name="T">Type of message to respond to.</typeparam>
    /// <remarks>
    /// When implementing a non-default constructor, the arguments will be injected.
    /// The dependencies need to have a default constructor in order for the dependency injection to work.
    /// Only one constructor is allowed for each ComponentSystem. Arguments of the same type will share the same instance
    /// across ComponentSystems.
    /// </remarks>
    public abstract class ComponentSystem<T> : ISystem
        where T : IMessage
    {
        /// <summary>
        /// Executed when a message of type T is posted.
        /// </summary>
        /// <param name="world">The world this system instance belongs to.</param>
        /// <param name="message">The message this system is listening for.</param>
        /// <remarks>
        /// ComponentSystems usually run in parallel.
        /// It's not safe to call any methods of the World instance aside from GetDependency&lt;T&gt; and Ref&lt;T&gt;
        /// </remarks>
        internal protected abstract void Execute(World world, T message);

        void ISystem.Execute(World world, IMessage message)
        {
            Execute(world, (T)message);
        }
    }
}
