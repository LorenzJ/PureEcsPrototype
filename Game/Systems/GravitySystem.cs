using Game.Components;
using OpenGL;
using System;
using System.Threading.Tasks;
using TinyEcs;

namespace Game.Systems
{
    public class GravitySystem : System<LateUpdateMessage>
    {
        public Type MessageType => typeof(LateUpdateMessage);

        private static Vertex2f gravityDirection = new Vertex2f(0, -8);

        public class Data
        {
            public int length;
            public RwArray<Direction> directions;
        }
        [Group] public Data data;

        protected override void Execute(World world, LateUpdateMessage message)
        {
            Parallel.For(0, data.length, i =>
            {
                data.directions[i].vector += gravityDirection * message.DeltaTime;
            });
        }
    }
}
