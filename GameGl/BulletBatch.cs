using Game.Components.Transform;
using GameGl.Properties;
using OpenGL;
using System;
using System.Runtime.InteropServices;
using System.Text;
using TinyEcs;

namespace GameGl
{
    public class BulletBatch
    {
        const int positionIndex = 0;
        const int offsetIndex = 1;

        uint vao;
        uint vbo;
        uint ivbo;
        uint program;
        int timeUniform;

        private BulletBatch(uint vao, uint vbo, uint ivbo, uint program, int timeUniform)
        {
            this.vao = vao;
            this.vbo = vbo;
            this.ivbo = ivbo;
            this.program = program;
            this.timeUniform = timeUniform;
        }

        public void Draw(RoArray<Position> positions, int length, float time)
        {
            Gl.UseProgram(program);
            Gl.Uniform1(timeUniform, time);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, ivbo);
            Gl.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (uint)(Marshal.SizeOf<Position>() * length), (Position[])positions);
            Gl.BindVertexArray(vao);
            Gl.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, length);
            Gl.BindVertexArray(0);
        }

        public static BulletBatch CreateBulletBatch()
        {
            return NewBulletBatch();

            BulletBatch NewBulletBatch()
            {
                var program = CreateProgram();
                var timeUniform = GetTimeUniform(program);
                var vbo = CreateVertexBuffer();
                var ivbo = CreateInstanceBuffer();
                var vao = CreateVertexArray(vbo, ivbo);
                return new BulletBatch(vao, vbo, ivbo, program, timeUniform);
            }

            int GetTimeUniform(uint program)
            {
                return Gl.GetUniformLocation(program, "uTime");
            }

            uint CreateProgram()
            {
                var vertexShader = Gl.CreateShader(ShaderType.VertexShader);
                var fragShader = Gl.CreateShader(ShaderType.FragmentShader);
                
                Gl.ShaderSource(vertexShader, new string[] { Resources.BulletVertex });
                Gl.ShaderSource(fragShader, new string[] { Resources.BulletFrag });
                Gl.CompileShader(vertexShader);
                Gl.CompileShader(fragShader);

                Gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out var compileStatus);
                const int maxLength = 1024;
                if (compileStatus == 0)
                {
                    var infoLog = new StringBuilder(maxLength);
                    Gl.GetShaderInfoLog(vertexShader, maxLength, out var length, infoLog);
                    throw new ShaderCompilationException($"Bullet vertex shader error: {infoLog.ToString()}");
                }
                Gl.GetShader(fragShader, ShaderParameterName.CompileStatus, out compileStatus);
                if (compileStatus == 0)
                {
                    var infoLog = new StringBuilder(maxLength);
                    Gl.GetShaderInfoLog(fragShader, maxLength, out var length, infoLog);
                    throw new ShaderCompilationException($"Bullet fragment shader error: {infoLog}");
                }

                var program = Gl.CreateProgram();
                Gl.AttachShader(program, vertexShader);
                Gl.AttachShader(program, fragShader);
                Gl.LinkProgram(program);

                Gl.GetProgram(program, ProgramProperty.LinkStatus, out var linkStatus);
                if (linkStatus == 0)
                {
                    var infoLog = new StringBuilder(maxLength);
                    Gl.GetProgramInfoLog(program, maxLength, out var length, infoLog);
                    throw new ProgramLinkingException($"Bullet program error: {infoLog}");
                }

                Gl.DeleteShader(vertexShader);
                Gl.DeleteShader(fragShader);
                return program;
            }

            uint CreateVertexBuffer()
            {
                var vertices = new float[]
                {
                    0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, -0.5f, 0.5f
                };

                var vbo = Gl.CreateBuffer();
                Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices, BufferUsage.StaticDraw);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
                return vbo;
            }

            uint CreateInstanceBuffer()
            {
                var ivbo = Gl.CreateBuffer();
                Gl.BindBuffer(BufferTarget.ArrayBuffer, ivbo);
                Gl.BufferData(BufferTarget.ArrayBuffer, (uint)Marshal.SizeOf<Position>() * 4096u, null, BufferUsage.DynamicDraw);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
                return ivbo;
            }

            uint CreateVertexArray(uint vbo, uint ivbo)
            {
                var vao = Gl.CreateVertexArray();
                Gl.BindVertexArray(vao);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                Gl.VertexAttribPointer(positionIndex, 2, VertexAttribType.Float, false, 0, IntPtr.Zero);
                Gl.EnableVertexAttribArray(positionIndex);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, ivbo);
                Gl.VertexAttribPointer(offsetIndex, 2, VertexAttribType.Float, false, 0, IntPtr.Zero);
                Gl.EnableVertexAttribArray(offsetIndex);
                Gl.VertexAttribDivisor(offsetIndex, 1);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
                Gl.BindVertexArray(0);
                
                return vao;
            }
        }
    }
       
}
