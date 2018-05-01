namespace TinyEcs
{
    internal interface IHandle<T>
    {
        T Handle { get; }
    }
}