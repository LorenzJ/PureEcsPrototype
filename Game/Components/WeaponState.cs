using TinyEcs;

namespace Game.Components
{
    public struct WeaponState : IComponent
    {
        public float Timeout;
        public float Frequency;

        public WeaponState(float timeout, float frequency)
        {
            Timeout = timeout;
            Frequency = frequency;
        }
    }
}
