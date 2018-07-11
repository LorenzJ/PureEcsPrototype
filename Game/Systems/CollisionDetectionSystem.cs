using Game.Components;
using Game.Components.Colliders;
using Game.Components.Enemy;
using Game.Components.Player;
using Game.Components.Transform;
using Game.Components.Utilities;
using Game.Dependencies;
using System.Collections.Generic;
using System.Numerics;
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

        private DeadEntityList deadEntityList;
        private World world;

        public CollisionDetectionSystem(DeadEntityList deadEntityList)
        {
            this.deadEntityList = deadEntityList;
        }

        protected override void Execute(World world, DetectCollisionsMessage message)
        {
            this.world = world;
            var playerBulletsToEnemies = Task.Run(() => 
                GetCollisionPairs(
                    playerBullets.Positions, playerBullets.Colliders, playerBullets.Entities, playerBullets.Length,
                    enemies.Positions, enemies.Colliders, enemies.Entities, enemies.Length))
                .ContinueWith(HandleCollisionsWithEnemies);

            var enemyBulletsToPlayers = Task.Run(() =>
                GetCollisionPairs(
                    enemyBullets.Positions, enemyBullets.Colliders, enemyBullets.Entities, enemyBullets.Length,
                    players.Positions, players.Colliders, players.Entities, players.Length))
                .ContinueWith(HandleCollisionsWithPlayers);
            
            Task.WaitAll(playerBulletsToEnemies, enemyBulletsToPlayers);
        }

        private void HandleCollisionsWithEnemies(Task<List<(Entity, Entity)>> pairs)
        {
            var list = new List<Entity>(pairs.Result.Count * 2);
            foreach (var (bullet, enemy) in pairs.Result)
            {
                list.Add(bullet);
                // Could be thread unsafe if other systems were to modify health as well
                if (world.Ref<Health>(enemy).Value > 0)
                {
                    world.Ref<Health>(enemy).Value -= world.Ref<DamageSource>(bullet).Value;
                    if (world.Ref<Health>(enemy).Value <= 0)
                    {
                        list.Add(enemy);
                        var player = world.Ref<ParentEntity>(bullet).Entity;
                        world.Ref<PlayerInfo>(player).Score += world.Ref<EnemyInfo>(enemy).Value;
                    }
                }
            }
            deadEntityList.AddRange(list);
        }

        private void HandleCollisionsWithPlayers(Task<List<(Entity, Entity)>> pairs)
        {
            var list = new List<Entity>(pairs.Result.Count * 2);
            foreach (var (bullet, player) in pairs.Result)
            {
                list.Add(bullet);
                world.Ref<PlayerInfo>(player).Lives -= 1;
                world.Ref<Position>(player).Vector = new Vector2();
            }
            deadEntityList.AddRange(list);
        }

        private List<(Entity, Entity)> 
            GetCollisionPairs(
                RoData<Position> positions1, RoData<Circle> colliders1, RoData<Entity> entities1, int length1,
                RoData<Position> positions2, RoData<Circle> colliders2, RoData<Entity> entities2, int length2)
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
