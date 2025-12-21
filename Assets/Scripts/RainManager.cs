using UnityEngine;

public class RainManager : MonoBehaviour
{
    [Header("Configuración")]
    public float umbralLluvia = 40f;

    [Header("Sistema de lluvia")]
    public ParticleSystem lluvia;

    void Start()
    {
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ComprobarLluvia;
            ComprobarLluvia(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ComprobarLluvia(float estado)
    {
        if (lluvia == null) return;

        if (estado >= umbralLluvia)
        {
            if (!lluvia.isPlaying)
                lluvia.Play();
        }
        else
        {
            if (lluvia.isPlaying)
                lluvia.Stop();
        }
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ComprobarLluvia;
    }
}
