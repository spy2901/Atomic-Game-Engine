using OpenTK.Graphics.OpenGL4;

namespace Atom_Game_Engine.Graphics
{

    public class Triangle : IDisposable
    {
        float[] _vertices =
        {
            // pozicija       // texture coords
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f,  // gore desno
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // dole desno
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,  // dole levo
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f   // gore levo
        };
        /*float[] _vertices = {
            //  pozicija (x, y, z)   // boja (r, g, b)
             0.5f, -0.5f, 0.0f,      1.0f, 0.0f, 0.0f,  // desno dole – crvena
            -0.5f, -0.5f, 0.0f,      0.0f, 1.0f, 0.0f,  // levo dole – zelena
             0.0f,  0.5f, 0.0f,      0.0f, 0.0f, 1.0f   // gore – plava
        };*/

        private int _vbo;
        private int _vao;

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
            // Pozicija (atribut 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture koordinate (atribut 2)
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);



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


