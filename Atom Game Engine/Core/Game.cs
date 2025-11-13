using System.Drawing;
using Atom_Game_Engine.Core;
using Atom_Game_Engine.Graphics;
using Atom_Game_Engine.Graphics.Figures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Atom_Game_Engine
{
    public class Game : GameWindow
    {
        //private Square _square;
        private Cube _cube;

        private Shader _shader;

        private Matrix4 _modelMatrix;
        private float _rotation;
        private Vector3 _position;
        private Vector3 _scale;

        private Camera _camera;
        private Vector2 _lastMousePos;
        private bool _firstMove = true;



        [Obsolete]
        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default,
                   new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _camera = new Camera(new Vector3(0f, 0f, 3f));
            CursorState = CursorState.Grabbed;

            // Set clear color
            GL.ClearColor(0.431f, 0.698f, 1.0f, 1.0f);

            // Create shader
            _shader = new Shader("./Graphics/Shaders/shader.vert", "./Graphics/Shaders/shader.frag");
            //_square = new Square();
            _cube = new Cube();

            _position = new Vector3(0.0f, 0.0f, 0.0f);
            _scale = new Vector3(1.0f, 1.0f, 1.0f);
            _rotation = 0.0f;
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Aktiviraj shader
            _shader.Use();

            // 🔹 Model (rotacija, skaliranje, translacija)
            Matrix4 model =
                Matrix4.CreateScale(_scale) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation)) *
                Matrix4.CreateTranslation(_position);

            // 🔹 View i Projection iz kamere
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix(Size.X / (float)Size.Y);

            // 🔹 Pošalji u shader
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            // 🔹 Renderuj kocku
            _cube.Render(_shader);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if(!IsFocused)
                return;

            var input = KeyboardState;
            _camera.ProcessKeyboard(input, (float)args.Time);

            // 🔹 Rotacija
            _rotation += 25.0f * (float)args.Time; // stepeni u sekundi

            // 🔹 Kreiranje model matrice
            _modelMatrix =
                Matrix4.CreateScale(_scale) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation)) *
                Matrix4.CreateTranslation(_position);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (_firstMove)
            {
                _lastMousePos = new Vector2(e.X, e.Y);
                _firstMove = false;
            }
            else
            {
                float deltaX = e.X - _lastMousePos.X;
                float deltaY = e.Y - _lastMousePos.Y;
                _lastMousePos = new Vector2(e.X, e.Y);

                _camera.ProcessMouseMovement(deltaX, deltaY);
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.ProcessMouseScroll(e.OffsetY);
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
            _cube.Dispose();
            _shader.Dispose();

            base.OnUnload();

        }
    }
}

