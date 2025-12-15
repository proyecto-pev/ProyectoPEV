using UnityEngine;
using UnityEngine;

public class InteractuableSimple : MonoBehaviour
{
    public float puntos = 10f; // Puntos que da al mundo
    public bool esPositivo = true; // True = bueno, False = malo

    // Materiales para mostrar cambio
    public Material materialNormal;
    public Material materialUsado;

    private bool usado = false;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null && materialNormal != null)
        {
            rend.material = materialNormal;
        }
    }

    public void Usar()
    {
        if (usado) return;

        usado = true;

        // Cambiar material
        if (rend != null && materialUsado != null)
        {
            rend.material = materialUsado;
        }

        // Cambiar estado del mundo
        if (WorldManager.Instance != null)
        {
            float cantidad = esPositivo ? puntos : -puntos;
            WorldManager.Instance.CambiarEstado(cantidad);
        }

        // Desactivar después de un tiempo
        Invoke("Desactivar", 2f);
    }

    void Desactivar()
    {
        gameObject.SetActive(false);
    }
}
