﻿using OpenGL;
using TinyEcs;

namespace Game.Components.Colliders
{
    public struct Circle : IComponent
    {
        public Vertex2f offset;
        public float radius;

        public Circle(Vertex2f offset, float radius)
        {
            this.offset = offset;
            this.radius = radius;
        }
    }
}
