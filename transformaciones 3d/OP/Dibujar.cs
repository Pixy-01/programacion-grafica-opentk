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

        private Vector3 _centroMasa;

        // Carga la figura desde JSON
        public void CargarFigura(string rutaArchivo)
        {
            string json = File.ReadAllText(rutaArchivo);
            var options = new JsonSerializerOptions
            {
                Converters = { new Vector3Converter() }
            };
            _figuraCargada = JsonSerializer.Deserialize<FiguraJson>(json, options);


            //calcula el centro de masa de la figura
            Vector3 suma = Vector3.Zero;
            foreach (var v in _figuraCargada.Vertices)
            {
                suma += new Vector3(v[0], v[1], v[2]);
            }
            _centroMasa = suma / _figuraCargada.Vertices.Count;
        }



        // Dibuja la figura
        public void DibujarFigura(Vector3 posicion, float escala, Vector3 rotacion)
        {
            if (_figuraCargada == null) return;

            GL.PushMatrix(); // Aislar transformaciones locales
            GL.Translate(posicion);
            GL.Translate(_centroMasa);
            GL.Scale(escala, escala, escala);
            GL.Rotate(rotacion.X, Vector3.UnitX);
            GL.Rotate(rotacion.Y, Vector3.UnitY);
            GL.Rotate(rotacion.Z, Vector3.UnitZ);
            GL.Translate(-_centroMasa);
            

            // Dibujar la figura
            GL.Color4(new Color4(
                _figuraCargada.Color[0],
                _figuraCargada.Color[1],
                _figuraCargada.Color[2],
                _figuraCargada.Color[3]
            ));

            GL.Begin(_figuraCargada.TipoPrimitiva);
            foreach (var v in _figuraCargada.Vertices)
            {
                GL.Vertex3(new Vector3(v[0], v[1], v[2]));
            }
            GL.End();

            GL.PopMatrix();
        }
    }
}