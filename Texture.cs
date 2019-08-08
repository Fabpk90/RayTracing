using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace RT
{
    public class Texture : IDisposable
    {
        public int id;
        public string type;
        public string path;

        public Texture(string path, string type)
        {
            
            this.path = path;
            this.type = type;
        
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            var stream = File.OpenRead(path);

            IImageFormat config;
            var img = Image.Load(stream, out config);
            
            
            stream.Close();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                 PixelInternalFormat.Rgba
                , img.Width, img.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, img.GetPixelSpan().ToArray());

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Dispose()
        {
            GL.DeleteTexture(id);
            Console.WriteLine("Unloading " + path + type);
        }
    }
}