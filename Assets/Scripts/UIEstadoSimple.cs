using UnityEngine;
using UnityEngine.UI;

public class UIEstadoSimple : MonoBehaviour
{
    public Slider sliderEstado;
    public Text textoEstado;

    void Start()
    {
        if (sliderEstado != null)
        {
            sliderEstado.minValue = 0;
            sliderEstado.maxValue = 100;
        }

        // SUSCRIPCIÓN AL OBSERVER
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.OnEstadoCambiado += ActualizarUI;

            // Inicializar UI
            ActualizarUI(WorldManager.Instance.GetEstadoActual());
        }
    }

    void ActualizarUI(float estado)
    {
        if (sliderEstado != null)
            sliderEstado.value = estado;

        if (textoEstado != null)
            textoEstado.text = "Estado: " + Mathf.Round(estado);
    }

    void OnDestroy()
    {
        // DESUSCRIPCIÓN (muy importante)
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarUI;
    }
}

