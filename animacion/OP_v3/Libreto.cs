using System.Collections.Generic;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OP // ¡Asegúrate que está en el mismo namespace!
{
    public class Libreto
    {
        public float Duracion { get; set; } = 20f;
        public List<AccionAnimacion> Acciones { get; set; } = new();
        
        public float LargoPista { get; set; } = 20f;
        public float AltoPista { get; set; } = 8f;
        public float AnchoPista { get; set; } = 2f;
        public float Velocidad { get; set; } = 2f;
        public float RadioRuedas { get; set; } = 0.25f;

        public static Libreto CrearLibretoPorDefecto() => new Libreto();
    }

    public class AccionAnimacion
    {
        public string ParteId { get; set; }
        public List<Transformacion> Transformaciones { get; set; } = new();
    }

    public class Transformacion
    {
        public Vector3 PosicionInicial { get; set; }
        public Vector3 PosicionFinal { get; set; }
        public Vector3 RotacionInicial { get; set; }
        public Vector3 RotacionFinal { get; set; }
        public Vector3 EscalaInicial { get; set; } = Vector3.One;
        public Vector3 EscalaFinal { get; set; } = Vector3.One;
    }
}