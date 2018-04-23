using TinyEcs;

namespace Game
{
    class BulletSpawner : Resource
    {
        Archetype playerBullet;
        Archetype enemyBullet;

        protected override void OnLoad(World world)
        {
            playerBullet = world.CreateArchetype(typeof())
        }
    }
}
