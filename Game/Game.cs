using Game.Components;
using Game.Components.Colliders;
using Game.Components.Player;
using Game.Components.Transform;
using Game.Components.Utilities;
using Game.Dependencies;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private World world;
        private float time;

        public World World { get => world; }
        public float Time { get => time; set => time = value; }

        public Game()
        {
            world = World.Create();
            var shipType = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(ShipTag), typeof(Circle), typeof(WeaponState));
            var playerShipType = world.CreateArchetype(shipType, typeof(PlayerTag), typeof(PlayerInfo), typeof(Input));
            var enemyShipType = world.CreateArchetype(shipType, typeof(EnemyTag));

            var bulletType = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(BulletTag), typeof(Circle), typeof(Ttl), typeof(ParticleTag));
            var playerBulletType = world.CreateArchetype(bulletType, typeof(PlayerTag), typeof(DamageSource));
            var enemyBulletType = world.CreateArchetype(bulletType, typeof(EnemyTag));

            var player = world.CreateEntity(playerShipType);
            world.Ref<PlayerInfo>(player).Speed = 0.5f;
            world.Ref<WeaponState>(player).Frequency = 0.2f;
        }

        public void Update(float deltaTime)
        {
            time += deltaTime;
            world.Post(new UpdateMessage(deltaTime));
            world.Post(new LateUpdateMessage(deltaTime));
            world.Post(new RenderMessage());
            world.GetDependency<DeadEntityList>().Commit(world);
            world.GetDependency<BulletSpawner>().Commit(world);
        }
    }
}
