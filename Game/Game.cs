using Game.Components;
using OpenGL;
using System.Diagnostics;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private World world;
        private Entity player;
        private UpdateMessage updateMessage;
        private LateUpdateMessage lateUpdateMessage;

        public World World { get => world; }

        public Game()
        {
            world = World.Create();

            player = world.CreateEntity();
            world.Add(player, new Position { vector = new Vertex2f(0) });
            world.Add(player, new Direction { vector = new Vertex2f(1) });

            for (int i = 0; i < 500000; i++)
            {
                var entity = world.CreateEntity();
                world.Add(entity, new Position { vector = new Vertex2f(-1) });
                world.Add(entity, new Direction { vector = new Vertex2f(2) });
            }

            updateMessage = new UpdateMessage(1 / 60.0f);
            lateUpdateMessage = new LateUpdateMessage(1 / 60.0f);
        }

        public void Update(float deltaTime)
        {
            updateMessage.DeltaTime = deltaTime;
            lateUpdateMessage.DeltaTime = deltaTime;
            world.Post(updateMessage);
            world.Post(lateUpdateMessage);
            Debug.Print($"rate: { 1 / deltaTime } fps");
        }
    }
}
