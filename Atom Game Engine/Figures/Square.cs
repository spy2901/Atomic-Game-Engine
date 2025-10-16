using Atom_Game_Engine.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Atom_Game_Engine
{
    public class Square : IDisposable
    {
        private readonly float[] _vertices =
        {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        private int _vao;
        private int _vbo;
        private int _ebo;

        public Square()
        {
            // 1️⃣ Kreiraj i binduj VAO
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            // 2️⃣ Kreiraj i popuni VBO (verteksi)
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // 3️⃣ Kreiraj i popuni EBO (indeksi)
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // 4️⃣ Definiši vertex atribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // 5️⃣ Odveži (ali NE EBO, on ostaje vezan za VAO)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Render(Shader shader)
        {
            shader.Use();
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
