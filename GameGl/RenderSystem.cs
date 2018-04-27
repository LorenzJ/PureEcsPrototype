using Game;
using TinyEcs;

namespace GameGl
{
    public class RenderSystem : ComponentSystem<RenderMessage>
    {
        [Resource] public Renderer renderer;

        [Group] public PlayerBullets playerBullets;
        [Group] public EnemyBullets enemyBullets;
        [Group] public Players players;
        [Group] public Enemies enemies;

        protected override void Execute(World world, RenderMessage message)
        {
            renderer.SetPlayerBullets(playerBullets.positions, playerBullets.length);
            //renderer.QueueEnemyBullets(enemyBullets.positions, enemyBullets.length);
            //renderer.QueuePlayers(players.positions, players.length);
            //renderer.QueueEnemies(enemies.positions, enemies.length);
        }
    }
}
