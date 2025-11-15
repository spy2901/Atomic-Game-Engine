
namespace Atom_Game_Engine.Core.World
{
	public static class Noise
	{
        public static float Perlin2D(float x, float z, float scale = 20f, float height = 8f)
        {
            float nx = x * scale;
            float nz = z * scale;

            float noise = PerlinNoise.Noise(nx, nz);  // koristi naš PerlinNoise

            // map [-1, 1] → [0, 1]
            noise = (noise + 1f) / 2f;

            return noise * height;
        }
    }
}

