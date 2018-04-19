using System;
using TinyEcs;
using Game.Components;
using System.Threading.Tasks;

namespace Game.Systems
{
    class MovementSystem : ISystem
    {
        public Type MessageType => typeof(UpdateMessage);

        class Components
        {
            [Length] public int length = default;
            [Read, Write] public Position[] positions = default;
            [Read] public Direction[] directions = default;
        }
        [InjectComponents] Components components = new Components();

        public void Do(World world, Message message)
        {
            var deltaTime = (message as UpdateMessage).DeltaTime;

            var positions = components.positions;
            var directions = components.directions;

            Parallel.For(0, components.length, i =>
            {
                positions[i].vector += directions[i].vector * deltaTime;
            });
            //for (int i = 0; i < components.length; i++)
            //{
            //    positions[i].vector += directions[i].vector * deltaTime;
            //}
        }
    }
}
