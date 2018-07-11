using Game.Components.Utilities;
using Game.Dependencies;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyEcs;

namespace Game.Systems
{
    public class TtlSystem : ComponentSystem<UpdateMessage>
    {
        public class Data
        {
            public int Length;
            public RoData<Entity> Entities;
            public RwData<Ttl> Ttls;
        }
        [Group] public Data data;

        private DeadEntityList deadEntities;

        public TtlSystem(DeadEntityList deadEntities)
        {
            this.deadEntities = deadEntities;
        }

        private ConcurrentBag<List<Entity>> entitiesBag = new ConcurrentBag<List<Entity>>();

        protected override void Execute(World world, UpdateMessage message)
        {
            if (data.Length == 0) return;
            var partitioner = Partitioner.Create(0, data.Length);
            Parallel.ForEach(partitioner, () => new List<Entity>(),
                (partition, state, list) =>
                {
                    for (var i = partition.Item1; i < partition.Item2; i++)
                    {
                        data.Ttls[i].Value -= message.DeltaTime;
                        if (data.Ttls[i].Value <= 0)
                        {
                            list.Add(data.Entities[i]);
                        }
                    }
                    return list;
                }, list => entitiesBag.Add(list));

            while (entitiesBag.TryTake(out var list))
            {
                deadEntities.AddRange(list);
            }
        }
    }
}
