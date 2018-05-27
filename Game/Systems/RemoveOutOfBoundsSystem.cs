using Game.Components;
using Game.Components.Transform;
using Game.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyEcs;

namespace Game.Systems
{
    public class RemoveOutOfBoundsSystem : ComponentSystem<LateUpdateMessage>
    {
        public class Bullets
        {
            public int Length;
            public RoDataStream<Entity> Entities;
            public RoDataStream<Position> Positions;
            public BulletTag BulletTag;
        }
        [Group] public Bullets bullets;

        public class Enemies
        {
            public int Length;
            public RoDataStream<Entity> Entities;
            public RoDataStream<Position> Positions;
            public ShipTag ShipTag;
            public EnemyTag EnemyTag;
        }
        [Group] public Enemies enemies;

        private DeadEntityList deaths;
        private List<Entity> oobBullets = new List<Entity>(64);
        private List<Entity> oobEnemies = new List<Entity>(64);

        public RemoveOutOfBoundsSystem(DeadEntityList deadEntityList)
        {
            deaths = deadEntityList;
        }

        protected override void Execute(World world, LateUpdateMessage message)
        {
            var removeBullets = Task.Run(() =>
            {
                for (var i = 0; i < bullets.Length; i++)
                {
                    var pos = bullets.Positions[i].Vector;
                    if (pos.X < -1.2f || pos.X > 1.2f || pos.Y > 2.0f || pos.Y < -2.0f)
                    {
                        oobBullets.Add(bullets.Entities[i]);
                    }
                }
                deaths.AddRange(oobBullets);
                oobBullets.Clear();
            });

            var removeEnemies = Task.Run(() =>
            {
                for (var i = 0; i < enemies.Length; i++)
                {
                    var pos = enemies.Positions[i].Vector;
                    if (pos.Y < -2f)
                    {
                        oobEnemies.Add(enemies.Entities[i]);
                    }
                }
                deaths.AddRange(oobEnemies);
                oobEnemies.Clear();
            });

            Task.WaitAll(removeBullets, removeEnemies);
        }
    }
}
