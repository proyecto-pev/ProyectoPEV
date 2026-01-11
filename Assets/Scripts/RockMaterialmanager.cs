using UnityEngine;

public class RockMaterialManager : MonoBehaviour
{
    [Header("Colores")]
    public Color colorNeutro = Color.gray;
    public Color colorDistopico = new Color(0.15f, 0.15f, 0.15f);

    [Header("Estado global")]
    [Range(0f, 10f)] public float estadoMin = 0f;
    [Range(0f, 10f)] public float estadoMax = 10f;

    private Renderer[] rockRenderers;

    void Start()
    {
        // Buscar todas las rocas por TAG
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
        rockRenderers = new Renderer[rocks.Length];

        for (int i = 0; i < rocks.Length; i++)
        {
            rockRenderers[i] = rocks[i].GetComponentInChildren<Renderer>();
        }

        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarRocas;
            ActualizarRocas(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarRocas(float estado)
    {
        float t = Mathf.Clamp01((estado - estadoMin) / (estadoMax - estadoMin));

        foreach (Renderer r in rockRenderers)
        {
            if (r == null) continue;

            // IMPORTANTE: instancia propia del material
            Material mat = r.material;
            mat.color = Color.Lerp(colorNeutro, colorDistopico, t);
        }
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarRocas;
    }
}
