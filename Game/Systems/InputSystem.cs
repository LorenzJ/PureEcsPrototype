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
            public int length;
            public RwDataStream<Input> inputs;
            public RoDataStream<PlayerInfo> playerInfos;
            public RwDataStream<Heading> headings;
        }
        [Group] public Data data;

        protected override void Execute(World world, InputMessage message)
        {
            for (var i = 0; i < data.length; i++)
            {
                var id = data.playerInfos[i].id;
                data.inputs[i].commands = message.inputCommands[id];
                data.inputs[i].direction = message.directions[id];
            }

            for (var i = 0; i < data.length; i++)
            {
                if ((data.inputs[i].commands & InputCommands.MoveRight) > 0)
                {
                    data.inputs[i].direction.X += 1;
                }
                if ((data.inputs[i].commands & InputCommands.MoveLeft) > 0)
                {
                    data.inputs[i].direction.X -= 1;
                }
                if ((data.inputs[i].commands & InputCommands.MoveUp) > 0)
                {
                    data.inputs[i].direction.Y += 1;
                }
                if ((data.inputs[i].commands & InputCommands.MoveDown) > 0)
                {
                    data.inputs[i].direction.Y -= 1;
                }
            }

            for (var i = 0; i < data.length; i++)
            {
                if (data.inputs[i].direction.LengthSquared() > 0)
                {
                    data.headings[i].vector = Vector2.Normalize(data.inputs[i].direction) * data.playerInfos[i].speed;
                }
                else
                {
                    data.headings[i].vector = default;
                }
            }
        }
    }
}
