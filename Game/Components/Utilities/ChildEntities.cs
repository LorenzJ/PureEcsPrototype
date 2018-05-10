using TinyEcs;

namespace Game.Components.Utilities
{
    public struct ChildEntities : IComponent
    {
        public Entity[] Children;

        public ChildEntities(Entity[] children)
        {
            Children = children;
        }
    }
}
