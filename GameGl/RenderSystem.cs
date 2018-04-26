using Game;
using System;
using TinyEcs;

namespace GameGl
{
    public class RenderSystem : ComponentSystem<UpdateMessage>
    {
        [Resource] public Renderer renderer;

        [Group] public PlayerBullets playerBullets;
        [Group] public EnemyBullets enemyBullets;
        [Group] public Players players;
        [Group] public Enemies enemies;

        protected override void Execute(World world, UpdateMessage message)
        {
            renderer.SetPlayerBullets(playerBullets.positions, playerBullets.length);
            //renderer.QueueEnemyBullets(enemyBullets.positions, enemyBullets.length);
            //renderer.QueuePlayers(players.positions, players.length);
            //renderer.QueueEnemies(enemies.positions, enemies.length);
        }
    }
}
