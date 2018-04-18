namespace TinyEcs
{
    internal interface IComponentContainer
    {
        void Add(Entity e);
        void Remove(Entity e);
        bool Contains(Entity e);
        void Flush();
        void Resize(int size);
    }
}