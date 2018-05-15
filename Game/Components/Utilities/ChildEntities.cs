using TinyEcs;

namespace Game.Components.Utilities
{
    public struct ChildEntities : IComponent
    {
        public Entity[] Entities;

        public ChildEntities(Entity[] children)
        {
            Entities = children;
        }
    }
}
