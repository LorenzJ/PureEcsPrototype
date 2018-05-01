﻿using Game;
using TinyEcs;

namespace GameGl
{
    public class RenderSystem : ComponentSystem<RenderMessage>
    {
        private Renderer renderer;

        [Group] public PlayerBullets playerBullets;
        [Group] public EnemyBullets enemyBullets;
        [Group] public Players players;
        [Group] public Enemies enemies;

        public RenderSystem(Renderer renderer)
        {
            this.renderer = renderer;
        }

        protected override void Execute(World world, RenderMessage message)
        {
            renderer.SetPlayers(players.positions, players.length);
            renderer.SetPlayerBullets(playerBullets.positions, playerBullets.length);
            //renderer.QueueEnemyBullets(enemyBullets.positions, enemyBullets.length);
            //renderer.QueuePlayers(players.positions, players.length);
            //renderer.QueueEnemies(enemies.positions, enemies.length);
        }
    }
}
