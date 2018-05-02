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
    /// Only one constructor is allowed for each <see cref="ComponentSystem{T}"/>. Arguments of the same type will share the same instance
    /// across ComponentSystems.
    /// </remarks>
    public abstract class ComponentSystem<T> : ISystem
        where T : IMessage
    {
        /// <summary>
        /// Executed when a message of type <typeparamref name="T"/> is posted.
        /// </summary>
        /// <param name="world">The world this system instance belongs to.</param>
        /// <param name="message">The message this system is listening for.</param>
        /// <remarks>
        /// ComponentSystems usually run in parallel.
        /// It's not safe to call any methods of the <see cref="World"/> instance aside from <c>GetDependency&lt;T&gt;</c> and <c>Ref&lt;T&gt;</c>
        /// </remarks>
        internal protected abstract void Execute(World world, T message);

        void ISystem.Execute(World world, IMessage message)
        {
            Execute(world, (T)message);
        }
    }
}
