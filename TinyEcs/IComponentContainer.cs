namespace TinyEcs
{
    internal interface IComponentContainer
    {
        void Add(Entity entity);
        void Remove(Entity entity);
        bool Contains(Entity entity);
        void Flush();
        void Resize(int size);
        void Set(Entity entity, IComponent component);
    }
}