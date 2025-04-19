using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Game : GameWindow
    {
        private readonly Control _control = new Control(); //control
        private Escenario _escenario = new Escenario();

        // Figura actualmente seleccionada para aplicar transformaciones
        private string _figuraSeleccionada = "letraU";

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0f, 0f, 0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Cargar figuras como objetos
            CargarFiguraComoObjeto("letraU", "Contenido/letraU.json", Vector3.Zero);
            CargarFiguraComoObjeto("cubo", "Contenido/cubo.json", new Vector3(0, 0, -5));
        }

        private void CargarFiguraComoObjeto(string nombre, string rutaJson, Vector3 posicion)
        {
            var obj = _escenario.CrearObjeto(nombre);
            obj.AgregarParte("main", rutaJson, posicion);
            obj.PosicionGlobal = posicion; // Opcional, según necesidad
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();

            _control.Update(KeyboardState, (float)args.Time);

            // Cambiar entre figuras con las teclas numéricas
            if (KeyboardState.IsKeyDown(Keys.D1)) _figuraSeleccionada = "letraU";
            if (KeyboardState.IsKeyDown(Keys.D2)) _figuraSeleccionada = "cubo";

            // si hubiera mas figuras podrías agregar más teclas
            // if (KeyboardState.IsKeyDown(Keys.D3)) _figuraSeleccionada = "esfera";

            var objetoActual = _escenario.ObtenerObjeto(_figuraSeleccionada);
    if (objetoActual != null)
    {
        objetoActual.PosicionGlobal = _control.Posicion;
        objetoActual.RotacionGlobal = _control.Rotacion;
        objetoActual.EscalaGlobal = _control.Escala;
    }

    _escenario.RotacionGlobal = _control.RotacionGlobal;
    _escenario.PosicionGlobal = _control.PosicionGlobal;

            
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupCamera();

            // Dibujar escenario completo
            _escenario.Dibujar();

            // Mostrar información de la figura seleccionada (opcional)
            MostrarInformacionFiguraSeleccionada();

            SwapBuffers();
        }

        private void MostrarInformacionFiguraSeleccionada()
        {
            // Aquí podrías agregar texto en pantalla mostrando qué figura está seleccionada
            // Si implementas una librería de UI como ImGui para OpenTK
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
                new Vector3(3, 2, 10), //punto de vista x y z
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