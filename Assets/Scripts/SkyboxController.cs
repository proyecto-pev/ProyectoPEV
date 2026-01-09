using UnityEngine;

public class SkyboxEstadoController : MonoBehaviour
{
    [Header("Skybox")]
    public Material skyboxBase; // arrastras el asset aquí

    private Material skyboxInstance;

    [Header("Colores")]
    public Color colorUtopico = Color.white;
    public Color colorDistopico = new Color(0.349f, 0.435f, 0.165f);

    void Start()
    {

        skyboxInstance = new Material(skyboxBase);

        RenderSettings.skybox = skyboxInstance;
        DynamicGI.UpdateEnvironment();
    }

    void Update()
    {
        if (WorldManager.Instance == null) return;

        float estado = WorldManager.Instance.GetEstadoActual();

        // Normalizamos de 0 → 100 (no negativo)
        float t = Mathf.Clamp01(estado / 100f);

        Color colorActual = Color.Lerp(colorUtopico, colorDistopico, t);
        skyboxInstance.SetColor("_TintColor", colorActual);

        DynamicGI.UpdateEnvironment();
    }
}
