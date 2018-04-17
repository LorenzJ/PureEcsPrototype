using Game.Components;
using OpenGL;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private World world;
        private Entity player;
        private UpdateMessage updateMessage;

        public Game()
        {
            world = World.Create();

            player = world.CreateEntity();
            world.SetComponent(player, new Position { vector = new Vertex3f(0) });
            world.SetComponent(player, new Direction { vector = new Vertex3f(1) });

            for (int i = 0; i < 1000; i++)
            {
                var entity = world.CreateEntity();
                world.SetComponent(entity, new Position { vector = new Vertex3f(-1) });
                world.SetComponent(entity, new Direction { vector = new Vertex3f(2) });
            }

            updateMessage = new UpdateMessage(1 / 60.0f);
        }

        public void Update(float deltaTime)
        {
            updateMessage.DeltaTime = deltaTime;
            world.Post(updateMessage);
        }

        public void Flush()
        {
            world.Flush();
        }
    }
}
