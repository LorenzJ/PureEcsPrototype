using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using TinyEcs;

namespace Game
{
    public class EnemyBullets
    {
        public int length;
        public RoDataStream<Entity> entities;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public EnemyTag enemyTag;
        public BulletTag bulletTag;
    }

    public class PlayerBullets
    {
        public int length;
        public RoDataStream<Entity> entities;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public PlayerTag playerTag;
        public BulletTag bulletTag;
    }

    public class Players
    {
        public int length;
        public RoDataStream<Entity> entities;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public PlayerTag playerTag;
        public ShipTag shipTag;
    }

    public class Enemies
    {
        public int length;
        public RoDataStream<Entity> entities;
        public RoDataStream<Position> positions;
        public RoDataStream<Circle> colliders;
        public EnemyTag enemyTag;
        public ShipTag shipTag;
    }
}