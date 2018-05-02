using TinyEcs;

namespace Game.Components.Player
{
    public struct PlayerInfo : IComponent
    {
        public int Id;
        public float Speed;
        public float FireTimeout;
    }
}
