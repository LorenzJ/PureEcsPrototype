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
        private List<PlayerBullet> playerBullets = new List<PlayerBullet>();
        private List<EnemyBullet> enemyBullets = new List<EnemyBullet>();
        private Archetype playerBulletType;
        private Archetype enemyBulletType;

        public struct PlayerBullet
        {
            public Position Position;
            public Heading Heading;
            public float Power;

            public PlayerBullet(Position position, Heading heading, float power)
            {
                Position = position;
                Heading = heading;
                Power = power;
            }
        }

        public struct EnemyBullet
        {
            public Position Position;
            public Heading Heading;

            public EnemyBullet(Position position, Heading heading)
            {
                Position = position;
                Heading = heading;
            }
        }

        public void OnLoad(World world)
        {
            var bullet = world.CreateArchetype(typeof(Position), typeof(Heading), typeof(Components.Colliders.Circle), typeof(BulletTag));
            playerBulletType = world.CreateArchetype(bullet, typeof(PlayerTag), typeof(DamageSource));
            enemyBulletType = world.CreateArchetype(bullet, typeof(EnemyTag));
        }

        public void Spawn(PlayerBullet bullet)
        {
            lock (playerBullets)
            {
                playerBullets.Add(bullet);
            }
        }

        public void Spawn(EnemyBullet bullet)
        {
            lock (enemyBullets)
            {
                enemyBullets.Add(bullet);
            }
        }

        internal void Commit(World world)
        {
            foreach (var bullet in playerBullets)
            {
                var newBullet = world.CreateEntity(playerBulletType);
                world.Ref<Position>(newBullet) = bullet.Position;
                world.Ref<Heading>(newBullet) = bullet.Heading;
                world.Ref<DamageSource>(newBullet).Value = bullet.Power;
            }
            foreach (var bullet in enemyBullets)
            {
                var newBullet = world.CreateEntity(enemyBulletType);
                world.Ref<Position>(newBullet) = bullet.Position;
                world.Ref<Heading>(newBullet) = bullet.Heading;
            }
            playerBullets.Clear();
            enemyBullets.Clear();
        }
    }
}
