using Game.Components;
using Game.Components.Colliders;
using Game.Components.Enemy;
using Game.Components.Player;
using Game.Components.Transform;
using System.Numerics;
using TinyEcs;

namespace Game
{
    class ShipFactory
    {
        private World world;
        private Archetype shipType;
        private Archetype playerShipType;
        private Archetype enemyShipType;

        public ShipFactory(World world)
        {
            this.world = world;
            shipType = world.CreateArchetype(typeof(ShipTag), typeof(Position), typeof(Heading), typeof(Circle));
            playerShipType = world.CreateArchetype(shipType, typeof(PlayerTag), typeof(Input), typeof(WeaponState), typeof(PlayerInfo));
            enemyShipType = world.CreateArchetype(shipType, typeof(EnemyTag), typeof(Health), typeof(EnemyInfo));
        }

        public Entity CreateAndAddPlayer(int id)
        {
            var player = world.CreateEntity(playerShipType);
            world.Ref<Position>(player).Vector = new Vector2(0f, -.5f);
            world.Ref<Circle>(player) = new Circle(default, .02f);
            world.Ref<Heading>(player) = default;
            world.Ref<WeaponState>(player) = new WeaponState { Frequency = .2f, Power = 2f, Timeout = 0f };
            world.Ref<PlayerInfo>(player) = new PlayerInfo { Id = id, Lives = 3, Speed = .5f };
            world.Ref<Input>(player) = default;
            return player;
        }

        public Entity CreateAndAddEnemy(Position position)
        {
            var enemy = world.CreateEntity(enemyShipType);
            world.Ref<Position>(enemy) = position;
            world.Ref<Circle>(enemy) = new Circle(default, .08f);
            world.Ref<Heading>(enemy) = new Heading(new Vector2(0f, -.4f));
            world.Ref<Health>(enemy) = new Health(5f);
            world.Ref<EnemyInfo>(enemy) = new EnemyInfo { Value = 100 };
            return enemy;
        }

    }
}
