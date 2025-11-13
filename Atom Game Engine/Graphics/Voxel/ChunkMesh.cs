using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Atom_Game_Engine.Graphics.Voxel
{
	public class ChunkMesh
	{
        private int _vao;
        private int _vbo;
        private int _vertexCount;
        private Texture _atlas;
        private Shader _shader;

        public ChunkMesh(Shader shader, Texture atlas,Chunk chunk)
        {
            _shader = shader;
            _atlas = atlas;
            Generate(chunk);
		}

		private void Generate(Chunk chunk)
		{
            List<float> vertices = new();

            // Prolazimo kroz sve voxele
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {
                        var voxel = chunk.GetVoxel(x, y, z);
                        if (voxel == null || voxel.IsAir)
                            continue;

                        // Dodajemo samo vidljive strane
                        AddVisibleFaces(vertices, chunk, voxel);
                    }
                }
            }

            _vertexCount = vertices.Count / 5; // 3 za poziciju + 2 za UV

            // OpenGL setup
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            // layout (location = 0) → position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // layout (location = 1) → texcoord
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }


        private void AddVisibleFaces(List<float> vertices, Chunk chunk, Voxel voxel)
        {
            Vector3i pos = voxel.Position;

            // Smatramo stranu vidljivom ako nema susednog voxela
            bool front = chunk.GetVoxel(pos.X, pos.Y, pos.Z + 1)?.IsAir ?? true;
            bool back = chunk.GetVoxel(pos.X, pos.Y, pos.Z - 1)?.IsAir ?? true;
            bool left = chunk.GetVoxel(pos.X - 1, pos.Y, pos.Z)?.IsAir ?? true;
            bool right = chunk.GetVoxel(pos.X + 1, pos.Y, pos.Z)?.IsAir ?? true;
            bool top = chunk.GetVoxel(pos.X, pos.Y + 1, pos.Z)?.IsAir ?? true;
            bool bottom = chunk.GetVoxel(pos.X, pos.Y - 1, pos.Z)?.IsAir ?? true;

            float x = pos.X;
            float y = pos.Y;
            float z = pos.Z;

            // dodaj svaku stranu koja je vidljiva
            if (front) AddFace(vertices, x, y, z, "front", voxel);
            if (back) AddFace(vertices, x, y, z, "back", voxel);
            if (left) AddFace(vertices, x, y, z, "left", voxel);
            if (right) AddFace(vertices, x, y, z, "right", voxel);
            if (top) AddFace(vertices, x, y, z, "top", voxel);
            if (bottom) AddFace(vertices, x, y, z, "bottom", voxel);
        }

        private void AddFace(List<float> v, float x, float y, float z, string face, Voxel voxel)
        {
            // 1️⃣ — uzmi blok i njegove teksture
            VoxelBlock block = voxel.BlockType;

            Vector2i tile = face switch
            {
                "top" => block.Top,
                "bottom" => block.Bottom,
                "front" => block.North,
                "back" => block.South,
                "left" => block.West,
                "right" => block.East,
                _ => new Vector2i(0, 0)
            };

            // 2️⃣ — uzmi UV iz atlasa (16×16 atlas)
            Vector2[] uv = AtlasHelper.GetUV(tile.X, tile.Y, 16);

            // 3️⃣ — postavi vertexe po strani (X,Y,Z + UV)
            float[] faceVertices = face switch
            {
                "front" => new float[]
                {
            x, y, z + 1,   uv[0].X, uv[0].Y,
            x + 1, y, z + 1, uv[1].X, uv[1].Y,
            x + 1, y + 1, z + 1, uv[2].X, uv[2].Y,
            x + 1, y + 1, z + 1, uv[2].X, uv[2].Y,
            x, y + 1, z + 1,   uv[3].X, uv[3].Y,
            x, y, z + 1,       uv[0].X, uv[0].Y
                },

                "back" => new float[]
                {
            x + 1, y, z,   uv[0].X, uv[0].Y,
            x, y, z,       uv[1].X, uv[1].Y,
            x, y + 1, z,   uv[2].X, uv[2].Y,
            x, y + 1, z,   uv[2].X, uv[2].Y,
            x + 1, y + 1, z, uv[3].X, uv[3].Y,
            x + 1, y, z,     uv[0].X, uv[0].Y
                },

                "left" => new float[]
                {
            x, y, z,         uv[0].X, uv[0].Y,
            x, y, z + 1,     uv[1].X, uv[1].Y,
            x, y + 1, z + 1, uv[2].X, uv[2].Y,
            x, y + 1, z + 1, uv[2].X, uv[2].Y,
            x, y + 1, z,     uv[3].X, uv[3].Y,
            x, y, z,         uv[0].X, uv[0].Y
                },

                "right" => new float[]
                {
            x + 1, y, z + 1, uv[0].X, uv[0].Y,
            x + 1, y, z,     uv[1].X, uv[1].Y,
            x + 1, y + 1, z, uv[2].X, uv[2].Y,
            x + 1, y + 1, z, uv[2].X, uv[2].Y,
            x + 1, y + 1, z + 1, uv[3].X, uv[3].Y,
            x + 1, y, z + 1, uv[0].X, uv[0].Y
                },

                "top" => new float[]
                {
            x, y + 1, z + 1, uv[0].X, uv[0].Y,
            x + 1, y + 1, z + 1, uv[1].X, uv[1].Y,
            x + 1, y + 1, z,     uv[2].X, uv[2].Y,
            x + 1, y + 1, z,     uv[2].X, uv[2].Y,
            x, y + 1, z,         uv[3].X, uv[3].Y,
            x, y + 1, z + 1,     uv[0].X, uv[0].Y
                },

                "bottom" => new float[]
                {
            x, y, z,         uv[0].X, uv[0].Y,
            x + 1, y, z,     uv[1].X, uv[1].Y,
            x + 1, y, z + 1, uv[2].X, uv[2].Y,
            x + 1, y, z + 1, uv[2].X, uv[2].Y,
            x, y, z + 1,     uv[3].X, uv[3].Y,
            x, y, z,         uv[0].X, uv[0].Y
                },

                _ => Array.Empty<float>()
            };

            v.AddRange(faceVertices);
        }


        public void Render()
        {

            _atlas.Use(TextureUnit.Texture0);
            _shader.Use();
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
        }
    }
}

