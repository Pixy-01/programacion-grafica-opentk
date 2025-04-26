using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace OP
{
    public class Control
    {
        // Parámetros controlables
        public Vector3 Posicion { get; private set; } = Vector3.Zero;
        public Vector3 Rotacion { get; private set; } = Vector3.Zero;
        public float Escala { get; private set; } = 1.0f;

        public Objeto ObjetoSeleccionado { get; set; }

        // Propiedades para transformaciones GLOBALES (escenario)
        public Vector3 RotacionGlobal { get; private set; } = Vector3.Zero;
        public Vector3 PosicionGlobal { get; private set; } = Vector3.Zero;//centro de masa de escenario

        // Velocidades
        public float VelocidadMovimiento { get; set; } = 2.0f;
        public float VelocidadRotacion { get; set; } = 50.0f;
        public float VelocidadEscala { get; set; } = 1.0f;

        // Actualiza los valores según las teclas presionadas

        public void Update(KeyboardState teclado, float deltaTime)
        {
            // === Controles GLOBALES (Escenario) ===
            // Rotación global en X
            if (teclado.IsKeyDown(Keys.O)) RotacionGlobal += Vector3.UnitX * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.L)) RotacionGlobal -= Vector3.UnitX * VelocidadRotacion * deltaTime;

            // Rotación global en Y (teclas U y J)
            if (teclado.IsKeyDown(Keys.U)) RotacionGlobal += Vector3.UnitY * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.J)) RotacionGlobal -= Vector3.UnitY * VelocidadRotacion * deltaTime;

            // Rotación global en Y (teclas I y K)
            if (teclado.IsKeyDown(Keys.I)) RotacionGlobal += Vector3.UnitZ * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.K)) RotacionGlobal -= Vector3.UnitZ * VelocidadRotacion * deltaTime;

            // === Controles LOCALES (Figura) ===
            // Movimiento (flechas)
            if (teclado.IsKeyDown(Keys.Right)) Posicion += Vector3.UnitX * VelocidadMovimiento * deltaTime;
            if (teclado.IsKeyDown(Keys.Left)) Posicion -= Vector3.UnitX * VelocidadMovimiento * deltaTime;
            if (teclado.IsKeyDown(Keys.Up)) Posicion += Vector3.UnitY * VelocidadMovimiento * deltaTime;
            if (teclado.IsKeyDown(Keys.Down)) Posicion -= Vector3.UnitY * VelocidadMovimiento * deltaTime;
            if (teclado.IsKeyDown(Keys.M)) Posicion += Vector3.UnitZ * VelocidadMovimiento * deltaTime;
            if (teclado.IsKeyDown(Keys.N)) Posicion -= Vector3.UnitZ * VelocidadMovimiento * deltaTime;

            // Rotación en X/Y/Z
            if (teclado.IsKeyDown(Keys.W)) Rotacion += Vector3.UnitX * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.S)) Rotacion -= Vector3.UnitX * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.Q)) Rotacion += Vector3.UnitY * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.E)) Rotacion -= Vector3.UnitY * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.A)) Rotacion += Vector3.UnitZ * VelocidadRotacion * deltaTime;
            if (teclado.IsKeyDown(Keys.D)) Rotacion -= Vector3.UnitZ * VelocidadRotacion * deltaTime;

            // Escala
            if (teclado.IsKeyDown(Keys.Z)) Escala += VelocidadEscala * deltaTime;
            if (teclado.IsKeyDown(Keys.X)) Escala = Math.Max(0.1f, Escala - VelocidadEscala * deltaTime);

            // Reset (LOCAL y GLOBAL)
            if (teclado.IsKeyDown(Keys.R))
            {
                Posicion = Vector3.Zero;
                Rotacion = Vector3.Zero;
                Escala = 1.0f;
                RotacionGlobal = Vector3.Zero; // Reset también global si es necesario
                PosicionGlobal = Vector3.Zero;
            }
        }
    }
}