using UnityEngine;

public class FogManager : MonoBehaviour
{
    [Header("Configuración niebla")]
    public Color colorNeutro = new Color(0.8f, 0.8f, 0.8f, 1f);  // color inicial
    public Color colorDistopico = new Color(0.2f, 0.2f, 0.2f, 1f); // color extremo
    public float densidadMin = 0.001f;
    public float densidadMax = 0.03f;

    [Header("Rango de cambio")]
    [Range(0f, 10f)] public float minEstado = 0f;
    [Range(0f, 10f)] public float maxEstado = 10f;

    void Start()
    {
        // Activar la niebla
        RenderSettings.fog = true;

        // Suscribirse al WorldManager
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarNiebla;
            ActualizarNiebla(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarNiebla(float estado)
    {
        // Normalizar estado entre 0 y 1
        float t = Mathf.Clamp01((estado - minEstado) / (maxEstado - minEstado));

        // Cambiar color y densidad progresivamente
        RenderSettings.fogColor = Color.Lerp(colorNeutro, colorDistopico, t);
        RenderSettings.fogDensity = Mathf.Lerp(densidadMin, densidadMax, t);
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarNiebla;
    }
}
