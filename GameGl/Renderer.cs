using Game.Components.Transform;
using OpenGL;
using TinyEcs;

namespace GameGl
{
    public class Renderer
    {
        private BulletBatch bulletBatch = BulletBatch.Create();
        private ShipBatch shipBatch = ShipBatch.Create();
        private Background background = Background.Create();

        private int playerBulletCount;
        private Position[] playerBulletPositions;

        private int enemyBulletCount;
        private Position[] enemyBulletPositions;

        private int playerCount;
        private Position[] playerPositions;

        private int enemyCount;
        private Position[] enemyPositions;

        public void Render(float time)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            background.Draw(time);
            shipBatch.Draw(playerPositions, playerCount, time);
            shipBatch.Draw(enemyPositions, enemyCount, time);
            bulletBatch.Draw(playerBulletPositions, playerBulletCount, time);
            bulletBatch.Draw(enemyBulletPositions, enemyBulletCount, time + .5f);
        }

        internal void SetPlayers(RoData<Position> positions, int length)
        {
            playerCount = length;
            playerPositions = (Position[])positions;
        }

        internal void SetEnemies(RoData<Position> positions, int length)
        {
            enemyPositions = (Position[])positions;
            enemyCount = length;
        }

        internal void SetPlayerBullets(RoData<Position> positions, int length)
        {
            playerBulletCount = length;
            playerBulletPositions = (Position[])positions;
        }

        internal void SetEnemyBullets(RoData<Position> positions, int length)
        {
            enemyBulletCount = length;
            enemyBulletPositions = (Position[])positions;
        }
    }
}
