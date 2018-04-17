using TinyEcs;

namespace Game
{
    class UpdateMessage : Message
    {
        private float deltaTime;

        public UpdateMessage(float deltaTime)
        {
            this.deltaTime = deltaTime;
        }

        public float DeltaTime { get => deltaTime; internal set => deltaTime = value; }
    }

    class Collision : Message
    {
        public readonly Entity entity1, entity2;
        public readonly object collider1, collider2;

        public Collision(Entity entity1, Entity entity2, object collider1, object collider2)
        {
            this.entity1 = entity1;
            this.entity2 = entity2;
            this.collider1 = collider1;
            this.collider2 = collider2;
        }
    }

    class InputCommandMessage : Message
    {

    }

}
