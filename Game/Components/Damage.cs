using TinyEcs;

namespace Game.Components
{
    public struct DamageSource : IComponent
    {
        public float Value;

        public DamageSource(float value)
        {
            Value = value;
        }
    }
}
