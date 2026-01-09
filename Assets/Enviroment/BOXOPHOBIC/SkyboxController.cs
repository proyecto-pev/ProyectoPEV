using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [Header("Skybox")]
    public Material skyboxBase; // Arrastra el asset aquí

    private Material skyboxInstance;

    [Header("Configuración Colores")]
    public Color colorUtopico = Color.white;
    public Color colorDistopico = new Color(0.349f, 0.435f, 0.165f); // HEX #596F2A

    [Header("Transición")]
    [Range(0f, 10f)] public float inicioTransicion = 3f;
    [Range(0f, 10f)] public float finTransicion = 8f;
    public float velocidadTransicion = 1f;

    private Color colorObjetivo;
    private Color colorActual;

    void Start()
    {
        // Crear instancia del material para no modificar el original
        if (skyboxBase != null)
        {
            skyboxInstance = new Material(skyboxBase);
            RenderSettings.skybox = skyboxInstance;

            // Inicializar colores
            colorActual = colorUtopico;
            colorObjetivo = colorUtopico;

            // Actualizar material
            UpdateSkyboxColor();

            // Forzar actualización del ambiente
            DynamicGI.UpdateEnvironment();

            Debug.Log("Skybox inicializado con shader: " + skyboxInstance.shader.name);
        }
        else
        {
            Debug.LogError("No se asignó skyboxBase en el inspector!");
        }

        // Suscribirse al evento del WorldManager
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarSkybox;

            // Inicializar con el estado actual
            ActualizarSkybox(WorldManager.Instance.GetEstadoActual());
        }
    }

    void Update()
    {
        // Transición suave del color
        if (colorActual != colorObjetivo)
        {
            colorActual = Color.Lerp(colorActual, colorObjetivo, Time.deltaTime * velocidadTransicion);
            UpdateSkyboxColor();
        }
    }

    void ActualizarSkybox(float estado)
    {
        // Calcular transición entre inicioTransicion y finTransicion
        float t = Mathf.Clamp01((estado - inicioTransicion) / (finTransicion - inicioTransicion));

        // Aplicar curva de transición (opcional, para control más preciso)
        t = Mathf.SmoothStep(0f, 1f, t);

        // Calcular color objetivo
        colorObjetivo = Color.Lerp(colorUtopico, colorDistopico, t);

        Debug.Log($"Estado: {estado}, t: {t}, Color objetivo: {colorObjetivo}");
    }

    void UpdateSkyboxColor()
    {
        if (skyboxInstance == null) return;

        // Diferentes shaders de skybox usan diferentes propiedades
        string shaderName = skyboxInstance.shader.name;

        switch (shaderName)
        {
            case "Skybox/Cubemap Extended Blend":
                // Para este shader, necesitamos modificar la textura o el tint
                if (skyboxInstance.HasProperty("_Tint"))
                {
                    skyboxInstance.SetColor("_Tint", colorActual);
                }
                else if (skyboxInstance.HasProperty("_TintColor"))
                {
                    skyboxInstance.SetColor("_TintColor", colorActual);
                }
                else if (skyboxInstance.HasProperty("_Color"))
                {
                    skyboxInstance.SetColor("_Color", colorActual);
                }
                break;

            case "Skybox/Cubemap":
            case "Skybox/6 Sided":
                if (skyboxInstance.HasProperty("_Tint"))
                {
                    skyboxInstance.SetColor("_Tint", colorActual);
                }
                break;

            case "Skybox/Procedural":
                if (skyboxInstance.HasProperty("_SkyTint"))
                {
                    skyboxInstance.SetColor("_SkyTint", colorActual);
                }
                break;

            default:
                // Intenta con las propiedades más comunes
                if (skyboxInstance.HasProperty("_Tint"))
                {
                    skyboxInstance.SetColor("_Tint", colorActual);
                }
                else if (skyboxInstance.HasProperty("_TintColor"))
                {
                    skyboxInstance.SetColor("_TintColor", colorActual);
                }
                else if (skyboxInstance.HasProperty("_Color"))
                {
                    skyboxInstance.SetColor("_Color", colorActual);
                }
                else if (skyboxInstance.HasProperty("_MainColor"))
                {
                    skyboxInstance.SetColor("_MainColor", colorActual);
                }
                break;
        }

        // Forzar actualización
        RenderSettings.skybox = skyboxInstance;
        DynamicGI.UpdateEnvironment();
    }

    // Método para debug: Cambiar color manualmente
    [ContextMenu("Test Color Utopico")]
    void TestColorUtopico()
    {
        colorObjetivo = colorUtopico;
    }

    [ContextMenu("Test Color Distopico")]
    void TestColorDistopico()
    {
        colorObjetivo = colorDistopico;
    }

    void OnDestroy()
    {
        // Desuscribirse del evento
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado -= ActualizarSkybox;
        }

        // Limpiar material instanciado si es necesario
        if (skyboxInstance != null && Application.isPlaying)
        {
            Destroy(skyboxInstance);
        }
    }

    // Método para ver las propiedades del shader (útil para debug)
    [ContextMenu("Debug Shader Properties")]
    void DebugShaderProperties()
    {
        if (skyboxInstance != null)
        {
            Debug.Log("Shader: " + skyboxInstance.shader.name);
            Debug.Log("Propiedades disponibles:");

            int propertyCount = skyboxInstance.shader.GetPropertyCount();
            for (int i = 0; i < propertyCount; i++)
            {
                string propertyName = skyboxInstance.shader.GetPropertyName(i);
                Debug.Log($"- {propertyName} ({skyboxInstance.shader.GetPropertyType(i)})");
            }
        }
    }
}