using UnityEngine;

public class CampFireManager : MonoBehaviour
{
    [Header("Sistema de partículas")]
    public ParticleSystem fuego;

    [Header("Colores")]
    public Color colorNeutro = new Color(1f, 0.6f, 0.2f);   // fuego vivo
    public Color colorDistopico = new Color(0.3f, 0.3f, 0.3f); // ceniza/apagado

    [Header("Intensidad")]
    public float intensidadMin = 5f;
    public float intensidadMax = 40f;

    void Start()
    {
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarFuego;
            ActualizarFuego(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarFuego(float estado)
    {
        if (fuego == null) return;

        float t = Mathf.InverseLerp(5f, 80f, estado);

        // Intensidad del fuego
        var emission = fuego.emission;
        emission.rateOverTime = Mathf.Lerp(intensidadMax, intensidadMin, t);

        // Color del fuego
        var main = fuego.main;
        main.startColor = Color.Lerp(colorNeutro, colorDistopico, t);

        // Asegurarse de que está activo
        if (!fuego.isPlaying)
            fuego.Play();
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarFuego;
    }
}
