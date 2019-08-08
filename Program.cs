
using System;
using System.Collections.Generic;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RT
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Game().Run(60);
        }

        public class Game : GameWindow
        {

            private int texId;
            
            private int vao;
            private int vbo;
            private int ebo;
            
            private Shader shader;
            private float[] vertices;
            private int[] ebos;
            private Texture tex;

            public Game() : base(1280, 720, GraphicsMode.Default, "RayTracing", GameWindowFlags.Default,
                DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
            {

                KeyDown += (sender, args) => Close();

                texId = GL.GenTexture();

                vao = GL.GenVertexArray();
                vbo = GL.GenBuffer();
                ebo = GL.GenBuffer();
                
                GL.BindVertexArray(vao);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
                
                //constructing a quad
                //texCoord inverted because the rendering was
                 vertices = new[]
                {
                    1.0f,  1.0f, 0.0f,  0.0f, 0.0f,   // top right
                    1.0f, -1.0f, 0.0f,  0.0f, 1.0f,   // bottom right
                    -1.0f, -1.0f, 0.0f, 1.0f, 1.0f,   // bottom left
                    -1.0f,  1.0f, 0.0f, 1.0f, 0.0f    // top left 
                };

                 ebos = new[]
                 {
                     0, 1, 3, // first triangle
                     1, 2, 3 // second triangle
                 };
                
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
                GL.BufferData(BufferTarget.ElementArrayBuffer, ebos.Length * sizeof(int), ebos, BufferUsageHint.StaticDraw);
                
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
                
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);
                
                shader = new Shader("rt.glsl");
                shader.Use();

                GL.ActiveTexture(TextureUnit.Texture0);

                shader.SetInt("tex", 0);
                

                int nx = 1280;
                int ny = 720;
                
                List<Vector4> vector4s = new List<Vector4>(nx * ny);

                
                
                
                Vector3 origin = Vector3.Zero;
                Vector3 vertical = new Vector3(0.0f, 2.0f, 0.0f);
                Vector3 horizontal = new Vector3(4.0f, 0.0f, 0.0f);
                Vector3 lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
                
                World world = new World();
                world.Add(new Sphere(new Vector3(0, 0, -1), .5f));
                world.Add(new Sphere(new Vector3( 0, -100.5f, -1), 100));
                
                for (int j = ny-1; j >= 0; j--)
                {
                    for (int i = 0; i < nx; i++)
                    {
                        float x = (float)i / (float)nx;
                        float y = (float)j / (float) ny;

                        Ray r = new Ray(origin, lowerLeftCorner + x * horizontal + y * vertical);
                        Vector3 color = GetColor(r, world);
                        
                        Vector4 v = Vector4.One;
                        v.X =  (color.X);
                        v.Y =  ( color.Y);;
                        v.Z =  ( color.Z);;
                        vector4s.Add(v);
                    }
                }

                texId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.Linear);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, nx, ny, 0, PixelFormat.Rgba,
                    PixelType.Float,vector4s.ToArray());
            }
            

            Vector3 GetColor(Ray ray, World world)
            {
                HitInfo info = new HitInfo();

                if (world.hit(ray, 0.0f, Single.MaxValue, ref info))
                {
                    return .5f * (Vector3.One + info.normal);
                }
                else
                {
                    Vector3 unitDir = ray.b.Normalized();

                    float blending = 0.5f * unitDir.Y + 1.0f; //param of the lerp
                    return (1.0f - blending) * Vector3.One + blending * new Vector3(0.5f, 0.7f, 1.0f);
                }
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                base.OnRenderFrame(e);

                GL.ClearColor(0, 0, 0, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit);
                
                GL.DrawElements(PrimitiveType.Triangles, ebos.Length, DrawElementsType.UnsignedInt, 0);

                SwapBuffers();
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                GL.Viewport(0, 0, Width, Height);
            }

            protected override void Dispose(bool manual)
            {
                base.Dispose(manual);

                GL.DeleteBuffer(vao);
                GL.DeleteBuffer(vbo);
                
                GL.DeleteTexture(texId);
            }
        }
    }
}