using Game.Components.Utilities;
using System.Threading.Tasks;
using TinyEcs;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Game.Systems
{
    public class TtlSystem : ComponentSystem<UpdateMessage>
    {
        public class Data
        {
            public int length;
            public RoArray<Entity> entities;
            public RwArray<Ttl> ttls;
        }
        [Group] public Data data;
        [Resource] public DeadEntityList deadEntities;

        private ConcurrentBag<List<Entity>> entitiesBag = new ConcurrentBag<List<Entity>>();

        protected override void Execute(World world, UpdateMessage message)
        {
            if (data.length == 0) return;
            var partitioner = Partitioner.Create(0, data.length);
            Parallel.ForEach(partitioner, () => new List<Entity>(),
                (partition, state, list) =>
                {
                    for (var i = partition.Item1; i < partition.Item2; i++)
                    {
                        data.ttls[i].value -= message.DeltaTime;
                        if (data.ttls[i].value <= 0)
                        {
                            list.Add(data.entities[i]);
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
