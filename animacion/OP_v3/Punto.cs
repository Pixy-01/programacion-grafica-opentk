using OpenTK.Mathematics;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Punto
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        // Constructor sin parámetros para la deserialización
        public Punto() { }

        public Punto(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public void Dibujar()
        {
            GL.Vertex3(X, Y, Z);
        }
        public Vector3 ToVector3()
        {
            return new Vector3((float)X, (float)Y, (float)Z);
        }
    }
}