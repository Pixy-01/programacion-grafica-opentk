using System.Collections.Generic;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;


namespace OP
{
    public class Game : GameWindow
    {
        private Dictionary<string, Control> _controlesPorFigura = new Dictionary<string, Control>();
        private Escenario escenario;
        private Control control = new Control();
        private List<string> _figurasSeleccionadas = new List<string> { "coche" };
        private bool _modoParte = false;
        private string _parteSeleccionada = "Rueda1";
        private string _figuraSeleccionada = "coche";
        private float _velocidad = 2f;            // unidades por segundo
        private float _anguloAnimacion = 0f;      // posición a lo largo del perímetro
        private float _rotacionRuedas = 0f;
        private Animacion _animacionCoche; // <--- Declaración faltante
        private Dictionary<string, Control> _controlesPorParte = new Dictionary<string, Control>();
        private Dictionary<string, Dictionary<string, Control>> _controlesPorFiguraYParte = new Dictionary<string, Dictionary<string, Control>>();

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0f, 0f, 0f, 1.0f);//color fondo
            GL.Enable(EnableCap.DepthTest);

            escenario = new Escenario();
            //carga desde escenario

            // Cargar el cubo desde un archivo
            string objeto = @"C:\Users\migue\Documents\C#\OP_v3\contenido\coche.json";
            //string U = @"C:\Users\migue\Documents\C#\OP_v3\contenido\letraU.json";


            Objeto coche = Objeto.Deserializar(objeto);
            coche.Posicion = new Punto(0, 0.5, 3); // Ajusta la posición del cubo cargado
            escenario.AñadirObjeto("coche", coche); // Añade el objeto al escenario

            // ruedas posicion
            //cuboCargado.Posicion = new Punto(0, 0.3f, 3); // Ajustar altura inicial

            // Ajustar orientación inicial de las ruedas (eje X para rotación de rodado)
            coche.Partes["Rueda1"].Rotacion = new Vector3(0, 0, 90);
            coche.Partes["Rueda2"].Rotacion = new Vector3(0, 0, 90);
            coche.Partes["Rueda3"].Rotacion = new Vector3(0, 0, 90);
            coche.Partes["Rueda4"].Rotacion = new Vector3(0, 0, 90);

            // Asegurar que las ruedas están a la altura correcta respecto al chasis
            coche.Partes["Rueda1"].Posicion = new Vector3(-0.8f, -0.3f, 0.6f);
            coche.Partes["Rueda2"].Posicion = new Vector3(0.8f, -0.3f, 0.6f);
            coche.Partes["Rueda3"].Posicion = new Vector3(-0.8f, -0.3f, -0.6f);
            coche.Partes["Rueda4"].Posicion = new Vector3(0.8f, -0.3f, -0.6f);

            //escenario.AñadirObjeto("coche", cuboCargado);

            //_controlesPorFigura["letraU"] = new Control();
            _controlesPorFigura["coche"] = new Control();

            // Inicializar controles para cada parte de la letra U
            _controlesPorFiguraYParte["coche"] = new Dictionary<string, Control>();
            foreach (string parteId in coche.Partes.Keys)
            {
                _controlesPorFiguraYParte["coche"][parteId] = new Control();
            }

            foreach (string parteId in coche.Partes.Keys)
            {
                _controlesPorParte[parteId] = new Control(); // Control individual por parte
                _controlesPorFiguraYParte["coche"][parteId] = new Control(); // Control por figura y parte
            }
            string rutaLibreto = @"C:\Users\migue\Documents\C#\OP_v3\contenido\libreto_coche.json";
            Directory.CreateDirectory(Path.GetDirectoryName(rutaLibreto)!); // Crear carpeta si no existe

            _animacionCoche = new Animacion();
            _animacionCoche.CargarLibreto(rutaLibreto); // La creación del archivo se maneja internamente
            _animacionCoche.Play();

        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            float delta = (float)args.Time;
            // Crear animación en memoria (sin serializar)
            var coche = escenario.GetObjeto("coche");
            _animacionCoche.Actualizar(delta, coche); //AnimarCoche


            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();

            control.Update(KeyboardState, (float)args.Time);


            if (KeyboardState.IsKeyPressed(Keys.Tab))
                _modoParte = !_modoParte;


        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupCamera();

            escenario.Dibujar();

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
                new Vector3(3, 2, 15), //punto de vista x y z
                Vector3.Zero,
                Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);


            // Aplicar transformaciones GLOBALES del escenario
            GL.Translate(control.PosicionGlobal);
            GL.Rotate(control.RotacionGlobal.X, Vector3.UnitX);
            GL.Rotate(control.RotacionGlobal.Y, Vector3.UnitY);
            GL.Rotate(control.RotacionGlobal.Z, Vector3.UnitZ);
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