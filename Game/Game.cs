using Game.Components.Transform;
using OpenGL;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private World world;
        private Entity player;

        public World World { get => world; }

        public Game()
        {
            world = World.Create();

            player = world.CreateEntity();
            world.Add(player, new Position { vector = new Vertex2f(0) });
            world.Add(player, new Heading { vector = new Vertex2f(1) });

            var archeType = world.CreateArcheType(typeof(Position), typeof(Heading));
            for (int i = 0; i < 100000; i++)
            {
                var entity = world.CreateEntity(archeType);
                world.Get<Position>(entity).vector = new Vertex2f(i * 0.2f, 100);
                world.Get<Heading>(entity).vector = new Vertex2f(0.1f, 0);
            }

        }

        public void Update(float deltaTime)
        {
            world.Post(new UpdateMessage(deltaTime));
            world.Post(new LateUpdateMessage(deltaTime));
        }
    }
}
