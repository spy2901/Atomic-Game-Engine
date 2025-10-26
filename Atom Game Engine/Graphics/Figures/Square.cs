using Atom_Game_Engine.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Atom_Game_Engine
{
    public class Square : IDisposable
    {
        private readonly float[] _vertices =
        {
            // Positions        // Texture Coords
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3, // first triangle
            1, 2, 3  // second triangle
        };

        private int _vao, _vbo, _ebo;
        private Texture _texture1;
        private Texture _texture2;

        public Square()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture coord attribute
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Load texture
            _texture1 = new Texture("./Assets/container.jpg");
            _texture2 = new Texture("./Assets/wall.jpg");
        }

        public void Render(Shader shader)
        {
            shader.Use();
            shader.SetInt("texture1", 0); // sampler1 koristi TextureUnit0
            shader.SetInt("texture2", 1); // sampler2 koristi TextureUnit1

            _texture1.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
        }
    }
}
