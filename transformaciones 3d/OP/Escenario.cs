using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OP
{
    public class Escenario
    {
        private readonly Dictionary<string, Objeto> _objetos = new Dictionary<string, Objeto>();
        
        public Vector3 RotacionGlobal { get; set; } = Vector3.Zero;
        public Vector3 PosicionGlobal { get; set; } = Vector3.Zero;

        public Objeto CrearObjeto(string nombreObjeto)
        {
            var objeto = new Objeto(nombreObjeto);
            _objetos.Add(nombreObjeto, objeto);
            return objeto;
        }

        public Objeto ObtenerObjeto(string nombreObjeto)
        {
            return _objetos.TryGetValue(nombreObjeto, out var obj) ? obj : null;
        }

        public bool EliminarObjeto(string nombreObjeto)
        {
            return _objetos.Remove(nombreObjeto);
        }

        public List<string> ObtenerNombresObjetos()
        {
            return new List<string>(_objetos.Keys);
        }

        public void Dibujar()
        {
            GL.PushMatrix(); 
            GL.Translate(PosicionGlobal);
            GL.Rotate(RotacionGlobal.X, Vector3.UnitX);
            GL.Rotate(RotacionGlobal.Y, Vector3.UnitY);
            GL.Rotate(RotacionGlobal.Z, Vector3.UnitZ);

            DibujarEjesGlobales();

            foreach (var objeto in _objetos.Values)
                objeto.Dibujar();
            
            GL.PopMatrix(); 
        }

        private void DibujarEjesGlobales()
        {
            float axisLength = 5.0f;

            GL.LineWidth(2.0f); // Grosor de línea para mayor visibilidad

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

            GL.LineWidth(1.0f); // Restaurar grosor por defecto
        }
    }
}