using Game.Components.Transform;
using Game.Components;
using TinyEcs;
using System.Collections.Generic;
using System;

namespace Game.Dependencies
{
    public class BulletSpawner : IOnLoad
    {
        private Archetype[] bulletTypes = new Archetype[2];
        private List<BulletCommand> bulletCommands = new List<BulletCommand>();

        public enum BulletType : int
        {
            Player = 0,
            Enemy = 1
        }
        public struct BulletCommand
        {
            public Position Position;
            public Heading Heading;
            public BulletType BulletType;

            public BulletCommand(Position position, Heading heading, BulletType bulletType)
            {
                Position = position;
                Heading = heading;
                BulletType = bulletType;
            }
        }

        public void OnLoad(World world)
        {
            var bullet = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(Components.Colliders.Circle), typeof(BulletTag));
            bulletTypes[(int)BulletType.Player] = world.CreateArchetype(bullet, typeof(PlayerTag));
            bulletTypes[(int)BulletType.Enemy] = world.CreateArchetype(bullet, typeof(EnemyTag));
        }

        public void Spawn(IEnumerable<BulletCommand> commands)
        {
            lock (bulletCommands)
            {
                bulletCommands.AddRange(commands);
            }
        }

        public void Spawn(BulletCommand command)
        {
            lock (bulletCommands)
            {
                bulletCommands.Add(command);
            }
        }

        internal void Commit(World world)
        {
            foreach (var bulletCommand in bulletCommands)
            {
                var newBullet = world.CreateEntity(bulletTypes[(int)bulletCommand.BulletType]);
                world.Ref<Position>(newBullet) = bulletCommand.Position;
                world.Ref<Heading>(newBullet) = bulletCommand.Heading;
            }
            bulletCommands.Clear();
        }
    }
}
