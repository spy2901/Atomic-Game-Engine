using System;

namespace Atom_Game_Engine.Core.World
{
    public static class PerlinNoise
    {
        private static int Hash(int x, int y)
        {
            int h = x * 374761393 + y * 668265263;
            h = (h ^ (h >> 13)) * 1274126177;
            return h ^ (h >> 16);
        }

        private static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        private static float Grad(int hash, float x, float y)
        {
            switch (hash & 3)
            {
                case 0: return x + y;
                case 1: return x - y;
                case 2: return -x + y;
                default: return -x - y;
            }
        }

        public static float Noise(float x, float y)
        {
            int X = (int)MathF.Floor(x);
            int Y = (int)MathF.Floor(y);

            float xf = x - X;
            float yf = y - Y;

            float u = Fade(xf);
            float v = Fade(yf);

            int aa = Hash(X, Y);
            int ab = Hash(X, Y + 1);
            int ba = Hash(X + 1, Y);
            int bb = Hash(X + 1, Y + 1);

            float x1 = Lerp(Grad(aa, xf, yf),
                            Grad(ba, xf - 1, yf), u);

            float x2 = Lerp(Grad(ab, xf, yf - 1),
                            Grad(bb, xf - 1, yf - 1), u);

            return Lerp(x1, x2, v);
        }
    }
}
