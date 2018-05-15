using Game.Components;
using Game.Components.Colliders;
using Game.Components.Player;
using Game.Components.Transform;
using Game.Components.Utilities;
using Game.Dependencies;
using System;
using System.Numerics;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private ShipFactory shipFactory;

        public World World { get; }
        public float Time { get; set; }

        public Game()
        {
            World = World.Create();
            shipFactory = new ShipFactory(World);
        }

        public void Init()
        {
            shipFactory.CreateAndAddPlayer(0);
            for (var i = 1; i < 200; i++)
            {
                shipFactory.CreateAndAddEnemy(new Position(new Vector2((float)Math.Sin(i), i + (float)Math.Cos(i))));
            }
        }

        public void Update(float deltaTime)
        {
            Time += deltaTime;
            World.Post(new UpdateMessage(deltaTime));
            World.Post(new LateUpdateMessage(deltaTime));
            World.Post(new DetectCollisionsMessage());
            World.Post(new RenderMessage());
            World.GetDependency<DeadEntityList>().Commit(World);
            World.GetDependency<BulletSpawner>().Commit(World);
        }
    }
}
