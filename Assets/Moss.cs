using UnityEngine;

[RequireComponent(typeof(InteractuableSimple))]
public class Moss : MonoBehaviour
{
    [Header("Material")]
    public Renderer rocaRenderer;
    public string overlayProperty = "_OverlayIntensity";

    [Header("Valores")]
    public float overlayConMusgo = 5f;
    public float overlaySinMusgo = 0f;
    public float velocidadCambio = 4f;

    private Material materialInstancia;
    private InteractuableSimple interactuable;

    private float overlayActual;
    private float overlayObjetivo;

    void Start()
    {
        if (rocaRenderer == null)
            rocaRenderer = GetComponent<Renderer>();

        interactuable = GetComponent<InteractuableSimple>();
        materialInstancia = rocaRenderer.material;

        // 🔹 Estado inicial según esNegativo
        if (interactuable.esNegativo)
        {
            overlayActual = overlaySinMusgo;
        }
        else
        {
            overlayActual = overlayConMusgo;
        }

        overlayObjetivo = overlayActual;
        materialInstancia.SetFloat(overlayProperty, overlayActual);
    }

    void Update()
    {
        // 🔹 Detectar que ha sido usado
        if (interactuable.Usado)
        {
            // Al usar: invertir el valor inicial
            overlayObjetivo = interactuable.esNegativo
                ? overlayConMusgo
                : overlaySinMusgo;
        }

        // 🔹 Transición suave
        overlayActual = Mathf.Lerp(
            overlayActual,
            overlayObjetivo,
            Time.deltaTime * velocidadCambio
        );

        materialInstancia.SetFloat(overlayProperty, overlayActual);
    }
}
