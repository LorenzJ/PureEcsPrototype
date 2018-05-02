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
            var playerBulletsToEnemies = Task.Run(() => 
                GetCollisionPairs(
                    playerBullets.Positions, playerBullets.Colliders, playerBullets.Entities, playerBullets.Length,
                    enemies.Positions, enemies.Colliders, enemies.Entities, enemies.Length))
                .ContinueWith(HandleCollisions);

            var enemyBulletsToPlayers = Task.Run(() =>
                GetCollisionPairs(
                    enemyBullets.Positions, enemyBullets.Colliders, enemyBullets.Entities, enemyBullets.Length,
                    players.Positions, players.Colliders, players.Entities, players.Length))
                .ContinueWith(HandleCollisions);
            
            Task.WaitAll(playerBulletsToEnemies, enemyBulletsToPlayers);
        }

        private void HandleCollisions(Task<List<(Entity, Entity)>> pairs)
        {
            throw new NotImplementedException();
        }

        private List<(Entity, Entity)> 
            GetCollisionPairs(
                RoDataStream<Position> positions1, RoDataStream<Circle> colliders1, RoDataStream<Entity> entities1, int length1,
                RoDataStream<Position> positions2, RoDataStream<Circle> colliders2, RoDataStream<Entity> entities2, int length2)
        {
            var pairs = new List<(Entity, Entity)>();
            for (var i = 0; i < length1; i++)
            {
                for (var j = 0; j < length2; j++)
                {
                    var v = (positions2[j].Vector + colliders2[j].Offset) - (positions1[i].Vector + colliders1[i].Offset);
                    var r = colliders1[i].Radius + colliders2[j].Radius;
                    var rSquared = r * r;
                    if (v.LengthSquared() < rSquared)
                    {
                        pairs.Add((entities1[i], entities2[j]));
                    }
                }
            }
            return pairs;
        }
    }
}
