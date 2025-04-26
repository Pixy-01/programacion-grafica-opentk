using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Escenario
    {
        public readonly Dictionary<string, Objeto> _objetos
            = new Dictionary<string, Objeto>();


        public void Dibujar()
        {
            foreach (var obj in _objetos.Values)
                obj.Dibujar(); 

            DibujarEjesGlobales();
        }


        public void AñadirObjeto(string id, Objeto obj)
        {
            _objetos[id] = obj;
        }
        public Objeto GetObjeto(string id)//para guardar
        {
            if (_objetos.ContainsKey(id))
            {
                return _objetos[id];
            }
            else
            {
                throw new KeyNotFoundException($"El objeto '{id}' no existe en el escenario.");
            }
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
        /*public Objeto CrearFigura(Color4 color)
        {
           //insertar figura
        }*/
    }
}
