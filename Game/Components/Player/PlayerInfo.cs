using TinyEcs;

namespace Game.Components.Player
{
    public struct PlayerInfo : IComponent
    {
        public int Id;
        public int Lives;
        public float Speed;
        public int Score;
    }
}
