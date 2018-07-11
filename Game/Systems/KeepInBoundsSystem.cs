using Game.Components;
using Game.Components.Transform;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using TinyEcs;

namespace Game.Systems
{
    public class KeepInBoundsSystem : ComponentSystem<LateUpdateMessage>
    {
        public class Data
        {
            public int Length;
            public RwData<Position> Positions;
            public PlayerTag playerTag;
            public ShipTag shipTag;
        }
        [Group] public Data data;

        protected override void Execute(World world, LateUpdateMessage message)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data.Positions[i].Vector = Vector2.Clamp(data.Positions[i].Vector, new Vector2(-.95f, -1.25f), new Vector2(.95f, 1.25f));
            }
        }
    }
}
