using TinyEcs;

namespace Game.Components.Utilities
{
    public struct ParentEntity : IComponent
    {
        public Entity Entity;

        public ParentEntity(Entity parent)
        {
            Entity = parent;
        }
    }
}
