using OpenGL;
using TinyEcs;

namespace Game
{
    public struct UpdateMessage : IMessage
    {
        public float DeltaTime { get; internal set; }

        public UpdateMessage(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    public struct LateUpdateMessage : IMessage
    {
        public float DeltaTime { get; internal set; }

        public LateUpdateMessage(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    public struct RenderMessage : IMessage { }

    public struct DetectCollisionsMessage : IMessage { }

    public struct InputMessage : IMessage
    {
        public InputCommands[] inputCommands;
        public Vertex2f[] directions;

        public InputMessage(InputCommands[] inputCommands, Vertex2f[] directions)
        {
            this.inputCommands = inputCommands;
            this.directions = directions;
        }
    }


}
