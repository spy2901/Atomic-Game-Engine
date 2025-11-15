using System;
using OpenTK.Mathematics;

namespace Atom_Game_Engine.Graphics.Voxel
{
    public class VoxelBlock
    {

        public Vector2i Top;
        public Vector2i Bottom;
        public Vector2i North;
        public Vector2i South;
        public Vector2i East;
        public Vector2i West;

        public VoxelBlock(Vector2i all)
        {
            Top = Bottom = North = South = East = West = all;
        }

        public VoxelBlock(Vector2i top, Vector2i bottom, Vector2i side)
        {
            Top = top;
            Bottom = bottom;
            North = South = East = West = side;
        }


    }
    public static class BlockTypes
    {
        public static readonly VoxelBlock Air =
            new VoxelBlock(new Vector2i(-1, -1));
        public static readonly VoxelBlock Dirt =
            new VoxelBlock(new Vector2i(2, 15));   // sve strane isti tile

        public static readonly VoxelBlock Grass =
            new VoxelBlock(
                new Vector2i(7, 13),   // top
                new Vector2i(2, 15),   // bottom (dirt)
                new Vector2i(3, 15));  // sides

        public static readonly VoxelBlock Stone =
            new VoxelBlock(new Vector2i(1, 15));

        public static readonly VoxelBlock Sand =
            new VoxelBlock(new Vector2i(2, 14));

        public static readonly VoxelBlock OakLog =
            new VoxelBlock(
                new Vector2i(5, 15), // top/bottomb v                            
                new Vector2i(4, 14),
                new Vector2i(4, 14)  // sides
            );

        public static readonly VoxelBlock OakPlanks =
            new VoxelBlock(new Vector2i(4, 15));

        public static readonly VoxelBlock Leaves =
            new VoxelBlock(new Vector2i(4, 3));

        public static readonly VoxelBlock Water =
            new VoxelBlock(new Vector2i(14, 1));

        public static readonly VoxelBlock Bedrock =
            new VoxelBlock(new Vector2i(1, 14));
    }
}


