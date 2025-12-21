using UnityEngine;

public class SkyManager : MonoBehaviour
{
    [Header("Configuración")]
    public float umbralDistopia = 20f;
    public float velocidadCambio = 1f;

    [Header("Multiplicadores de color")]
    public Color colorUtopico = Color.white;
    public Color colorDistopico = new Color(0.4f, 0.4f, 0.5f);

    private Material skyboxMaterial;
    private Color colorObjetivo;

    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;

        if (skyboxMaterial != null)
            colorObjetivo = skyboxMaterial.GetColor("_Tint");

        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += CambiarCielo;
            CambiarCielo(WorldManager.Instance.GetEstadoActual());
        }
    }

    void Update()
    {
        if (skyboxMaterial == null) return;

        Color colorActual = skyboxMaterial.GetColor("_Tint");
        Color nuevoColor = Color.Lerp(colorActual, colorObjetivo, Time.deltaTime * velocidadCambio);
        skyboxMaterial.SetColor("_Tint", nuevoColor);
    }

    void CambiarCielo(float estado)
    {
        if (estado > umbralDistopia)
            colorObjetivo = colorDistopico;
        else
            colorObjetivo = colorUtopico;
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= CambiarCielo;
    }
}
