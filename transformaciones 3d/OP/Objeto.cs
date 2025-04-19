using OpenTK.Mathematics;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    // Esta clase representa un objeto 3D compuesto por múltiples partes
    public class Objeto
    {
        // Nombre del objeto para identificarlo
        public string Nombre { get; set; }

        // Diccionario de partes que componen el objeto
        private Dictionary<string, Parte> _partes = new Dictionary<string, Parte>();

        // Transformaciones globales del objeto completo
        public Vector3 PosicionGlobal { get; set; } = Vector3.Zero;
        public Vector3 RotacionGlobal { get; set; } = Vector3.Zero;
        public float EscalaGlobal { get; set; } = 1.0f;

        // Constructor
        public Objeto(string nombre)
        {
            Nombre = nombre;
        }

        // Clase interna para representar cada parte del objeto
        public class Parte
        {
            public string Nombre { get; set; }
            public Dibujar Dibujador { get; set; }
            public Vector3 PosicionLocal { get; set; } = Vector3.Zero;
            public Vector3 RotacionLocal { get; set; } = Vector3.Zero;
            public float EscalaLocal { get; set; } = 1.0f;

            // Centro de pivot para rotaciones locales
            public Vector3 CentroPivot { get; set; } = Vector3.Zero;
        }

        // Agregar una parte al objeto
        public void AgregarParte(string nombreParte, string rutaJson, Vector3 posicionLocal = default)
        {
            var dibujador = new Dibujar();
            dibujador.CargarFigura(rutaJson);

            _partes.Add(nombreParte, new Parte
            {
                Nombre = nombreParte,
                Dibujador = dibujador,
                PosicionLocal = posicionLocal
            });
        }

        // Obtener una parte específica por nombre
        public Parte ObtenerParte(string nombreParte)
        {
            if (_partes.ContainsKey(nombreParte))
                return _partes[nombreParte];
            return null;
        }

        // Comprobar si existe una parte
        public bool ExisteParte(string nombreParte)
        {
            return _partes.ContainsKey(nombreParte);
        }

        // Obtener lista de nombres de partes
        public List<string> ObtenerNombresPartes()
        {
            return new List<string>(_partes.Keys);
        }

        // Dibujar el objeto completo aplicando todas las transformaciones
        public void Dibujar()
        {
            GL.PushMatrix(); // Aislar transformaciones del objeto

            // Aplicar transformaciones globales del objeto
            GL.Translate(PosicionGlobal);
            GL.Rotate(RotacionGlobal.X, Vector3.UnitX);
            GL.Rotate(RotacionGlobal.Y, Vector3.UnitY);
            GL.Rotate(RotacionGlobal.Z, Vector3.UnitZ);
            GL.Scale(EscalaGlobal, EscalaGlobal, EscalaGlobal);

            // Dibujar cada parte con sus transformaciones locales
            foreach (var parte in _partes.Values)
            {
                GL.PushMatrix(); // Aislar transformaciones de la parte

                // Aplicar transformaciones locales de la parte
                GL.Translate(parte.PosicionLocal);

                // Para rotar alrededor del centro de pivot:
                GL.Translate(parte.CentroPivot);
                GL.Rotate(parte.RotacionLocal.X, Vector3.UnitX);
                GL.Rotate(parte.RotacionLocal.Y, Vector3.UnitY);
                GL.Rotate(parte.RotacionLocal.Z, Vector3.UnitZ);
                GL.Translate(-parte.CentroPivot);

                GL.Scale(parte.EscalaLocal, parte.EscalaLocal, parte.EscalaLocal);

                // Dibujar la parte
                parte.Dibujador.DibujarFigura(Vector3.Zero, 1.0f, Vector3.Zero);

                GL.PopMatrix(); // Restaurar matriz de la parte
            }

            GL.PopMatrix(); // Restaurar matriz del objeto
        }

        // Actualizar transformaciones de una parte específica
        public void ActualizarTransformacionesParte(string nombreParte, Vector3 posicion, Vector3 rotacion, float escala)
        {
            if (_partes.ContainsKey(nombreParte))
            {
                _partes[nombreParte].PosicionLocal = posicion;
                _partes[nombreParte].RotacionLocal = rotacion;
                _partes[nombreParte].EscalaLocal = escala;
            }
        }

        // Establecer centro de pivot para una parte
        public void EstablecerCentroPivot(string nombreParte, Vector3 centroPivot)
        {
            if (_partes.ContainsKey(nombreParte))
            {
                _partes[nombreParte].CentroPivot = centroPivot;
            }
        }
    }
}