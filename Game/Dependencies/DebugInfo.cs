using Game.Components;
using TinyEcs;

namespace Game.Dependencies
{
    public class DebugInfo
    {
        public float DeltaTime { get; set; }
        public int BulletCount { get; set; }
        public int PlayerCount { get; internal set; }
        public int ShipCount { get; internal set; }
        public int EnemyCount { get; set; }
    }

    public class DebugInfoSystem : ComponentSystem<UpdateMessage>
    {
        public class Bullets
        {
            public int length;
            public BulletTag bulletTag;
        }
        public class Ships
        {
            public int length;
            public ShipTag shipTag;
        }
        public class PlayerObjects
        {
            public int length;
            public PlayerTag playerTag;
        }
        public class EnemyObjects
        {
            public int length;
            public EnemyTag enemyTag;
        }
        [Group] public Bullets bullets;
        [Group] public Ships ships;
        [Group] public PlayerObjects players;
        [Group] public EnemyObjects enemies;

        private DebugInfo debugInfo;

        public DebugInfoSystem(DebugInfo debugInfo)
        {
            this.debugInfo = debugInfo;
        }

        protected override void Execute(World world, UpdateMessage message)
        {
            debugInfo.BulletCount = bullets.length;
            debugInfo.PlayerCount = players.length;
            debugInfo.ShipCount = ships.length;
            debugInfo.EnemyCount = enemies.length;
        }
    }
}