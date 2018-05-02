using TinyEcs;

namespace Game.Components
{
    struct Health : IComponent
    {
        public float Value;

        public Health(float value)
        {
            Value = value;
        }
    }
}
