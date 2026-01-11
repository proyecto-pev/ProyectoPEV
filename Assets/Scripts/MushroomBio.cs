using UnityEngine;

[RequireComponent(typeof(InteractuableSimple))]
public class MushroomBio : MonoBehaviour
{
    public Renderer capaRenderer;        // Renderer del caparazón
    public Material matBiolum;           // Material con Bioluminescence.shader
    public Color colorOpaco = Color.gray; // Color base cuando es positivo/opaco

    private InteractuableSimple interactuable;
    private Material matInstance;

    void Start()
    {
        interactuable = GetComponent<InteractuableSimple>();

        if (capaRenderer == null)
            capaRenderer = GetComponentInChildren<Renderer>();

        if (capaRenderer != null && matBiolum != null)
        {
            // Instanciar material solo para el slot del caparazón (slot 1)
            Material[] mats = capaRenderer.materials;
            mats[1] = new Material(matBiolum);
            capaRenderer.materials = mats;

            matInstance = mats[1];

            ActualizarVisual(); // Inicializamos
        }
    }

    void Update()
    {
        ActualizarVisual();
    }

    void ActualizarVisual()
    {
        if (matInstance == null || interactuable == null) return;

        // Si es negativo → bioluminescente
        if (interactuable.esNegativo)
        {
            matInstance.SetColor("_Color", Color.white);       // Color base
            matInstance.SetFloat("_LumiMultiplier", 1f);       // Emission activa
        }
        else
        {
            // Positivo → opaco
            matInstance.SetColor("_Color", colorOpaco);
            matInstance.SetFloat("_LumiMultiplier", 0f);       // Emission apagada
        }
    }
}
