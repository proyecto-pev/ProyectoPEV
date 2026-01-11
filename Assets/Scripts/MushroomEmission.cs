using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MushroomEmission : MonoBehaviour
{
    [Header("Material y Glow")]
    public int capMaterialIndex = 1;         // Índice del material del caparazón
    public float maxGlowStrength = 5f;       // Valor máximo de Glow Strength cuando estadoGlobal = 10
    public string glowProperty = "_GlowStrength"; // Nombre de la propiedad en tu shader

    private Material capMaterialInstance;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null && rend.materials.Length > capMaterialIndex)
        {
            // Crear instancia del material del caparazón
            capMaterialInstance = new Material(rend.materials[capMaterialIndex]);
            Material[] mats = rend.materials;
            mats[capMaterialIndex] = capMaterialInstance;
            rend.materials = mats;
        }

        // Suscribirse al evento de WorldManager
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarGlow;
        }
    }

    void OnDestroy()
    {
        // Desuscribirse del evento
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado -= ActualizarGlow;
        }
    }

    // Función que actualiza Glow Strength según estado global
    private void ActualizarGlow(float estado)
    {
        if (capMaterialInstance == null) return;

        // Mapear estado 0-10 a 0-1
        float t = Mathf.InverseLerp(0f, 10f, estado);

        // Actualizar solo Glow Strength
        capMaterialInstance.SetFloat(glowProperty, t * maxGlowStrength);
    }
}
