using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Atom_Game_Engine.Core
{
	public class Camera
	{
        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; } = -Vector3.UnitZ;
        public Vector3 Up { get; set; } = Vector3.UnitY;
        public Vector3 Right { get; private set; } = Vector3.UnitX;
        public Vector3 WorldUp { get; private set; } = Vector3.UnitY;

        public float Yaw { get; set; } = -90f;
        public float Pitch { get; set; } = 0f;
        public float Speed { get; set; } = 2.5f;
        public float Sensitivity { get; set; } = 0.1f;
        public float Zoom { get; set; } = 45f;

        private float _fov = 45f;
        public float Fov
        {
            get => _fov;
            set
            {
                if (value < 1.0f)
                    _fov = 1.0f;
                else if (value > 90.0f)
                    _fov = 90.0f;
                else
                    _fov = value;
            }
        }

        public Camera(Vector3 position)
        {
            Position = position;
            UpdateCameraVectors();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public Matrix4 GetProjectionMatrix(float aspectRatio)
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(Zoom),
                aspectRatio,
                0.1f,
                100f
            );
        }

        public void ProcessKeyboard(KeyboardState input, float deltaTime)
        {
            float velocity = Speed * deltaTime;

            if (input.IsKeyDown(Keys.W))
                Position += Front * velocity;
            if (input.IsKeyDown(Keys.S))
                Position -= Front * velocity;
            if (input.IsKeyDown(Keys.A))
                Position -= Right * velocity;
            if (input.IsKeyDown(Keys.D))
                Position += Right * velocity;
            if (input.IsKeyDown(Keys.Space))
                Position += Up * velocity;
            if (input.IsKeyDown(Keys.LeftShift))
                Position -= Up * velocity;
        }

        public void ProcessMouseMovement(float deltaX, float deltaY, bool constrainPitch = true)
        {
            deltaX *= Sensitivity;
            deltaY *= Sensitivity;

            Yaw += deltaX;
            Pitch -= deltaY;

            if (constrainPitch)
            {
                if (Pitch > 89.0f)
                    Pitch = 89.0f;
                if (Pitch < -89.0f)
                    Pitch = -89.0f;
            }

            UpdateCameraVectors();
        }
        

        public void ProcessMouseScroll(float deltaY)
        {
            Zoom -= deltaY;
            if (Zoom < 1.0f)
                Zoom = 1.0f;
            if (Zoom > 45.0f)
                Zoom = 45.0f;
        }

        private void UpdateCameraVectors()
        {
            Vector3 front;
            front.X = MathF.Cos(MathHelper.DegreesToRadians(Pitch)) * MathF.Cos(MathHelper.DegreesToRadians(Yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(Pitch)) * MathF.Sin(MathHelper.DegreesToRadians(Yaw));
            Front = Vector3.Normalize(front);
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
    }
}

