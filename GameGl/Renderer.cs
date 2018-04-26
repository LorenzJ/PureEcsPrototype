using System;
using Game.Components.Transform;
using OpenGL;
using TinyEcs;

namespace GameGl
{
    public class Renderer : Resource
    {
        private BulletBatch bulletBatch = BulletBatch.CreateBulletBatch();
        private int playerBulletCount;
        private Position[] playerBulletPositions;

        public Renderer()
        {
            
        }


        public void Render(float time)
        {
            bulletBatch.Draw(playerBulletPositions, playerBulletCount, time);
        }

        internal void SetPlayerBullets(RoArray<Position> positions, int length)
        {
            playerBulletCount = length;
            playerBulletPositions = (Position[])positions;
        }
    }
}
