using TinyEcs;

namespace Game.Components.Utilities
{
    public struct ParentEntity : IComponent
    {
        public Entity Parent;

        public ParentEntity(Entity parent)
        {
            Parent = parent;
        }
    }
}
