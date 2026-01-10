using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundManager : MonoBehaviour
{
    [Header("Configuración")]
    public float volumenMin = 0.2f;  // estado neutro
    public float volumenMax = 1f;    // estado distópico
    public AudioClip clipAmbiental;  // sonido de fondo

    private AudioSource audioSource;

    [Header("Rango de cambio")]
    [Range(0f, 10f)] public float minEstado = 0f;
    [Range(0f, 10f)] public float maxEstado = 10f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (clipAmbiental != null)
        {
            audioSource.clip = clipAmbiental;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.volume = volumenMin;
            audioSource.Play();
        }

        // Suscribirse al WorldManager
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarSonido;
            ActualizarSonido(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarSonido(float estado)
    {
        // Normalizar estado entre 0 y 1
        float t = Mathf.Clamp01((estado - minEstado) / (maxEstado - minEstado));

        // Ajustar volumen progresivamente
        audioSource.volume = Mathf.Lerp(volumenMin, volumenMax, t);

        // Opcional: pitch para efecto más tenso
        audioSource.pitch = Mathf.Lerp(1f, 1.2f, t);
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarSonido;
    }
}
