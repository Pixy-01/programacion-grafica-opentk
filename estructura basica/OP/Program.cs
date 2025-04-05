using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Game : GameWindow
    {
        private Vector3 _position = Vector3.Zero;
        private const float MoveSpeed = 0.01f;
        private readonly Dibujar _dibujador = new Dibujar();//objeto

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0f, 0f, 0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
            
            // Manejo de entrada
            if (KeyboardState.IsKeyDown(Keys.Left)) _position.X -= MoveSpeed;
            if (KeyboardState.IsKeyDown(Keys.Right)) _position.X += MoveSpeed;
            if (KeyboardState.IsKeyDown(Keys.Up)) _position.Y += MoveSpeed;
            if (KeyboardState.IsKeyDown(Keys.Down)) _position.Y -= MoveSpeed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupCamera();
            
            // Dibujar ejes y U
            _dibujador.DrawAxes();
            _dibujador.DrawU();
            
            GL.PushMatrix();
            GL.Translate(_position);
            _dibujador.DrawU();
            GL.PopMatrix();

            SwapBuffers();
        }

         private void SetupCamera()
        {
            // Proyección
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f), 
                Size.X / (float)Size.Y, 
                0.1f, 
                100f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            // Vista
            Matrix4 modelview = Matrix4.LookAt(
                new Vector3(3, 2, 10), 
                Vector3.Zero, 
                Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        public static void Main()
        {
            var nativeSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "U 3D",
                Profile = ContextProfile.Compatability
            };

            using var game = new Game(GameWindowSettings.Default, nativeSettings);
            game.Run();
        }
    }
}