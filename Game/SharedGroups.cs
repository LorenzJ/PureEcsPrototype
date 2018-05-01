using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using TinyEcs;

namespace Game
{
    public class EnemyBullets
    {
        public int length;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public EnemyTag enemyTag;
        public BulletTag bulletTag;
        public RoDataStream<Entity> entities;
    }

    public class PlayerBullets
    {
        public int length;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public PlayerTag playerTag;
        public BulletTag bulletTag;
        public RoDataStream<Entity> entities;
    }

    public class Players
    {
        public int length;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public PlayerTag playerTag;
        public ShipTag shipTag;
        public RoDataStream<Entity> entities;
    }

    public class Enemies
    {
        public int length;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public EnemyTag enemyTag;
        public ShipTag shipTag;
        public RoDataStream<Entity> entities;
    }
}