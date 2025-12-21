using UnityEngine;

public class RainManager : MonoBehaviour
{
    [Header("Configuración")]
    public float intensidadMin = 10f;     // Lluvia ligera desde el inicio
    public float intensidadMax = 2000f;   // Tormenta máxima
    public float velocidadMin = 5f;       // Velocidad mínima
    public float velocidadMax = 80f;      // Velocidad máxima en tormenta
    public float tamañoMin = 0.1f;        // Tamaño mínimo
    public float tamañoMax = 0.6f;        // Tamaño máximo en tormenta
    public float umbralTormenta = 30f;    // Estado a partir del cual es tormenta

    [Header("Sistema de lluvia")]
    public ParticleSystem lluvia;

    void Start()
    {
        if (lluvia == null)
        {
            Debug.LogWarning("RainManager: asigna un Particle System a 'lluvia'");
            return;
        }

        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarLluvia;
            ActualizarLluvia(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarLluvia(float estado)
    {
        if (lluvia == null) return;

        var emission = lluvia.emission;
        var main = lluvia.main;

        // Normalizamos el estado 0-1
        float t = Mathf.Clamp01(estado / 100f);

        // Factor para transición progresiva (no lineal)
        float factor = Mathf.Pow(t, 2f); // empieza suave y aumenta rápido

        // Intensidad
        float intensidad = Mathf.Lerp(intensidadMin, intensidadMax, factor);
        emission.rateOverTime = intensidad;

        // Velocidad
        main.startSpeed = Mathf.Lerp(velocidadMin, velocidadMax, factor);

        // Tamaño
        main.startSize = Mathf.Lerp(tamañoMin, tamañoMax, factor);

        // Asegurarnos de que siempre se reproduzca
        if (!lluvia.isPlaying && intensidad > 0f)
            lluvia.Play();

        // Tormenta brutal si estado > umbral
        if (estado >= umbralTormenta)
        {
            emission.rateOverTime = intensidadMax;    // máxima densidad
            main.startSpeed = velocidadMax;           // máxima velocidad
            main.startSize = tamañoMax;              // gotas grandes
        }
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarLluvia;
    }
}
