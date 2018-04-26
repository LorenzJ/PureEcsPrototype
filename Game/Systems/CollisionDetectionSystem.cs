using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyEcs;

namespace Game.Systems
{
    public class CollisionDetectionSystem : ComponentSystem<DetectCollisionsMessage>
    {
        [Group] public EnemyBullets enemyBullets;
        [Group] public PlayerBullets playerBullets;
        [Group] public Players players;
        [Group] public Enemies enemies;

        protected override void Execute(World world, DetectCollisionsMessage message)
        {
            var playerBulletToEnemies = Task.Run(() => 
                GetCollisionPairs(
                    playerBullets.positions, playerBullets.colliders, playerBullets.entities, playerBullets.length,
                    enemies.positions, enemies.colliders, enemies.entities, enemies.length));

            var enemyBulletsToPlayer = Task.Run(() => 
                GetCollisionPairs(
                    enemyBullets.positions, enemyBullets.colliders, enemyBullets.entities, enemyBullets.length,
                    players.positions, players.colliders, players.entities, players.length));

            var handlePlayerBulletsToEnemies = playerBulletToEnemies.ContinueWith(HandleCollisions);
            var handleEnemyBulletsToPlayers = enemyBulletsToPlayer.ContinueWith(HandleCollisions);
            
            Task.WaitAll(handlePlayerBulletsToEnemies, handleEnemyBulletsToPlayers);
        }

        private void HandleCollisions(Task<List<(Entity, Entity)>> pairs)
        {
            throw new NotImplementedException();
        }

        private List<(Entity, Entity)> 
            GetCollisionPairs(
                RoArray<Position> positions1, RoArray<Circle> colliders1, RoArray<Entity> entities1, int length1,
                RoArray<Position> positions2, RoArray<Circle> colliders2, RoArray<Entity> entities2, int length2)
        {
            var pairs = new List<(Entity, Entity)>();
            for (var i = 0; i < length1; i++)
            {
                for (var j = 0; j < length2; j++)
                {
                    var v = (positions2[j].vector + colliders2[j].offset) - (positions1[i].vector + colliders1[i].offset);
                    var r = colliders1[i].radius + colliders2[j].radius;
                    var rSquared = r * r;
                    if (v.ModuleSquared() < rSquared)
                    {
                        pairs.Add((entities1[i], entities2[j]));
                    }
                }
            }
            return pairs;
        }
    }
}
