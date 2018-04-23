namespace TinyEcs
{
    public abstract class Resource
    {
        internal protected virtual void OnLoad(World world) { }
    }
}