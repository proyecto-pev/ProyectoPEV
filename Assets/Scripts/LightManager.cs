using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light luz;

    [Header("Colores")]
    public Color colorNeutro = Color.white;
    public Color colorDistopico = new Color(0.6f, 0.5f, 0.5f);

    [Header("Intensidad")]
    public float intensidadNeutra = 1.2f;
    public float intensidadDistopica = 0.4f;

    [Header("Umbral distopía")]
    public float inicioDistopia = 30f;
    public float finDistopia = 80f;

    void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarLuz;
            ActualizarLuz(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarLuz(float estado)
    {
        float t = Mathf.InverseLerp(inicioDistopia, finDistopia, estado);

        luz.color = Color.Lerp(colorNeutro, colorDistopico, t);
        luz.intensity = Mathf.Lerp(intensidadNeutra, intensidadDistopica, t);
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarLuz;
    }
}
