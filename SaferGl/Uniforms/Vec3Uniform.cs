using System;
using System.Collections.Generic;
using System.Text;

namespace SaferGl.Uniforms
{
    public struct Vec3Uniform : IUniform
    {
        public int Location { get; }

        public Vec3Uniform(int location)
        {
            Location = location;
        }
    }
}
