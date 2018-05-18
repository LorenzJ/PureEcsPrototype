using System;
using System.Collections.Generic;
using System.Text;

namespace SaferGl.Uniforms
{
    public struct Mat4Uniform : IUniform
    {
        public int Location { get; }

        public Mat4Uniform(int location) : this()
        {
            Location = location;
        }
    }
}
