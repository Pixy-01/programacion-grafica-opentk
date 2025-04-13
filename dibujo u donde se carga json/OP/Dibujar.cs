using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization; 

namespace OP
{
    public class Dibujar
    {
        private FiguraJson _figuraCargada;

        // Carga la figura desde JSON
        public void CargarFigura(string rutaArchivo)
        {
            string json = File.ReadAllText(rutaArchivo);
            var options = new JsonSerializerOptions
            {
                Converters = { new Vector3Converter() }
            };
            _figuraCargada = JsonSerializer.Deserialize<FiguraJson>(json, options);
        }

        // Dibuja la figura en una posición específica
        public void DibujarFigura(Vector3 posicion)
        {
            if (_figuraCargada == null) return;

            GL.PushMatrix();
            GL.Translate(posicion);
            
            GL.Color4(new Color4(
                _figuraCargada.Color[0],
                _figuraCargada.Color[1],
                _figuraCargada.Color[2],
                _figuraCargada.Color[3]
            ));

            GL.Begin(_figuraCargada.TipoPrimitiva);
            foreach (var vertice in _figuraCargada.Vertices)
            {
                GL.Vertex3(new Vector3(vertice[0], vertice[1], vertice[2]));
            }
            GL.End();
            
            GL.PopMatrix();
        }

        // dibuja los ejes de coordenadas 
        public void DrawAxes()
        {
            float axisLength = 5.0f;//longitud

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
    }
}