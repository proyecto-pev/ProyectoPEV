using UnityEngine;

public class RainManager : MonoBehaviour
{
    [Header("Configuración")]
    public float intensidadMin = 5f;    // Intensidad mínima siempre visible
    public float intensidadMax = 100f;  // Intensidad máxima

    [Header("Sistema de lluvia")]
    public ParticleSystem lluvia;

    void Start()
    {
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

        // Calculamos intensidad de manera progresiva
        float t = estado / 100f;
        emission.rateOverTime = Mathf.Lerp(intensidadMin, intensidadMax, t);

        // Forzamos que siempre se reproduzca si intensidad > 0
        if (emission.rateOverTime.constant > 0f && !lluvia.isPlaying)
            lluvia.Play();
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarLluvia;
    }
}
