using System.Collections.Generic;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Parte
    {
        public Dictionary<string, Poligono> Poligonos { get; set; }
        public float[] ColorArray
        {
            get => new[] { Color.R, Color.G, Color.B, Color.A };
            set => Color = new Color4(value[0], value[1], value[2], value[3]);
        }
        [JsonIgnore]
        public Color4 Color { get; set; }
        [JsonIgnore]
        public Vector3 Posicion { get; set; }

        [JsonIgnore]
        public Vector3 Rotacion { get; set; } = Vector3.Zero;

        [JsonIgnore]
        public Vector3 Escala { get; set; } = Vector3.One;

        //  Constructor sin parámetros (OBLIGATORIO para deserialización)
        public Parte()
        {
            Poligonos = new Dictionary<string, Poligono>();
            Color = Color4.Blue; // Valor por defecto
        }

        //  Constructor con color (para creación manual)
        public Parte(Color4 color) : this() // Reutiliza el constructor sin parámetros
        {
            Color = color;
        }

        public void AñadirPoligono(string id, Poligono poligono)
        {
            Poligonos[id] = poligono;
        }
        public void Dibujar()
        {
            GL.PushMatrix();
            GL.Translate(Posicion);
            GL.Rotate(Rotacion.X, Vector3.UnitX);
            GL.Rotate(Rotacion.Y, Vector3.UnitY);
            GL.Rotate(Rotacion.Z, Vector3.UnitZ);
            GL.Scale(Escala);

            foreach (var poligono in Poligonos.Values)
                poligono.Dibujar();

            GL.PopMatrix();
        }
    }
}