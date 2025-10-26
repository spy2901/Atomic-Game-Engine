using System.Drawing;
using Atom_Game_Engine.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Atom_Game_Engine
{
    public class Game : GameWindow
    {
        private Square _square;

        private Shader _shader;

        [Obsolete]
        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default,
                   new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Set clear color
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            
            // Create shader
            _shader = new Shader("./Graphics/Shaders/shader.vert", "./Graphics/Shaders/shader.frag");
            _square = new Square();

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _square.Render(_shader);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Get the state of the keyboard this frame
            // 'KeyboardState' is a property of GameWindow
            // TODO: Obrisati Drugi uslov za gasenje u trenutku kad krene izrada kontorli za karaktera
            if (KeyboardState.IsKeyDown(Keys.Escape) || KeyboardState.IsKeyDown(Keys.Space))
            {
                Close();
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            int size = Math.Min(e.Width, e.Height);
            int x = (e.Width - size) / 2;
            int y = (e.Height - size) / 2;

            GL.Viewport(x, y, size, size);
        }

        protected override void OnUnload()
        {
            _square.Dispose();
            _shader.Dispose();

            base.OnUnload();

        }
    }
}

