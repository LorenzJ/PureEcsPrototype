using TinyEcs;

namespace Game.Components.Enemy
{
    public struct EnemyInfo : IComponent
    {
        public int Value;

        public EnemyInfo(int value)
        {
            Value = value;
        }
    }
}
