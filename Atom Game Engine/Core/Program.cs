using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AtomGameEngine
{
    class Program
    {
        static void Main()
        {
            using (Game game = new Game(800, 600, "LearnOpenTK"))
            {
                game.Run();
            }
        }
    }

    public class Game : GameWindow
    {
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            //Get the state of the keyboard this frame
            // 'KeyboardState' is a property of GameWindow
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}
