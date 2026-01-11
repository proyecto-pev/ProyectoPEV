using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Estado global")]
    public float inicioZoom = 3f;    // Desde qué estado empieza el zoom
    public float estadoMax = 10f;    // Estado máximo

    [Header("FOV")]
    public float fovNormal = 60f;
    public float fovMaxZoom = 45f;   // Zoom máximo (campo de visión más estrecho)

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("CameraZoom: No se encontró componente Camera.");
            enabled = false;
            return;
        }

        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarZoom;
            ActualizarZoom(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarZoom(float estado)
    {
        if (cam == null) return;

        if (estado <= inicioZoom)
        {
            cam.fieldOfView = fovNormal;
            return;
        }

        float t = Mathf.InverseLerp(inicioZoom, estadoMax, estado);
        cam.fieldOfView = Mathf.Lerp(fovNormal, fovMaxZoom, t);
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarZoom;
    }
}
