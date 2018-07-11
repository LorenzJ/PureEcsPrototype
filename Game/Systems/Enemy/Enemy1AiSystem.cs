using Game.Components.Enemy;
using Game.Components.Transform;
using Game.Dependencies;
using System.Numerics;
using TinyEcs;

namespace Game.Systems.Enemy
{
    public class Enemy1AiSystem : ComponentSystem<UpdateMessage>
    {
        public class Data
        {
            public int Length;
            public RwData<Enemy1Weapon> Weapons;
            public RoData<Position> Positions;
        }
        [Group] public Data data;
        [Group] public Players players;

        private readonly BulletSpawner bulletSpawner;

        public Enemy1AiSystem(BulletSpawner bulletSpawner)
        {
            this.bulletSpawner = bulletSpawner;
        }

        protected override void Execute(World world, UpdateMessage message)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data.Weapons[i].Timeout1 -= message.DeltaTime;
                if (data.Weapons[i].Timeout1 > 0)
                {
                    continue;
                }
                data.Weapons[i].Timeout1 = .8f;

                var nearestPosition = players.Positions[0];
                var closestDistance = Vector2.DistanceSquared(data.Positions[i].Vector, players.Positions[0].Vector);

                for (var j = 1; j < players.Length; i++)
                {
                    var distance = Vector2.DistanceSquared(data.Positions[i].Vector, players.Positions[j].Vector);
                    if (closestDistance > distance)
                    {
                        closestDistance = distance;
                        nearestPosition = players.Positions[j];
                    }
                }

                var heading = new Heading(Vector2.Normalize(nearestPosition.Vector - data.Positions[i].Vector) * .8f);
                bulletSpawner.Spawn(new BulletSpawner.EnemyBullet(data.Positions[i], heading));
            }
        }
    }
}
