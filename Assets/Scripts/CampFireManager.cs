using UnityEngine;

public class CampFireManager : MonoBehaviour
{
    [Header("Sistema de partículas")]
    public ParticleSystem fuego;

    [Header("Configuración")]
    [Range(0f, 10f)] public float inicioApagado = 3f;
    [Range(0f, 10f)] public float fuegoApagado = 8f;

    [Header("Efectos")]
    [Min(0)] public float particulasEnEstado0 = 15f;  // Menos fuego al inicio
    [Min(0)] public float particulasEnEstado8 = 3f;   // Casi apagado en estado 8
    [Min(0)] public float particulasApagado = 0f;     // Totalmente apagado

    public Color colorNormal = new Color(1f, 0.6f, 0.2f, 0.8f);   // Naranja suave
    public Color colorApagandose = new Color(0.8f, 0.4f, 0.1f, 0.4f); // Naranja oscuro
    public Color colorApagado = new Color(0.3f, 0.3f, 0.3f, 0.1f);    // Gris tenue

    void Start()
    {
        // Buscar ParticleSystem si no está asignado
        if (fuego == null)
        {
            fuego = GetComponent<ParticleSystem>();
            if (fuego == null)
                fuego = GetComponentInChildren<ParticleSystem>();
        }

        if (fuego == null)
        {
            Debug.LogWarning($"CampFireManager en {gameObject.name}: No se encontró ParticleSystem");
            enabled = false;
            return;
        }

        // Suscribirse a eventos
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarFuego;
            ActualizarFuego(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarFuego(float estado)
    {
        if (fuego == null) return;

        float cantidadParticulas;
        Color colorActual;

        if (estado <= inicioApagado)
        {
            // Estado 0-3: Fuego normal (pero menos intenso)
            cantidadParticulas = particulasEnEstado0;
            colorActual = colorNormal;
        }
        else if (estado <= fuegoApagado)
        {
            // Estado 3-8: Fuego se apaga gradualmente
            float t = Mathf.InverseLerp(inicioApagado, fuegoApagado, estado);
            cantidadParticulas = Mathf.Lerp(particulasEnEstado0, particulasEnEstado8, t);
            colorActual = Color.Lerp(colorNormal, colorApagandose, t);
        }
        else
        {
            // Estado 8-10: Fuego apagado o cenizas
            float t = Mathf.InverseLerp(fuegoApagado, 10f, estado);
            cantidadParticulas = Mathf.Lerp(particulasEnEstado8, particulasApagado, t);
            colorActual = Color.Lerp(colorApagandose, colorApagado, t);
        }

        // Aplicar cambios
        var emission = fuego.emission;
        emission.rateOverTime = cantidadParticulas;

        var main = fuego.main;
        main.startColor = colorActual;

        // Encender o apagar según cantidad de partículas
        if (cantidadParticulas > 0.1f && !fuego.isPlaying)
        {
            fuego.Play();
        }
        else if (cantidadParticulas <= 0.1f && fuego.isPlaying)
        {
            fuego.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarFuego;
    }

    // Métodos para test en el editor
    [ContextMenu("Test Estado 0")]
    void TestEstado0() => ActualizarFuego(0f);

    [ContextMenu("Test Estado 3")]
    void TestEstado3() => ActualizarFuego(3f);

    [ContextMenu("Test Estado 5")]
    void TestEstado5() => ActualizarFuego(5f);

    [ContextMenu("Test Estado 8")]
    void TestEstado8() => ActualizarFuego(8f);

    [ContextMenu("Test Estado 10")]
    void TestEstado10() => ActualizarFuego(10f);
}