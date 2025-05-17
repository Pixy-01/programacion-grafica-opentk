using System.Collections.Generic;
using OpenTK.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Objeto
    {
        public Dictionary<string, Parte> Partes { get; set; } = new Dictionary<string, Parte>();

        // Propiedad para serialización/deserialización
        public float[] ColorArray
        {
            get => new[] { Color.R, Color.G, Color.B, Color.A };
            set => Color = new Color4(value[0], value[1], value[2], value[3]);
        }

        // Propiedad ignorada en la serialización
        [JsonIgnore]
        public Color4 Color { get; set; }

        public Punto CentroDeMasa { get; set; }
        [JsonIgnore]
        public Punto Posicion { get; set; }
        [JsonIgnore]
        public Vector3 Rotacion { get; set; } = Vector3.Zero;
        [JsonIgnore]
        public Vector3 Escala { get; set; } = Vector3.One;




        public void Dibujar()
        {
            GL.PushMatrix();

            // Aplica transformaciones locales del objeto
            GL.Translate(Posicion.ToVector3());
            GL.Rotate(Rotacion.X, Vector3.UnitX);
            GL.Rotate(Rotacion.Y, Vector3.UnitY);
            GL.Rotate(Rotacion.Z, Vector3.UnitZ);
            GL.Scale(Escala);

            foreach (var parte in Partes.Values)
                parte.Dibujar();

            GL.PopMatrix();
        }


        public Objeto()
        {
            Partes = new Dictionary<string, Parte>();
            Color = Color4.Blue;
            CentroDeMasa = new Punto(0, 0, 0);
        }

        public Objeto(Color4 color) : this()
        {
            Color = color;
        }

        public void AñadirParte(string id, Parte parte)
        {
            Partes[id] = parte;
        }


        // Método para serializar una figura (Objeto)
        public void Serializar(string nombreArchivo)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,

            };
            string json = JsonSerializer.Serialize(this, opciones);
            File.WriteAllText(nombreArchivo, json);
        }

        public static Objeto Deserializar(string nombreArchivo)
        {
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Ignorar mayúsculas/minúsculas
                IncludeFields = true, // Incluir campos si los hay
                ReadCommentHandling = JsonCommentHandling.Skip // Ignorar comentarios en el JSON
            };

            string json = File.ReadAllText(nombreArchivo);
            return JsonSerializer.Deserialize<Objeto>(json, opciones);
        }
    }
}
