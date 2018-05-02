﻿using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using Game.Components.Utilities;
using Game.Dependencies;
using System;
using TinyEcs;

namespace Game
{
    public class Game
    {
        private World world;
        private float time;

        public World World { get => world; }
        public float Time { get => time; set => time = value; }

        public Game()
        {
            world = World.Create();
            var shipType = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(ShipTag), typeof(Circle));
            var playerShipType = world.CreateArchetype(shipType, typeof(PlayerTag));
            var enemyShipType = world.CreateArchetype(shipType, typeof(EnemyTag));

            var bulletType = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(BulletTag), typeof(Circle), typeof(Ttl));
            var playerBulletType = world.CreateArchetype(bulletType, typeof(PlayerTag));
            var enemyBulletType = world.CreateArchetype(bulletType, typeof(EnemyTag));

            var player = world.CreateEntity(playerShipType);
        }

        public void Update(float deltaTime)
        {
            time += deltaTime;
            world.Post(new UpdateMessage(deltaTime));
            world.Post(new LateUpdateMessage(deltaTime));
            world.Post(new RenderMessage());
            world.GetDependency<DeadEntityList>().Commit(world);
        }
    }
}
