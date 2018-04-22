namespace TinyEcs
{
    internal interface ISystem
    {
        void Execute(World world, IMessage message);
    }
    public abstract class System<T> : ISystem
        where T : IMessage
    {
        internal protected abstract void Execute(World world, T message);

        void ISystem.Execute(World world, IMessage message)
        {
            Execute(world, (T)message);
        }
    }
}
