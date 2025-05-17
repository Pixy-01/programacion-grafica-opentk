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
            Pista p=new Pista();
            foreach (var obj in _objetos.Values)
                obj.Dibujar(); // Cada objeto aplica sus propias transformaciones

            DibujarEjesGlobales();
            p.Dibujar(); // Dibuja la pista después de los objetos
        }


        public void AñadirObjeto(string id, Objeto obj)
        {
            _objetos[id] = obj;
        }
        // En Escenario.cs
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
        public Objeto CrearFigura(Color4 colorChasis)
        {
            var auto = new Objeto();

            // —————— 1) Chasis (caja) ——————
            var chasis = new Parte(colorChasis);
            float hx = 1.0f, hy = 0.25f, hz = 0.5f;
            var caras = new Dictionary<string, Vector3[]>
            {
                ["Front"] = new[]{ new Vector3(-hx,-hy, hz), new Vector3( hx,-hy, hz),
                             new Vector3( hx, hy, hz), new Vector3(-hx, hy, hz) },
                ["Back"] = new[]{ new Vector3( hx,-hy,-hz), new Vector3(-hx,-hy,-hz),
                             new Vector3(-hx, hy,-hz), new Vector3( hx, hy,-hz) },
                ["Left"] = new[]{ new Vector3(-hx,-hy,-hz), new Vector3(-hx,-hy, hz),
                             new Vector3(-hx, hy, hz), new Vector3(-hx, hy,-hz) },
                ["Right"] = new[]{ new Vector3( hx,-hy, hz), new Vector3( hx,-hy,-hz),
                             new Vector3( hx, hy,-hz), new Vector3( hx, hy, hz) },
                ["Top"] = new[]{ new Vector3(-hx, hy, hz), new Vector3( hx, hy, hz),
                             new Vector3( hx, hy,-hz), new Vector3(-hx, hy,-hz) },
                ["Bottom"] = new[]{ new Vector3(-hx,-hy,-hz), new Vector3( hx,-hy,-hz),
                             new Vector3( hx,-hy, hz), new Vector3(-hx,-hy, hz) },
            };
            foreach (var kv in caras)
            {
                var poly = new Poligono(colorChasis);
                for (int v = 0; v < 4; v++)
                    poly.AñadirVertice($"v{kv.Key}{v}", new Punto(
                        kv.Value[v].X, kv.Value[v].Y, kv.Value[v].Z));
                chasis.AñadirPoligono($"Chasis_{kv.Key}", poly);
            }
            auto.AñadirParte("Chasis", chasis);

            // —————— 2) Ruedas (prisma heptagonal 3D) ——————
            Color4 colorRueda = new Color4(1f, 1f, 0f, 1f);
            float radio = 0.25f;
            float grosor = 0.1f; // pequeña profundidad en Z
            int lados = 7;
            var offsets = new[]
            {
        new Vector3(-0.8f, -0.25f,  0.6f),
        new Vector3( 0.8f, -0.25f,  0.6f),
        new Vector3(-0.8f, -0.25f, -0.6f),
        new Vector3( 0.8f, -0.25f, -0.6f),
    };

            for (int w = 0; w < 4; w++)
            {
                var rueda = new Parte(colorRueda)
                {
                    Posicion = offsets[w]
                };

                // Crear las dos caras: frontal y trasera
                var poliFrente = new Poligono(colorRueda);
                var poliAtras = new Poligono(colorRueda);

                for (int i = 0; i < lados; i++)
                {
                    float ang = MathHelper.TwoPi * i / lados;
                    float x = radio * (float)Math.Cos(ang);
                    float y = radio * (float)Math.Sin(ang);

                    poliFrente.AñadirVertice($"F{i}", new Punto(x, y, grosor / 2));
                    poliAtras.AñadirVertice($"B{i}", new Punto(x, y, -grosor / 2));
                }
                rueda.AñadirPoligono($"Frente{w + 1}", poliFrente);
                rueda.AñadirPoligono($"Atras{w + 1}", poliAtras);

                // Unir los lados (caras laterales)
                for (int i = 0; i < lados; i++)
                {
                    int siguiente = (i + 1) % lados;

                    var caraLado = new Poligono(colorRueda);
                    caraLado.AñadirVertice($"F{i}", new Punto(
                        radio * (float)Math.Cos(MathHelper.TwoPi * i / lados),
                        radio * (float)Math.Sin(MathHelper.TwoPi * i / lados),
                        grosor / 2));

                    caraLado.AñadirVertice($"F{siguiente}", new Punto(
                        radio * (float)Math.Cos(MathHelper.TwoPi * siguiente / lados),
                        radio * (float)Math.Sin(MathHelper.TwoPi * siguiente / lados),
                        grosor / 2));

                    caraLado.AñadirVertice($"B{siguiente}", new Punto(
                        radio * (float)Math.Cos(MathHelper.TwoPi * siguiente / lados),
                        radio * (float)Math.Sin(MathHelper.TwoPi * siguiente / lados),
                        -grosor / 2));

                    caraLado.AñadirVertice($"B{i}", new Punto(
                        radio * (float)Math.Cos(MathHelper.TwoPi * i / lados),
                        radio * (float)Math.Sin(MathHelper.TwoPi * i / lados),
                        -grosor / 2));

                    rueda.AñadirPoligono($"Lado{w + 1}_{i}", caraLado);
                }

                auto.AñadirParte($"Rueda{w + 1}", rueda);
            }

            return auto;
        }

    }
}
