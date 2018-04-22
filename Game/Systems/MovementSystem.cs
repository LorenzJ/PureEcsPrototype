using TinyEcs;
using Game.Components;
using System.Threading.Tasks;

namespace Game.Systems
{
    public class MovementSystem : System<UpdateMessage>
    {

        class Data
        {
            public int length = default;
            public RwArray<Position> positions = default;
            public RArray<Direction> directions = default;
        }
        [Group] Data data;

        protected override void Execute(World world, UpdateMessage message)
        {
            var positions = data.positions;
            var directions = data.directions;

            Parallel.For(0, data.length, i =>
            {
                positions[i].vector += directions[i].vector * message.DeltaTime;
            });
        }
    }
}
