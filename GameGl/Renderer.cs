using System;
using Game.Components.Transform;
using GameGl.Core;
using GameGl.Core.Shaders;
using GameGl.Core.Textures;
using GameGl.Properties;
using OpenGL;
using TinyEcs;

namespace GameGl
{
    public class Renderer
    {
        private BulletBatch bulletBatch = BulletBatch.Create();
        private ShipBatch shipBatch = ShipBatch.Create();
        private Background background = Background.Create();

        //private Framebuffer offscreenBuffer;
        //private VertexArray offscreenVertexArray;
        //private ShaderProgram offscreenProgram;

        private int playerBulletCount;
        private Position[] playerBulletPositions;

        private int playerCount;
        private Position[] playerPositions;

        private int enemyCount;
        private Position[] enemyPositions;

        //public Renderer()
        //{
        //    {
        //        var builder = new FramebufferBuilder();
        //        var texture = Texture2d.Create();
        //        using (BindLock.Bind(texture))
        //        {
        //            texture.Image(null, 0, InternalFormat.Rgba, 800, 800, 0, PixelFormat.Rgba, PixelType.UnsignedByte);
        //            texture.WrapS = TextureWrapMode.Repeat;
        //            texture.WrapT = TextureWrapMode.Repeat;
        //            texture.MagFilter = TextureMagFilter.Linear;
        //            texture.MinFilter = TextureMinFilter.Linear;
        //        }
        //        builder.Attach(texture, FramebufferAttachment.ColorAttachment0);
        //        builder.Attach(Renderbuffer.Create(InternalFormat.Depth24Stencil8, 800, 800), (FramebufferAttachment)Gl.DEPTH24_STENCIL8);
        //        offscreenBuffer = builder.Create();
        //    }

        //    {
        //        var builder = new VertexArrayBuilder();
        //        offscreenVertexArray = builder.Build();
        //    }

        //    offscreenProgram = ShaderProgram.LinkShaders(
        //        VertexShader.FromSource(Resources.PostProcessVertex), 
        //        FragmentShader.FromSource(Resources.PostProcessFrag));
            
        //}

        public void Render(float time)
        {
            //offscreenBuffer.Bind();
            //Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            background.Draw(time);
            shipBatch.Draw(playerPositions, playerCount, time);
            shipBatch.Draw(enemyPositions, enemyCount, time);
            bulletBatch.Draw(playerBulletPositions, playerBulletCount, time);
            //offscreenBuffer.Unbind();

            //Gl.ActiveTexture(TextureUnit.Texture0);
            ////Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //offscreenProgram.Use();
            //offscreenVertexArray.Bind();
            //Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
            //offscreenVertexArray.Unbind();
        }

        internal void SetPlayers(RoDataStream<Position> positions, int length)
        {
            playerCount = length;
            playerPositions = (Position[])positions;
        }

        internal void SetEnemies(RoDataStream<Position> positions, int length)
        {
            enemyPositions = (Position[])positions;
            enemyCount = length;
        }

        internal void SetPlayerBullets(RoDataStream<Position> positions, int length)
        {
            playerBulletCount = length;
            playerBulletPositions = (Position[])positions;
        }
    }
}
