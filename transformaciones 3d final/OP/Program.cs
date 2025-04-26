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
        private Escenario escenario;//|para guardar el escenario
        private Control control = new Control();//|para guardar el control de la figura seleccionada

        private Dictionary<string, Dictionary<string, Control>> _controlesPorFiguraYParte = new Dictionary<string, Dictionary<string, Control>>();

        private string _figuraSeleccionada = "letraU";
        private string _parteSeleccionada = "FrenteBrazoIzq"; // Nombre de la parte según tu JSON
        private List<string> _figurasSeleccionadas = new List<string> { "letraU" };
        private bool _modoParte = false;
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0f, 0f, 0f, 1.0f);//color fondo
            GL.Enable(EnableCap.DepthTest);

            escenario = new Escenario();
            //carga desde escenario

            /*Objeto Figura = escenario.CrearFigura(Color4.Blue);
            Figura.Posicion = new Punto(0, 0, 0);
            escenario.AñadirObjeto("cubo", Figura); */

            // Cargar el cubo desde un archivo
            string cubo = @"C:\Users\migue\Documents\C#\OP\contenido\cubo.json";
            string U = @"C:\Users\migue\Documents\C#\OP\contenido\letraU.json";


            Objeto letraU = Objeto.Deserializar(U);
            letraU.Posicion = new Punto(0, 0, 0); // Ajusta la posición de la letra U cargada
            escenario.AñadirObjeto("letraU", letraU); // Añade el objeto al escenario


            Objeto cuboCargado = Objeto.Deserializar(cubo);
            cuboCargado.Posicion = new Punto(0, 0, -5); // Ajusta la posición del cubo cargado
            escenario.AñadirObjeto("Cubo", cuboCargado); // Añade el objeto al escenario

            _controlesPorFigura["letraU"] = new Control();//|para guardar el control de la figura seleccionada
            _controlesPorFigura["Cubo"] = new Control();//|para guardar el control de la figura seleccionada

            // Inicializar controles para cada parte de la letra U
            _controlesPorFiguraYParte["letraU"] = new Dictionary<string, Control>();
            foreach (string parteId in letraU.Partes.Keys)
            {
                _controlesPorFiguraYParte["letraU"][parteId] = new Control();
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
            control.Update(KeyboardState, (float)args.Time);//para el escenario

            // Cambiar selección de figuras
            if (KeyboardState.IsKeyDown(Keys.D0))
            {
                _figurasSeleccionadas.Clear();
                _figurasSeleccionadas.AddRange(_controlesPorFigura.Keys);
            }
            else if (KeyboardState.IsKeyDown(Keys.D1))
            {
                _figurasSeleccionadas.Clear();
                _figurasSeleccionadas.Add("letraU");
            }
            else if (KeyboardState.IsKeyDown(Keys.D2))
            {
                _figurasSeleccionadas.Clear();
                _figurasSeleccionadas.Add("Cubo");
            }


            // Alternar modo parte/figura
            if (KeyboardState.IsKeyPressed(Keys.Tab)) _modoParte = !_modoParte;

            if (_modoParte)
            {
                // Cambiar parte seleccionada con teclas (ejemplo: D3, D4)
                if (KeyboardState.IsKeyDown(Keys.D3)) _parteSeleccionada = "renteBrazoIzq";
                if (KeyboardState.IsKeyDown(Keys.D4)) _parteSeleccionada = "FrenteBrazoDer";

                // Obtener control de la parte seleccionada
                Control controlDeLaParte = _controlesPorFiguraYParte[_figuraSeleccionada][_parteSeleccionada];
                controlDeLaParte.Update(KeyboardState, (float)args.Time);

                // Aplicar transformaciones a la parte
                Objeto figura = escenario.GetObjeto(_figuraSeleccionada);
                Parte parte = figura.Partes[_parteSeleccionada];
                parte.Posicion = controlDeLaParte.Posicion;
                parte.Rotacion = controlDeLaParte.Rotacion;
                parte.Escala = new Vector3(controlDeLaParte.Escala); // Conversión correcta
            }
            else
            {
                // Controlar figura completa
                foreach (string figuraId in _figurasSeleccionadas)
                {
                    Control controlFigura = _controlesPorFigura[figuraId];
                    controlFigura.Update(KeyboardState, (float)args.Time);

                    Objeto selectedObj = escenario.GetObjeto(figuraId);
                    selectedObj.Posicion = new Punto(
                        controlFigura.Posicion.X,
                        controlFigura.Posicion.Y,
                        controlFigura.Posicion.Z
                    );
                    selectedObj.Rotacion = controlFigura.Rotacion;
                    selectedObj.Escala = new Vector3(controlFigura.Escala);
                }
            }



            /*if (KeyboardState.IsKeyDown(Keys.S))
            {
                // Guardar el cubo en un archivo JSON
                Objeto Figura = escenario.GetObjeto("cubo"); 
                Figura.Serializar(@"C:\Users\migue\Documents\C#\OP\contenido\cubo.json");
            }*/


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
                new Vector3(3, 2, 10), //punto de vista x y z
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