using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.ES11;

namespace AtomGameEngine
{
    class Program
    {
        static void Main()
        {
            var settings = new GameWindowSettings
            {
                RenderFrequency = 60.0,
                UpdateFrequency = 60.0
            };

            var native = new NativeWindowSettings
            {
                Size = new Vector2i(1280, 720),
                Title = "VoxCore Engine"
            };

            using var window = new EngineWindow(settings, native);
            window.Run();
        }
    }

    class EngineWindow : GameWindow
    {
        public EngineWindow(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) { }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Close();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();
        }
    }
}
