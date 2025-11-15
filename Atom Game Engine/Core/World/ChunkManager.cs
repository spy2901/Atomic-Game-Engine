using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using Atom_Game_Engine.Graphics.Voxel;
using Atom_Game_Engine.Graphics;

namespace Atom_Game_Engine.Core.World
{
    public class ChunkManager : IDisposable
    {
        // chunk size in voxels (treba da se podudara sa Chunk.CHUNK_SIZE)
        private readonly int _chunkSize;

        // koliko chunkova u svakom pravcu (radius)
        private readonly int _radius; // for 5x5 -> radius = 2

        // key: (chunkX, chunkZ)
        private readonly Dictionary<(int, int), (Chunk chunk, ChunkMesh mesh)> _chunks =
            new Dictionary<(int, int), (Chunk, ChunkMesh)>();

        private readonly Shader _shader;
        private readonly Texture _atlas;

        public ChunkManager(int chunkSize, int radius, Shader shader, Texture atlas)
        {
            _chunkSize = chunkSize;
            _radius = radius;
            _shader = shader;
            _atlas = atlas;
        }

        // Convert world position (float world coords) to chunk coords
        private (int cx, int cz) WorldToChunkCoords(Vector3 worldPos)
        {
            int cx = (int)Math.Floor(worldPos.X / _chunkSize);
            int cz = (int)Math.Floor(worldPos.Z / _chunkSize);
            return (cx, cz);
        }

        // Ensure chunks exist around cameraPos (world coords)
        public void Update(Vector3 cameraWorldPos)
        {
            var (centerCX, centerCZ) = WorldToChunkCoords(cameraWorldPos);

            var wanted = new HashSet<(int, int)>();

            for (int dx = -_radius; dx <= _radius; dx++)
                for (int dz = -_radius; dz <= _radius; dz++)
                {
                    int cx = centerCX + dx;
                    int cz = centerCZ + dz;
                    wanted.Add((cx, cz));
                    if (!_chunks.ContainsKey((cx, cz)))
                    {
                        LoadChunk(cx, cz);
                    }
                }

            // Unload chunks that are not wanted
            var toRemove = new List<(int, int)>();
            foreach (var key in _chunks.Keys)
            {
                if (!wanted.Contains(key))
                    toRemove.Add(key);
            }

            foreach (var k in toRemove)
            {
                UnloadChunk(k.Item1, k.Item2);
            }
        }

        // Create chunk and its mesh immediately (synchronous)
        private void LoadChunk(int cx, int cz)
        {
            // world origin of chunk in voxels: cx * chunkSize, cz * chunkSize
            var chunk = new Chunk(cx, cz); // expects Chunk(int worldX, int worldZ) and auto-Generate()
            var mesh = new ChunkMesh(_shader, _atlas, chunk);
            _chunks[(cx, cz)] = (chunk, mesh);
            Console.WriteLine($"LOADING CHUNK: {cx}, {cz}");

        }

        private void UnloadChunk(int cx, int cz)
        {
            if (_chunks.TryGetValue((cx, cz), out var pair))
            {
                pair.mesh.Dispose();
                // pair.chunk.Dispose() if you implement IDisposable there
                _chunks.Remove((cx, cz));
            }
        }

        public void RenderAll()
        {
            foreach (var kv in _chunks)
            {
                var (cx, cz) = kv.Key;

                Vector3 offset = new Vector3(
                    cx * Chunk.CHUNK_SIZE,
                    0,
                    cz * Chunk.CHUNK_SIZE
                );

                kv.Value.mesh.Render(offset);
            }
        }

        public void Dispose()
        {
            foreach (var kv in _chunks.Values)
            {
                kv.mesh.Dispose();
            }
            _chunks.Clear();
        }
    }
}
