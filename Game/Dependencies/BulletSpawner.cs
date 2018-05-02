using Game.Components.Transform;
using Game.Components;
using TinyEcs;
using System.Collections.Generic;
using System;

namespace Game.Dependencies
{
    public class BulletSpawner : IOnLoad
    {
        Archetype[] bulletTypes = new Archetype[2];
        List<BulletCommand> bulletCommands = new List<BulletCommand>();

        public enum BulletType : int
        {
            Player = 0,
            Enemy = 1
        }
        public struct BulletCommand
        {
            public Position position;
            public Heading heading;
            public BulletType bulletType;

            public BulletCommand(Position position, Heading heading, BulletType bulletType)
            {
                this.position = position;
                this.heading = heading;
                this.bulletType = bulletType;
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
                var newBullet = world.CreateEntity(bulletTypes[(int)bulletCommand.bulletType]);
                world.Ref<Position>(newBullet) = bulletCommand.position;
                world.Ref<Heading>(newBullet) = bulletCommand.heading;
            }
            bulletCommands.Clear();
        }
    }
}
