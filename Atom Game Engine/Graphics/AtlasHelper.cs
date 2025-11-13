using System;
using OpenTK.Mathematics;

namespace Atom_Game_Engine.Graphics
{
	public static class AtlasHelper
	{
        
            public static Vector2[] GetUV(int tileX, int tileY, int atlasSize = 16)
            {
                float unit = 1f / atlasSize;

                float uMin = tileX * unit;
                float vMin = tileY * unit;
                float uMax = uMin + unit;
                float vMax = vMin + unit;

                return new[]
                {
                    new Vector2(uMin, vMin),
                    new Vector2(uMax, vMin),
                    new Vector2(uMax, vMax),
                    new Vector2(uMin, vMax)
                    
            };    
        }
    }
}

