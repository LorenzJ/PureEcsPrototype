using TinyEcs;

namespace Game
{
    class UpdateMessage : Message
    {
        public float DeltaTime { get; internal set; }

        public UpdateMessage(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    internal class LateUpdateMessage : Message
    {
        public float DeltaTime { get; internal set; }

        public LateUpdateMessage(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    class Collision : Message
    {
        public Entity Entity1 { get; }
        public Entity Entity2 { get; private set; }
        public object Collider1 { get; }
        public object Collider2 { get; private set; }

        public Collision(Entity entity1, Entity entity2, object collider1, object collider2)
        {
            Entity1 = entity1;
            Entity2 = entity2;
            Collider1 = collider1;
            Collider2 = collider2;
        }
    }

    enum InputCommand
    {
        Fire
    }
    class InputCommandMessage : Message
    {
        public InputCommand Command { get; private set; }

        public InputCommandMessage(InputCommand command)
        {
            Command = command;
        }
    }

}
