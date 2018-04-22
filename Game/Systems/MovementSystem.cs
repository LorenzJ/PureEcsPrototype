using TinyEcs;
using System.Threading.Tasks;
using Game.Components.Transform;

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
            Parallel.For(0, data.length, i =>
            {
                data.positions[i].vector += data.directions[i].vector * message.DeltaTime;
            });
        }
    }
}
