using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace U3DExample
{
    public class Game : GameWindow
    {
        float rotationX = 0f;
        float rotationY = 0f;

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

            // Rotación con las flechas
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
                rotationX -= 1f;
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
                rotationX += 1f;
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
                rotationY -= 1f;
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
                rotationY += 1f;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Limpiar buffers de color y profundidad
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configurar la proyección
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                Size.X / (float)Size.Y,
                0.1f,
                100f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            // Configurar la vista (cámara)
            Matrix4 modelview = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            // Aplicar rotaciones
            GL.Rotate(rotationX, 1.0f, 0.0f, 0.0f);
            GL.Rotate(rotationY, 0.0f, 1.0f, 0.0f);

            // Dibujar la letra U en 3D
            DrawU();

            SwapBuffers();
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

            // --- Cara trasera (vértices en orden inverso para la orientación correcta) ---
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

            // --- Conexión lateral entre la parte frontal y trasera ---
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
