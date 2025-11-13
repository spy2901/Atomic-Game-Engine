using System.Drawing;
using Atom_Game_Engine.Core;
using Atom_Game_Engine.Graphics;
using Atom_Game_Engine.Graphics.Figures;
using Atom_Game_Engine.Graphics.Voxel;
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
        //private Cube _cube;

        private Chunk _chunk;
        private ChunkMesh _chunkMesh;

        private Shader _shader;

        private Matrix4 _modelMatrix;
        private float _rotation;
        private Vector3 _position;
        private Vector3 _scale;

        private Camera _camera;
        private Vector2 _lastMousePos;
        private bool _firstMove = true;

        Texture _atlas;



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
            _atlas = new Texture("./Assets/atlas.png");
            //CursorState = CursorState.Grabbed;

            // Set clear color
            GL.ClearColor(0.431f, 0.698f, 1.0f, 1.0f);


            // Create shader
            _shader = new Shader("./Graphics/Shaders/shader.vert", "./Graphics/Shaders/shader.frag");
            _shader.SetInt("textureAtlas", 0);
            //_square = new Square();
            _chunk = new Chunk(new Vector3i(0, 0, 0));
            _chunkMesh = new ChunkMesh(_shader, _atlas, _chunk);

            var chunk = new Chunk(new Vector3i(0, 0, 0));
            Console.WriteLine("chunk generated: " + chunk.GetAll().Count() + " voxels");

            _position = new Vector3(0.0f, 0.0f, 0.0f);
            _scale = new Vector3(1.0f, 1.0f, 1.0f);
            _rotation = 0.0f;
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();

            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(_camera.Fov),
                Size.X / (float)Size.Y,
                0.1f,
                100.0f));
            Matrix4 model = Matrix4.Identity;
            _shader.SetMatrix4("model", model);

            _chunkMesh.Render();

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
            _shader.Dispose();
            base.OnUnload();

        }
    }
}

