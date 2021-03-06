﻿using System.Numerics;
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

    //public struct DamageMessage : IMessage
    //{
    //    public Entity Source;
    //    public Entity Target;

    //    public float Value;

    //    public DamageMessage(Entity source, Entity target, float value)
    //    {
    //        Source = source;
    //        Target = target;
    //        Value = value;
    //    }
    //}

    public struct InputMessage : IMessage
    {
        public InputCommands[] inputCommands;
        public Vector2[] directions;

        public InputMessage(InputCommands[] inputCommands, Vector2[] directions)
        {
            this.inputCommands = inputCommands;
            this.directions = directions;
        }
    }


}
