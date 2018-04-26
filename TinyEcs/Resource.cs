namespace TinyEcs
{
    // Should perhaps be an interface
    public abstract class Resource
    {
        // Maybe make these methods a part of an interface
        internal protected virtual void OnLoad(World world) { }
        internal protected virtual void Flush(IMessage message) { }
    }
}