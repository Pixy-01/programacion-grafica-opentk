using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace U3DExample
{
    public class Game : GameWindow
    {
        //float rotationX = 0f;
        //float rotationY = 0f;

        private Vector3 _position = Vector3.Zero;
        private const float MoveSpeed = 0.01f;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            // Fondo negro
            GL.ClearColor(0f, 0f, 0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Permitir salir con Escape
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Close();

            /* Rotación con las flechas
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
                rotationX -= 1f;
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
                rotationX += 1f;
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
                rotationY -= 1f;
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
                rotationY += 1f;*/
                if (KeyboardState.IsKeyDown(Keys.Left))
                _position.X -= MoveSpeed;
            if (KeyboardState.IsKeyDown(Keys.Right))
                _position.X += MoveSpeed;
            if (KeyboardState.IsKeyDown(Keys.Up))
                _position.Y += MoveSpeed;
            if (KeyboardState.IsKeyDown(Keys.Down))
                _position.Y -= MoveSpeed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
{
    base.OnRenderFrame(args);

    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    // Configuración de proyección
    Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
        MathHelper.DegreesToRadians(45f),
        Size.X / (float)Size.Y,
        0.1f,
        100f);
    GL.MatrixMode(MatrixMode.Projection);
    GL.LoadMatrix(ref projection);

    // Configuración de vista (cámara)
    Matrix4 modelview = Matrix4.LookAt(new Vector3(3, 2, 10), Vector3.Zero, Vector3.UnitY);
    GL.MatrixMode(MatrixMode.Modelview);
    GL.LoadMatrix(ref modelview);

    // Dibujar ejes estáticos (sin transformaciones)
    DrawAxes();

    // Dibujar la U con transformaciones
    GL.PushMatrix();
    GL.Translate(_position);
    DrawU();
    GL.PopMatrix(); // <- Pop debe estar ANTES del SwapBuffers

    SwapBuffers(); // Solo UN SwapBuffers al final
}

        void DrawU()
        {
            // Color azul para la U
            GL.Color3(0.0f, 0.0f, 1.0f);
            // Espesor de la extrusión
            float depth = 0.2f;
            float frontZ = depth / 2;
            float backZ = -depth / 2;

            // --- Cara frontal ---
            GL.Begin(PrimitiveType.Quads);
            // Barra vertical izquierda
            GL.Vertex3(-1f, 1f, frontZ);
            GL.Vertex3(-1f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, 1f, frontZ);
            // Barra vertical derecha
            GL.Vertex3(0.5f, 1f, frontZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.Vertex3(1f, -1f, frontZ);
            GL.Vertex3(1f, 1f, frontZ);
            // Barra inferior
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1.5f, frontZ);
            GL.Vertex3(0.5f, -1.5f, frontZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.End();

            //Cara trasera (vértices en orden inverso para la orientación correcta) 
            GL.Begin(PrimitiveType.Quads);
            // Barra vertical izquierda
            GL.Vertex3(-1f, 1f, backZ);
            GL.Vertex3(-0.5f, 1f, backZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-1f, -1f, backZ);
            // Barra vertical derecha
            GL.Vertex3(0.5f, 1f, backZ);
            GL.Vertex3(1f, 1f, backZ);
            GL.Vertex3(1f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            // Barra inferior
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1.5f, backZ);
            GL.Vertex3(-0.5f, -1.5f, backZ);
            GL.End();

            // Conexión lateral entre la parte frontal y trasera
            // Barra vertical izquierda
            GL.Begin(PrimitiveType.Quads);
            // Lado izquierdo
            GL.Vertex3(-1f, 1f, frontZ);
            GL.Vertex3(-1f, 1f, backZ);
            GL.Vertex3(-1f, -1f, backZ);
            GL.Vertex3(-1f, -1f, frontZ);
            // Lado derecho
            GL.Vertex3(-0.5f, 1f, frontZ);
            GL.Vertex3(-0.5f, 1f, backZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-0.5f, -1f, frontZ);
            // Superior
            GL.Vertex3(-1f, 1f, frontZ);
            GL.Vertex3(-1f, 1f, backZ);
            GL.Vertex3(-0.5f, 1f, backZ);
            GL.Vertex3(-0.5f, 1f, frontZ);
            // Inferior
            GL.Vertex3(-1f, -1f, frontZ);
            GL.Vertex3(-1f, -1f, backZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.End();

            // Barra vertical derecha
            GL.Begin(PrimitiveType.Quads);
            // Lado izquierdo
            GL.Vertex3(0.5f, 1f, frontZ);
            GL.Vertex3(0.5f, 1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            // Lado derecho
            GL.Vertex3(1f, 1f, frontZ);
            GL.Vertex3(1f, 1f, backZ);
            GL.Vertex3(1f, -1f, backZ);
            GL.Vertex3(1f, -1f, frontZ);
            // Superior
            GL.Vertex3(0.5f, 1f, frontZ);
            GL.Vertex3(0.5f, 1f, backZ);
            GL.Vertex3(1f, 1f, backZ);
            GL.Vertex3(1f, 1f, frontZ);
            // Inferior
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(1f, -1f, backZ);
            GL.Vertex3(1f, -1f, frontZ);
            GL.End();

            // Barra inferior
            GL.Begin(PrimitiveType.Quads);
            // Lado izquierdo
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-0.5f, -1.5f, backZ);
            GL.Vertex3(-0.5f, -1.5f, frontZ);
            // Lado derecho
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1.5f, backZ);
            GL.Vertex3(0.5f, -1.5f, frontZ);
            // Superior
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            // Inferior
            GL.Vertex3(-0.5f, -1.5f, frontZ);
            GL.Vertex3(-0.5f, -1.5f, backZ);
            GL.Vertex3(0.5f, -1.5f, backZ);
            GL.Vertex3(0.5f, -1.5f, frontZ);
            GL.End();
        }
        void DrawAxes()
        {
            float axisLength = 5.0f;

            // Eje X (Rojo)
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(-axisLength, 0.0f, 0.0f);
            GL.Vertex3(axisLength, 0.0f, 0.0f);
            GL.End();

            // Eje Y (Amarillo)
            GL.Color3(1.0f, 1.0f, 0.0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0f, -axisLength, 0.0f);
            GL.Vertex3(0.0f, axisLength, 0.0f);
            GL.End();

            // Eje Z (Verde)
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0f, 0.0f, -axisLength);
            GL.Vertex3(0.0f, 0.0f, axisLength);
            GL.End();
        }

        public static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Letra U 3D - OpenTK .NET 6",
                Profile = ContextProfile.Compatability
            };
            using (var window = new Game(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}