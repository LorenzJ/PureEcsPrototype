using TinyEcs;

namespace Game.Components.Player
{
    public struct PlayerInfo : IComponent
    {
        public int id;
        public float speed;
        public float fireTimeout;
    }
}
