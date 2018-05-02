using Game.Components.Player;
using Game.Components.Transform;
using Game.Dependencies;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TinyEcs;

namespace Game.Systems
{
    public class PlayerWeaponSystem : ComponentSystem<UpdateMessage>
    {
        private BulletSpawner bulletSpawner;

        public class Data
        {
            public int length;
            public RoDataStream<Input> inputs;
            public RoDataStream<Position> positions;
            public RwDataStream<PlayerInfo> infos;
        }
        [Group] public Data data;

        public PlayerWeaponSystem(BulletSpawner bulletSpawner)
        {
            this.bulletSpawner = bulletSpawner;
            
        }
        protected override void Execute(World world, UpdateMessage message)
        {
            for (var i = 0; i < data.length; i++)
            {
                data.infos[i].fireTimeout -= message.DeltaTime;
            }

            for (var i = 0; i < data.length; i++)
            {
                if ((data.inputs[i].commands & InputCommands.Fire) > 0 && data.infos[i].fireTimeout <= 0)
                {
                    bulletSpawner.Spawn(
                        new BulletSpawner.BulletCommand(data.positions[i], 
                        new Heading(new Vector2(0, 1)), 
                        BulletSpawner.BulletType.Player));
                    data.infos[i].fireTimeout = 0.2f;
                }
            }
        }
    }
}
