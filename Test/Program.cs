using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyEcs;

namespace Test
{
    public class PhysicsMessage : Message
    {
        public readonly float deltaTime;

        public PhysicsMessage(float deltaTime)
        {
            this.deltaTime = deltaTime;
        }
    }
    public class UpdatePositionsMessage : Message
    {
        public readonly float deltaTime;

        public UpdatePositionsMessage(float deltaTime)
        {
            this.deltaTime = deltaTime;
        }
    }

    public struct Position : IComponent
    {
        public float x, y;
    }

    public struct Direction : IComponent
    {
        public float x, y;
    }

    public class GravitySystem : ISystem
    {
        public class Data
        {
            [Length] public int length;
            [Write] public Direction[] directions;
        }
        [InjectComponents] public Data data = new Data();

        public Type MessageType => typeof(PhysicsMessage);

        public void Do(World world, Message msg)
        {
            var message = msg as PhysicsMessage;
            Parallel.For(0, data.length, i =>
            {
                data.directions[i].y -= 2 * message.deltaTime;
            });
        }
    }

    public class UpdatePositionsSystem : ISystem
    {
        public class Data
        {
            [Length] public int length;
            [Read] public Direction[] directions;
            [Write] public Position[] positions;
        }
        [InjectComponents] public Data data = new Data();

        public Type MessageType => typeof(UpdatePositionsMessage);

        public void Do(World world, Message msg)
        {
            var message = msg as UpdatePositionsMessage;
            Parallel.For(0, data.length, i =>
            {
                data.positions[i].x += data.directions[i].x * message.deltaTime;
                data.positions[i].y += data.directions[i].y * message.deltaTime;
            });
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var world = World.Create();
            for (var i = 0; i < 500000; i++)
            {
                var entity = world.CreateEntity();
                world.SetComponent(entity, new Position());
                world.SetComponent(entity, new Direction());
            }
            for (var i = 0; i < 60; i++)
            {
                world.Post(new PhysicsMessage(1 / 60.0f));
                world.Post(new UpdatePositionsMessage(1 / 60.0f));
            }
        }
    }
}
