using Game.Components;
using Game.Components.Player;
using TinyEcs;

namespace Game.Dependencies
{
    public class DebugInfo
    {
        public float DeltaTime { get; set; }
        public int BulletCount { get; internal set; }
        public int PlayerCount { get; internal set; }
        public int ShipCount { get; internal set; }
        public int EnemyCount { get; internal set; }
    }

    public class DebugInfoSystem : ComponentSystem<UpdateMessage>
    {
        public class Bullets
        {
            public int Length;
            public BulletTag BulletTag;
        }
        public class Ships
        {
            public int Length;
            public ShipTag ShipTag;
        }
        public class PlayerObjects
        {
            public int Length;
            public PlayerTag PlayerTag;
        }
        public class EnemyObjects
        {
            public int Length;
            public EnemyTag EnemyTag;
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
            debugInfo.BulletCount = bullets.Length;
            debugInfo.PlayerCount = players.Length;
            debugInfo.ShipCount = ships.Length;
            debugInfo.EnemyCount = enemies.Length;
        }
    }
}