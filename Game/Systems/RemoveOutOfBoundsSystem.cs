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
    public class RemoveOutOfBoundsSystem : ComponentSystem<UpdateMessage>
    {
        public class Bullets
        {
            public int Length;
            public RoDataStream<Entity> Entities;
            public RoDataStream<Position> Positions;
            public BulletTag BulletTag;
        }
        [Group] public Bullets bullets;
        public Bullets bullets2 = new Bullets();

        private DeadEntityList deaths;
        private List<Entity> oobBullets = new List<Entity>(64);

        public RemoveOutOfBoundsSystem(DeadEntityList deadEntityList)
        {
            deaths = deadEntityList;
        }

        protected override void Execute(World world, UpdateMessage message)
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

            Task.WaitAll(removeBullets);
        }
    }
}
