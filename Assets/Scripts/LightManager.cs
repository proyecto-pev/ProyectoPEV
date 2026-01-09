using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light luz;

    [Header("Configuración Colores")]
    public Color colorNeutro = Color.white;
    public Color colorDistopico = new Color(0.349f, 0.435f, 0.165f); // HEX #596F2A

    [Header("Configuración Intensidad")]
    public float intensidadNeutra = 1.2f;
    public float intensidadDistopica = 0.9f;

    [Header("Rango de Cambio")]
    [Range(0f, 10f)] public float minEstado = 1f;
    [Range(0f, 10f)] public float maxEstado = 10f;

    void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += OnEstadoCambiado;
            OnEstadoCambiado(WorldManager.Instance.GetEstadoActual());
        }
    }

    void OnEstadoCambiado(float estado)
    {
        if (luz == null) return;

        // Normalizar estado entre 0 y 1
        float t = Mathf.Clamp01((estado - minEstado) / (maxEstado - minEstado));

        // Aplicar inmediatamente
        luz.color = Color.Lerp(colorNeutro, colorDistopico, t);
        luz.intensity = Mathf.Lerp(intensidadNeutra, intensidadDistopica, t);

        // Opcional: ajustar temperatura de color para tono más verde
        if (luz.useColorTemperature)
        {
            float tempNeutra = 6500f; // Blanco neutro
            float tempDistopica = 4500f; // Más cálido/verdoso
            luz.colorTemperature = Mathf.Lerp(tempNeutra, tempDistopica, t);
        }

        Debug.Log($"Luz actualizada - Estado: {estado}, Color: {luz.color}");
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= OnEstadoCambiado;
    }

    // Editor helper
    void OnValidate()
    {
        // Mantener maxEstado mayor o igual que minEstado
        if (maxEstado < minEstado)
            maxEstado = minEstado;
    }
}