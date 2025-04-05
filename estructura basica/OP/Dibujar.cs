using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OP
{
    public class Dibujar
    {
        //private readonly Color4 _colorU = new Color4(0.0f, 0.0f, 1.0f, 1.0f);
        //private const float ProfundidadU = 0.2f;
        private readonly Color4 _colorU;
        private readonly float _profundidadU;
        private readonly float _axisLength;

        // Constructor con parámetros personalizables
        public Dibujar(Color4 colorU, float profundidadU, float axisLength = 5.0f)
        {
            _colorU = colorU;
            _profundidadU = profundidadU;
            _axisLength = axisLength;
        }

        // Constructor por defecto (azul, profundidad 0.2)
        public Dibujar() : this(new Color4(0.0f, 0.0f, 1.0f, 1.0f), 0.2f) { }

        public void DrawU()
        {
            // Color azul para la U
            GL.Color3(0.0f, 0.0f, 1.0f);
            // Espesor de la extrusión
            float depth = 0.2f;
            float frontZ = depth / 2;
            float backZ = -depth / 2;

            // --- Cara frontal ---
            GL.Begin(PrimitiveType.Quads);
            // Barra vertical izquierda
            GL.Vertex3(-1f, 1f, frontZ);
            GL.Vertex3(-1f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, 1f, frontZ);
            // Barra vertical derecha
            GL.Vertex3(0.5f, 1f, frontZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.Vertex3(1f, -1f, frontZ);
            GL.Vertex3(1f, 1f, frontZ);
            // Barra inferior
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1.5f, frontZ);
            GL.Vertex3(0.5f, -1.5f, frontZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.End();

            //Cara trasera (vértices en orden inverso para la orientación correcta) 
            GL.Begin(PrimitiveType.Quads);
            // Barra vertical izquierda
            GL.Vertex3(-1f, 1f, backZ);
            GL.Vertex3(-0.5f, 1f, backZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-1f, -1f, backZ);
            // Barra vertical derecha
            GL.Vertex3(0.5f, 1f, backZ);
            GL.Vertex3(1f, 1f, backZ);
            GL.Vertex3(1f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            // Barra inferior
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1.5f, backZ);
            GL.Vertex3(-0.5f, -1.5f, backZ);
            GL.End();

            // Conexión lateral entre la parte frontal y trasera
            // Barra vertical izquierda
            GL.Begin(PrimitiveType.Quads);
            // Lado izquierdo
            GL.Vertex3(-1f, 1f, frontZ);
            GL.Vertex3(-1f, 1f, backZ);
            GL.Vertex3(-1f, -1f, backZ);
            GL.Vertex3(-1f, -1f, frontZ);
            // Lado derecho
            GL.Vertex3(-0.5f, 1f, frontZ);
            GL.Vertex3(-0.5f, 1f, backZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-0.5f, -1f, frontZ);
            // Superior
            GL.Vertex3(-1f, 1f, frontZ);
            GL.Vertex3(-1f, 1f, backZ);
            GL.Vertex3(-0.5f, 1f, backZ);
            GL.Vertex3(-0.5f, 1f, frontZ);
            // Inferior
            GL.Vertex3(-1f, -1f, frontZ);
            GL.Vertex3(-1f, -1f, backZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.End();

            // Barra vertical derecha
            GL.Begin(PrimitiveType.Quads);
            // Lado izquierdo
            GL.Vertex3(0.5f, 1f, frontZ);
            GL.Vertex3(0.5f, 1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            // Lado derecho
            GL.Vertex3(1f, 1f, frontZ);
            GL.Vertex3(1f, 1f, backZ);
            GL.Vertex3(1f, -1f, backZ);
            GL.Vertex3(1f, -1f, frontZ);
            // Superior
            GL.Vertex3(0.5f, 1f, frontZ);
            GL.Vertex3(0.5f, 1f, backZ);
            GL.Vertex3(1f, 1f, backZ);
            GL.Vertex3(1f, 1f, frontZ);
            // Inferior
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(1f, -1f, backZ);
            GL.Vertex3(1f, -1f, frontZ);
            GL.End();

            // Barra inferior
            GL.Begin(PrimitiveType.Quads);
            // Lado izquierdo
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(-0.5f, -1.5f, backZ);
            GL.Vertex3(-0.5f, -1.5f, frontZ);
            // Lado derecho
            GL.Vertex3(0.5f, -1f, frontZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1.5f, backZ);
            GL.Vertex3(0.5f, -1.5f, frontZ);
            // Superior
            GL.Vertex3(-0.5f, -1f, frontZ);
            GL.Vertex3(-0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, backZ);
            GL.Vertex3(0.5f, -1f, frontZ);
            // Inferior
            GL.Vertex3(-0.5f, -1.5f, frontZ);
            GL.Vertex3(-0.5f, -1.5f, backZ);
            GL.Vertex3(0.5f, -1.5f, backZ);
            GL.Vertex3(0.5f, -1.5f, frontZ);
            GL.End();
        }

        public void DrawAxes()
        {
            float axisLength = 5.0f;

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