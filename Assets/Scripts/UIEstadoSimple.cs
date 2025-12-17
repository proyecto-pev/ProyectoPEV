using UnityEngine;
using UnityEngine.UI;

public class UIEstadoSimple : MonoBehaviour
{
    public Slider sliderEstado;
    public Text textoEstado;

    void Start()
    {
        // Configurar slider
        if (sliderEstado != null)
        {
            sliderEstado.minValue = 0;
            sliderEstado.maxValue = 100;
        }
    }

    void Update()
    {
        if (WorldManager.Instance != null)
        {
            float estado = WorldManager.Instance.GetEstadoActual();

            // Actualizar slider
            if (sliderEstado != null)
            {
                sliderEstado.value = estado;
            }

            // Actualizar texto
            if (textoEstado != null)
            {
                textoEstado.text = "Estado: " + Mathf.Round(estado);
            }
        }
    }
}
