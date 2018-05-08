using TinyEcs;

namespace Game.Components
{
    public struct WeaponState : IComponent
    {
        public float Timeout;
        public float Frequency;
        public float Power;

        public WeaponState(float timeout, float frequency, float power)
        {
            Timeout = timeout;
            Frequency = frequency;
            Power = power;
        }
    }
}
