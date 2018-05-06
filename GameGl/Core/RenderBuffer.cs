using OpenGL;
using System;

namespace GameGl.Core
{
    public struct RenderBuffer : IBindable, IDisposable
    {
        private uint handle;
        public uint Handle => handle;

        public RenderBuffer(uint handle)
        {
            this.handle = handle;
        }

        public static RenderBuffer Create(InternalFormat format, int width, int height)
        {
            var renderBuffer = new RenderBuffer(Gl.CreateRenderbuffer());
            Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBuffer.handle);
            Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, format, width, height);
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
