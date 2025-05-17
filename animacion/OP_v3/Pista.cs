
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OP
{
    public class Pista
    {
        public Pista()
        {
            // Constructor vacío, se puede inicializar la pista aquí si es necesario
        }
        public void Dibujar()
        {
            GL.Color3(0.1f, 0.1f, 0.1f); // Asfalto oscuro

    float largo = 20f;  // largo horizontal (eje X)
    float alto = 8f;    // alto vertical (eje Z)
    float ancho = 2f;   // grosor de la pista

    // Recta superior
    GL.Begin(PrimitiveType.Quads);
    GL.Vertex3(-largo / 2, 0f, alto / 2);
    GL.Vertex3( largo / 2, 0f, alto / 2);
    GL.Vertex3( largo / 2, 0f, alto / 2 - ancho);
    GL.Vertex3(-largo / 2, 0f, alto / 2 - ancho);
    GL.End();

    // Recta derecha
    GL.Begin(PrimitiveType.Quads);
    GL.Vertex3(largo / 2, 0f, alto / 2);
    GL.Vertex3(largo / 2, 0f, -alto / 2);
    GL.Vertex3(largo / 2 - ancho, 0f, -alto / 2);
    GL.Vertex3(largo / 2 - ancho, 0f, alto / 2);
    GL.End();

    // Recta inferior
    GL.Begin(PrimitiveType.Quads);
    GL.Vertex3(largo / 2, 0f, -alto / 2);
    GL.Vertex3(-largo / 2, 0f, -alto / 2);
    GL.Vertex3(-largo / 2, 0f, -alto / 2 + ancho);
    GL.Vertex3(largo / 2, 0f, -alto / 2 + ancho);
    GL.End();

    // Recta izquierda
    GL.Begin(PrimitiveType.Quads);
    GL.Vertex3(-largo / 2, 0f, -alto / 2);
    GL.Vertex3(-largo / 2, 0f, alto / 2);
    GL.Vertex3(-largo / 2 + ancho, 0f, alto / 2);
    GL.Vertex3(-largo / 2 + ancho, 0f, -alto / 2);
    GL.End();
        }
    }
}