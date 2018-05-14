using OpenGL;
using System;

namespace GameGl.Core
{
    public struct Renderbuffer : IBindable, IHandle, IDisposable
    {
        public uint Handle { get; }

        public Renderbuffer(uint handle)
        {
            Handle = handle;
        }

        public static Renderbuffer Create(InternalFormat format, int width, int height)
        {
            var renderBuffer = new Renderbuffer(Gl.CreateRenderbuffer());
            renderBuffer.Bind();
            Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, format, width, height);
            renderBuffer.Unbind();
            return renderBuffer;
        }

        public void Bind()
        {
            Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
        }

        public void Dispose()
        {
            Gl.DeleteRenderbuffers(new uint[] { Handle });
        }

        public void Unbind()
        {
            Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0u);
        }
    }
}
