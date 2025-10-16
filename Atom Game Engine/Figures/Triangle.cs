using OpenTK.Graphics.OpenGL4;

namespace Atom_Game_Engine.Graphics
{

    public class Triangle : IDisposable
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

        private int _vbo;
        private int _vao;
        private int _ebo;

        public Triangle()
        {
            // 1️⃣ Kreiraj VAO
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            // 2️⃣ Kreiraj VBO
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // 3️⃣ Poveži atribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // 4️⃣ Odveži sve
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Render(Shader shader)
        {
            shader.Use();
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
        }
    }
}


