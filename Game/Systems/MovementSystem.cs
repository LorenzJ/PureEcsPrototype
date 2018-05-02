using TinyEcs;
using System.Threading.Tasks;
using Game.Components.Transform;
using System.Collections.Concurrent;

namespace Game.Systems
{
    public class MovementSystem : ComponentSystem<UpdateMessage>
    {

        public class Data
        {
            public int Length;
            public RwDataStream<Position> Positions;
            public RoDataStream<Heading> Directions;
        }
        [Group] public Data data;

        protected override void Execute(World world, UpdateMessage message)
        {
            if (data.Length == 0) return;
            var partitioner = Partitioner.Create(0, data.Length);
            Parallel.ForEach(partitioner, partition =>
            {
                for (var i = partition.Item1; i < partition.Item2; i++)
                {
                    data.Positions[i].Vector += data.Directions[i].Vector * message.DeltaTime;
                }
            });
        }
    }
}
