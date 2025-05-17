using System.Text.Json;
using OpenTK.Mathematics;
using System.IO;

namespace OP
{
    public class Animacion
    {
        public Libreto Libreto { get; set; }
        public float TiempoActual { get; private set; }
        private bool _reproducir;
        private float _anguloAnimacion;
        private float _rotacionRuedas;

        public void CargarLibreto(string rutaLibreto)
        {
            var opciones = new JsonSerializerOptions
            {
                Converters = { new Vector3Converter() },
                WriteIndented = true
            };

            if (!File.Exists(rutaLibreto))
            {
                var libretoPorDefecto = Libreto.CrearLibretoPorDefecto();
                File.WriteAllText(rutaLibreto, JsonSerializer.Serialize(libretoPorDefecto, opciones));
            }

            string json = File.ReadAllText(rutaLibreto);
            Libreto = JsonSerializer.Deserialize<Libreto>(json, opciones);
        }

        private Vector3 InterpolarVector(Vector3 inicio, Vector3 fin, float t)
        {
            return new Vector3(
                MathHelper.Lerp(inicio.X, fin.X, t),
                MathHelper.Lerp(inicio.Y, fin.Y, t),
                MathHelper.Lerp(inicio.Z, fin.Z, t)
            );
        }

        public void Actualizar(float deltaTime, Objeto objetivo)
        {
            if (!_reproducir || Libreto == null) return;

            // Mantener tiempo dentro de la duración
            TiempoActual = MathHelper.Clamp(TiempoActual + deltaTime, 0, Libreto.Duracion);

            //pista
            float largo = Libreto.LargoPista;
            float alto = Libreto.AltoPista;
            float ancho = Libreto.AnchoPista;
            float velocidad = Libreto.Velocidad;
            float radioRuedas = Libreto.RadioRuedas;
            float transitionLength = 1.5f;
            float maxSteerAngle = 30f;

            // Cálculos de perímetro interior
            float segX = largo - ancho;
            float segZ = alto - ancho;
            float perimetro = 2 * (segX + segZ);

            // Avanzar ángulo de animación
            float distancia = velocidad * deltaTime;
            _anguloAnimacion = (_anguloAnimacion + distancia) % perimetro;
            float p = _anguloAnimacion;

            Vector2 A = new Vector2(-largo/2 + ancho/2,  alto/2 - ancho/2);
            Vector2 B = new Vector2( largo/2 - ancho/2,  alto/2 - ancho/2);
            Vector2 C = new Vector2( largo/2 - ancho/2, -alto/2 + ancho/2);
            Vector2 D = new Vector2(-largo/2 + ancho/2, -alto/2 + ancho/2);

            // Variables de salida
            Punto nuevaPos = objetivo.Posicion;
            float targetRot = 0f;
            float steerAngle = 0f;

            // Lógica de segmentos con transiciones de giro
            if (p < segX)
            {
                float t = p / segX;
                nuevaPos = new Punto(MathHelper.Lerp(A.X, B.X, t), objetivo.Posicion.Y, A.Y);
                CalcularGiro(p, segX, transitionLength, 0, 90, maxSteerAngle, out targetRot, out steerAngle);
            }
            else if (p < segX + segZ)
            {
                float t = (p - segX) / segZ;
                nuevaPos = new Punto(B.X, objetivo.Posicion.Y, MathHelper.Lerp(B.Y, C.Y, t));
                CalcularGiro(p, segX + segZ, transitionLength, 90, 180, maxSteerAngle, out targetRot, out steerAngle);
            }
            else if (p < segX + segZ + segX)
            {
                float t = (p - (segX + segZ)) / segX;
                nuevaPos = new Punto(MathHelper.Lerp(C.X, D.X, t), objetivo.Posicion.Y, C.Y);
                CalcularGiro(p, segX + segZ + segX, transitionLength, 180, 270, maxSteerAngle, out targetRot, out steerAngle);
            }
            else
            {
                float t = (p - (segX + segZ + segX)) / segZ;
                nuevaPos = new Punto(D.X, objetivo.Posicion.Y, MathHelper.Lerp(D.Y, A.Y, t));
                CalcularGiro(p, perimetro, transitionLength, 270, 360, maxSteerAngle, out targetRot, out steerAngle);
            }

            // Aplicar transformación al objeto
            objetivo.Posicion = nuevaPos;
            objetivo.Rotacion = new Vector3(0, targetRot % 360f, 0);

            // Rotación de ruedas giratorias
            float circunferencia = MathHelper.TwoPi * radioRuedas;
            _rotacionRuedas -= MathHelper.RadiansToDegrees(distancia / circunferencia);

            // Actualizar partes: delanteras con giro+spin, traseras solo spin
            foreach (var id in new[] { "Rueda1", "Rueda2" })
            {
                var rueda = objetivo.Partes[id];
                rueda.Rotacion = new Vector3(0, steerAngle, _rotacionRuedas);
            }
            foreach (var id in new[] { "Rueda3", "Rueda4" })
            {
                var rueda = objetivo.Partes[id];
                rueda.Rotacion = new Vector3(0, 0, _rotacionRuedas);
            }
        }

        private void CalcularGiro(float progreso, float finSegmento, float largoTrans, float angIni, float angFin, float maxSteer,
                                  out float angulo, out float steer)
        {
            if (progreso > finSegmento - largoTrans)
            {
                float tt = (progreso - (finSegmento - largoTrans)) / largoTrans;
                angulo = MathHelper.Lerp(angIni, angFin, tt);
                steer = MathHelper.Lerp(0f, maxSteer, tt);
            }
            else
            {
                angulo = angIni;
                steer = 0f;
            }
        }

        public void Play() => _reproducir = true;
        public void Pause() => _reproducir = false;
    }
}
