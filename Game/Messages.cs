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

    enum InputCommand
    {
        Fire
    }
    struct InputCommandMessage : IMessage
    {
        public InputCommand Command { get; private set; }

        public InputCommandMessage(InputCommand command)
        {
            Command = command;
        }
    }

}
