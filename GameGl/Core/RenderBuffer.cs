using OpenGL;
using System;

namespace GameGl.Core
{
    public struct Renderbuffer : IBindable, IHandle, IDisposable
    {
        private uint handle;
        public uint Handle => handle;

        public Renderbuffer(uint handle)
        {
            this.handle = handle;
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
            Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, handle);
        }

        public void Dispose()
        {
            Gl.DeleteRenderbuffers(new uint[] { handle });
        }

        public void Unbind()
        {
            Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0u);
        }
    }
}
