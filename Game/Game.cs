using Game.Components;
using Game.Components.Colliders;
using Game.Components.Player;
using Game.Components.Transform;
using Game.Components.Utilities;
using Game.Dependencies;
using System.Numerics;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private World world;
        private ShipFactory shipFactory;
        private float time;

        public World World { get => world; }
        public float Time { get => time; set => time = value; }

        public Game()
        {
            world = World.Create();
            shipFactory = new ShipFactory(world);
        }

        public void Init()
        {
            shipFactory.CreateAndAddPlayer(0);
            shipFactory.CreateAndAddEnemy(new Position(new Vector2(0, 1.4f)));
        }

        public void Update(float deltaTime)
        {
            time += deltaTime;
            world.Post(new UpdateMessage(deltaTime));
            world.Post(new LateUpdateMessage(deltaTime));
            world.Post(new DetectCollisionsMessage());
            world.Post(new RenderMessage());
            world.GetDependency<DeadEntityList>().Commit(world);
            world.GetDependency<BulletSpawner>().Commit(world);
        }
    }
}
