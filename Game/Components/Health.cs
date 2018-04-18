using TinyEcs;

namespace Game.Components
{
    struct Health : IComponent
    {
        public float value;

        public Health(float value)
        {
            this.value = value;
        }
    }
}
