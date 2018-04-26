using Game.Components.Transform;
using Game.Components;
using TinyEcs;
using System.Collections.Generic;

namespace Game
{
    class BulletSpawner : Resource
    {
        Archetype[] bulletTypes = new Archetype[2];
        List<BulletCommand> bulletCommands = new List<BulletCommand>();
        World world;

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

        protected override void OnLoad(World world)
        {
            this.world = world;
            var bullet = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(Components.Colliders.Circle), typeof(BulletTag));
            bulletTypes[(int)BulletType.Player] = world.DeriveArchetype(bullet, typeof(PlayerTag));
            bulletTypes[(int)BulletType.Enemy] = world.DeriveArchetype(bullet, typeof(EnemyTag));
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

        protected override void Flush(IMessage message)
        {
            foreach (var bulletCommand in bulletCommands)
            {
                var newBullet = world.CreateEntity(bulletTypes[(int)bulletCommand.bulletType]);
                world.Get<Position>(newBullet) = bulletCommand.position;
                world.Get<Heading>(newBullet) = bulletCommand.heading;
            }
            bulletCommands.Clear();
        }
    }
}
