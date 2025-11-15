using Atom_Game_Engine.Core.World;
using OpenTK.Mathematics;

namespace Atom_Game_Engine.Graphics.Voxel
{
    public class Chunk
    {
        public const int CHUNK_SIZE = 16;
        public const int CHUNK_HEIGHT = 70;

        public int WorldX { get; }
        public int WorldZ { get; }

        private Voxel[,,] _voxels;

        public Chunk(int worldX, int worldZ)
        {
            _voxels = new Voxel[CHUNK_SIZE, CHUNK_HEIGHT, CHUNK_SIZE];
            WorldX = worldX;
            WorldZ = worldZ;
            Generate();
        }

        private void Generate()
        {
            int minHeight = 40;
            int maxHeight = CHUNK_HEIGHT - 1;

            for (int x = 0; x < CHUNK_SIZE; x++)
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    int globalX = WorldX * CHUNK_SIZE + x;
                    int globalZ = WorldZ * CHUNK_SIZE + z;

                    float n = PerlinNoise.Noise(globalX * 0.01f, globalZ * 0.01f);
                    n = (n + 1f) * 0.5f;

                    int height = (int)(minHeight + n * (maxHeight - minHeight));

                    for (int y = 0; y < CHUNK_HEIGHT; y++)
                    {
                        VoxelBlock block;
                        bool isAir = false;

                        if (y == 0) block = BlockTypes.Bedrock;
                        else if (y > height) { block = BlockTypes.Air; isAir = true; }
                        else if (y == height) block = BlockTypes.Grass;
                        else if (y >= height - 4) block = BlockTypes.Dirt;
                        else block = BlockTypes.Stone;

                        _voxels[x, y, z] = new Voxel(new Vector3i(x, y, z), isAir, block);
                    }
                }
        }

        public Voxel GetVoxel(int x, int y, int z)
        {
            if (x < 0 || x >= CHUNK_SIZE ||
                y < 0 || y >= CHUNK_HEIGHT ||
                z < 0 || z >= CHUNK_SIZE)
                return null;

            return _voxels[x, y, z];
        }

        public IEnumerable<Voxel> GetAll()
        {
            foreach (var v in _voxels)
                yield return v;
        }
    }
}
