using Game.Components;
using OpenGL;
using System;
using System.Threading.Tasks;
using TinyEcs;

namespace Game.Systems
{
    class GravitySystem : ISystem
    {
        public Type MessageType => typeof(LateUpdateMessage);

        private static Vertex2f gravityDirection = new Vertex2f(0, -8);

        public class Data
        {
            [Length] public int length;
            [Write] public Direction[] directions;
        }
        [InjectComponents] public Data data = new Data();

        public void Do(World world, Message message)
        {
            var deltaTime = (message as LateUpdateMessage).DeltaTime;

            //for (var i = 0; i < data.length; i++)
            //{
            //    data.directions[i].vector += gravityDirection * deltaTime;
            //}
            Parallel.For(0, data.length, i =>
            {
                data.directions[i].vector += gravityDirection * deltaTime;
            });
        }
    }
}
