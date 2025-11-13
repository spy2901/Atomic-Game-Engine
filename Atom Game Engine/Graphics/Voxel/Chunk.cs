using OpenTK.Mathematics;

namespace Atom_Game_Engine.Graphics.Voxel
{
	public class Chunk
	{
		public const int CHUNK_SIZE = 16;
		public Vector3i Position { get; private set; }

		private Voxel[,,] _voxels;

		public Chunk(Vector3i position)
		{
			Position = position;
			_voxels = new Voxel[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

			Generate();
		}

        private void Generate()
        {
			// Za sada jednostavna zemlja donjih 8 blokova su dirt gornjih 8 je vazduh
			for (int x = 0; x < CHUNK_SIZE; x++)
			{
				for (int y = 0; y < CHUNK_SIZE; y++)
				{
					for (int z = 0; z < CHUNK_SIZE; z++)
					{

						if (y == CHUNK_SIZE - 1)
						{
							_voxels[x, y, z] = new Voxel(new Vector3i(x, y, z), false, BlockTypes.Grass);
						}
						else
						{

							_voxels[x, y, z] = new Voxel(new Vector3i(x, y, z), false, BlockTypes.Dirt);
						}
					}
				}
			}
        }

		public Voxel GetVoxel(int x,int y,int z)
		{
            if (x < 0 || y < 0 || z < 0 || x >= CHUNK_SIZE || y >= CHUNK_SIZE || z >= CHUNK_SIZE)
                return null;

            return _voxels[x, y, z];
        }

		public IEnumerable<Voxel> GetAll()
		{
			foreach (var v in _voxels)
			{
				yield return v;
			}
		}
    }
}

