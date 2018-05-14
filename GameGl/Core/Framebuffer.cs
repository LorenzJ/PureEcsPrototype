using GameGl.Core.Textures;
using OpenGL;
using System;

namespace GameGl.Core
{
    public struct Framebuffer : IBindable, IHandle, IDisposable
    {
        public uint Handle { get; }

        internal Framebuffer(uint handle)
        {
            Handle = handle;
        }

        public void Bind()
        {
            Gl.BindFramebuffer(FramebufferTarget.DrawFramebuffer, Handle);
        }

        public void Unbind()
        {
            Gl.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0u);
        }

        public void Dispose()
        {
            Gl.DeleteFramebuffers(new uint[] { Handle });
        }
    }

    public class FramebufferBuilder
    {
        private Framebuffer frameBuffer;

        public FramebufferBuilder()
        {
            frameBuffer = new Framebuffer(Gl.GenFramebuffer());
            Gl.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer.Handle);
        }

        public void Attach(Texture2d texture, FramebufferAttachment framebufferAttachment)
        {
            Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, framebufferAttachment, TextureTarget.Texture2d, texture.Handle, 0);
        }

        public void Attach(Renderbuffer renderBuffer, FramebufferAttachment framebufferAttachment)
        {
            Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, framebufferAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.Handle);
        }

        public Framebuffer Create()
        {
            var status = Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferStatus.FramebufferComplete)
            {
                throw new FramebufferCreationException($"Framebuffer not completed, status: {status}");
            }
            Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0u);
            return frameBuffer;
        }
    }
}
