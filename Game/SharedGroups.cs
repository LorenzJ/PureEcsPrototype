using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using TinyEcs;

namespace Game
{
    public class EnemyBullets
    {
        public int length;
        public RoArray<Position> positions;
        public RoArray<Circle> colliders;
        public RoArray<EnemyTag> enemyTag;
        public RoArray<BulletTag> bulletTag;
        public RoArray<Entity> entities;
    }

    public class PlayerBullets
    {
        public int length;
        public RoArray<Position> positions;
        public RoArray<Circle> colliders;
        public RoArray<PlayerTag> playerTag;
        public RoArray<BulletTag> bulletTag;
        public RoArray<Entity> entities;
    }

    public class Players
    {
        public int length;
        public RoArray<Position> positions;
        public RoArray<Circle> colliders;
        public RoArray<PlayerTag> playerTag;
        public RoArray<ShipTag> shipTag;
        public RoArray<Entity> entities;
    }

    public class Enemies
    {
        public int length;
        public RoArray<Position> positions;
        public RoArray<Circle> colliders;
        public RoArray<EnemyTag> enemyTag;
        public RoArray<ShipTag> shipTag;
        public RoArray<Entity> entities;
    }
}