using System;
using Game.Components.Transform;
using OpenGL;
using TinyEcs;

namespace GameGl
{
    public class Renderer
    {
        private BulletBatch bulletBatch = BulletBatch.Create();
        private PlayerBatch playerBatch = PlayerBatch.Create();
        private Background background = Background.Create();
        private int playerBulletCount;
        private Position[] playerBulletPositions;
        private int playerCount;
        private Position[] playerPositions;

        public void Render(float time)
        {
            background.Draw(time);
            playerBatch.Draw(playerPositions, playerCount);
            bulletBatch.Draw(playerBulletPositions, playerBulletCount, time);
        }

        internal void SetPlayers(RoDataStream<Position> positions, int length)
        {
            playerCount = length;
            playerPositions = (Position[])positions;
        }

        internal void SetPlayerBullets(RoDataStream<Position> positions, int length)
        {
            playerBulletCount = length;
            playerBulletPositions = (Position[])positions;
        }
    }
}
