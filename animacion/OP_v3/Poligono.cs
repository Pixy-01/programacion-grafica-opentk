using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Linq;
using System.Text.Json.Serialization;

namespace OP
{
    public class Poligono
    {
        public Dictionary<string, Punto> Vertices { get; set; }
        public float[] ColorArray
        {
            get => new[] { Color.R, Color.G, Color.B, Color.A };
            set => Color = new Color4(value[0], value[1], value[2], value[3]);
        }
        [JsonIgnore]
        public Color4 Color { get; set; }


        // Constructor sin parámetros
        public Poligono()
        {
            Vertices = new Dictionary<string, Punto>();
            Color = Color4.Blue;
        }

        // Constructor con color
        public Poligono(Color4 color) : this()
        {
            Color = color;
        }

        public void AñadirVertice(string id, Punto punto)
        {
            Vertices[id] = punto;
        }

        public Punto CalcularCentroDeMasa()
        {
            if (Vertices.Count == 0)
                return new Punto();

            double xProm = Vertices.Values.Average(v => v.X);
            double yProm = Vertices.Values.Average(v => v.Y);
            double zProm = Vertices.Values.Average(v => v.Z);

            return new Punto(xProm, yProm, zProm);
        }



        public void Dibujar()
        {
            if (Vertices.Count < 3)
            {
                Console.WriteLine("El polígono no tiene suficientes vértices para ser dibujado.");
                return;
            }

            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(Color);
            foreach (var vertice in Vertices.Values)
            {
                //Console.WriteLine($"Dibujando vértice: X={vertice.X}, Y={vertice.Y}, Z={vertice.Z}");
                vertice.Dibujar();
            }
            GL.End();
        }
    }
}