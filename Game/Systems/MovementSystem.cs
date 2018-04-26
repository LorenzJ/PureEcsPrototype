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
            public int length;
            public RwArray<Position> positions;
            public RoArray<Heading> directions;
        }
        [Group] public Data data;

        protected override void Execute(World world, UpdateMessage message)
        {
            if (data.length == 0) return;
            var partitioner = Partitioner.Create(0, data.length);
            Parallel.ForEach(partitioner, partition =>
            {
                for (var i = partition.Item1; i < partition.Item2; i++)
                {
                    data.positions[i].vector += data.directions[i].vector * message.DeltaTime;
                }
            });
        }
    }
}
