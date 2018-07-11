using Game.Components.Player;
using Game.Components.Transform;
using System.Numerics;
using TinyEcs;

namespace Game.Systems
{
    public class InputSystem : ComponentSystem<InputMessage>
    {
        public class Data
        {
            public int Length;
            public RwData<Input> Inputs;
            public RoData<PlayerInfo> PlayerInfos;
            public RwData<Heading> Headings;
        }
        [Group] public Data data;

        protected override void Execute(World world, InputMessage message)
        {
            for (var i = 0; i < data.Length; i++)
            {
                var id = data.PlayerInfos[i].Id;
                data.Inputs[i].Commands = message.inputCommands[id];
                data.Inputs[i].Direction = message.directions[id];
            }

            for (var i = 0; i < data.Length; i++)
            {
                if ((data.Inputs[i].Commands & InputCommands.MoveRight) > 0)
                {
                    data.Inputs[i].Direction.X += 1;
                }
                if ((data.Inputs[i].Commands & InputCommands.MoveLeft) > 0)
                {
                    data.Inputs[i].Direction.X -= 1;
                }
                if ((data.Inputs[i].Commands & InputCommands.MoveUp) > 0)
                {
                    data.Inputs[i].Direction.Y += 1;
                }
                if ((data.Inputs[i].Commands & InputCommands.MoveDown) > 0)
                {
                    data.Inputs[i].Direction.Y -= 1;
                }
            }

            for (var i = 0; i < data.Length; i++)
            {
                if (data.Inputs[i].Direction.LengthSquared() > 0)
                {
                    data.Headings[i].Vector = Vector2.Normalize(data.Inputs[i].Direction) * data.PlayerInfos[i].Speed;
                }
                else
                {
                    data.Headings[i].Vector = default;
                }
            }
        }
    }
}
