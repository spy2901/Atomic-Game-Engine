using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Atom_Game_Engine.Graphics
{
	public class Texture
	{
        public int Handle { get; private set; }

        public Texture(string path)
		{
            // 1. Generiši i aktiviraj teksturu
            Handle = GL.GenTexture();
            Use();

            // 2. Flipuj sliku (jer OpenGL učitava odozdo, a slike često odozgo)
            StbImage.stbi_set_flip_vertically_on_load(1);

            // 3. Učitaj sliku sa diska
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            // 4. Učitaj podatke u OpenGL teksturu
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            // 5. Podesi parametre filtriranja i omotavanja (wrap + filter)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // 6. Generiši mipmap
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }
    }
}

