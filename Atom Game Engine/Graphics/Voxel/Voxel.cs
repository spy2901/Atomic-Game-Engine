using System;
using OpenTK.Mathematics;

namespace Atom_Game_Engine.Graphics.Voxel
{
	public class Voxel
	{
		public Vector3i Position { get; set; }
        public bool IsAir { get; }

        public VoxelBlock BlockType { get; }

        public Voxel(Vector3i position, bool isAir, VoxelBlock blockType)
		{
			Position = position;
            IsAir = isAir;
            BlockType = blockType;
        }
	}
}

