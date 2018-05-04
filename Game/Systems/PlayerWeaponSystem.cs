﻿using Game.Components;
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
            public int Length;
            public RoDataStream<Input> Inputs;
            public RoDataStream<Position> Positions;
            public RwDataStream<WeaponState> Weapons;
        }
        [Group] public Data data;

        public PlayerWeaponSystem(BulletSpawner bulletSpawner)
        {
            this.bulletSpawner = bulletSpawner;
            
        }
        protected override void Execute(World world, UpdateMessage message)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data.Weapons[i].Timeout -= message.DeltaTime;
            }

            for (var i = 0; i < data.Length; i++)
            {
                if ((data.Inputs[i].Commands & InputCommands.Fire) > 0 && data.Weapons[i].Timeout <= 0)
                {
                    bulletSpawner.Spawn(
                        new BulletSpawner.BulletCommand(data.Positions[i], 
                        new Heading(new Vector2(0, 1)), 
                        BulletSpawner.BulletType.Player));
                    data.Weapons[i].Timeout = data.Weapons[i].Frequency;
                }
            }
        }
    }
}