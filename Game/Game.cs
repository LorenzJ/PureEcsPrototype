using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using Game.Components.Utilities;
using System;
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
            var shipType = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(ShipTag), typeof(Circle));
            var playerShipType = world.DeriveArchetype(shipType, typeof(PlayerTag));
            var enemyShipType = world.DeriveArchetype(shipType, typeof(EnemyTag));

            var bulletType = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(BulletTag), typeof(Circle), typeof(Ttl));
            var playerBulletType = world.DeriveArchetype(bulletType, typeof(PlayerTag));
            var enemyBulletType = world.DeriveArchetype(bulletType, typeof(EnemyTag));

            var rng = new Random();
            float nextFloat() => (float)(rng.NextDouble() - 0.5) * 2.0f;
            for (int i = 0; i < 2000; i++)
            {
                var bullet = world.CreateEntity(playerBulletType);
                world.Get<Position>(bullet).vector = new OpenGL.Vertex2f(nextFloat(), nextFloat());
                world.Get<Heading>(bullet).vector = new OpenGL.Vertex2f(nextFloat(), nextFloat());
                world.Get<Ttl>(bullet).value = (float)(rng.NextDouble() * 10) + 5;
            }
            
        }

        public void Update(float deltaTime)
        {
            time += deltaTime;
            world.Post(new UpdateMessage(deltaTime));
            world.Post(new LateUpdateMessage(deltaTime));
        }
    }
}
