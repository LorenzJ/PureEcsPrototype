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
    public abstract class ComponentSystem<T> : ISystem
        where T : IMessage
    {
        /// <summary>
        /// Custom logic for a system should be ran here.
        /// </summary>
        /// <param name="world">The world this system instance belongs to.</param>
        /// <param name="message">The message this system is listening for.</param>
        internal protected abstract void Execute(World world, T message);

        void ISystem.Execute(World world, IMessage message)
        {
            Execute(world, (T)message);
        }
    }
}
