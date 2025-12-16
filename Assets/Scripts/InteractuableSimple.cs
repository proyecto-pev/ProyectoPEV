using UnityEngine;

public class InteractuableSimple : MonoBehaviour
{
    public float puntos = 10f;
    public bool esPositivo = true;

    public Material materialNormal;
    public Material materialUsado;
    public Material materialHighlight;

    private bool usado = false;
    private Renderer rend;

    public bool Usado => usado;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null && materialNormal != null)
            rend.material = materialNormal;
    }

    public void Usar()
    {
        if (usado) return;

        usado = true;

        if (rend != null && materialUsado != null)
            rend.material = materialUsado;

        if (WorldManager.Instance != null)
        {
            float cantidad = esPositivo ? puntos : -puntos;
            WorldManager.Instance.CambiarEstado(cantidad);
        }

    }

    public void Resaltar()
    {
        if (!usado && rend != null && materialHighlight != null)
            rend.material = materialHighlight;
    }

    public void QuitarResaltar()
    {
        if (!usado && rend != null && materialNormal != null)
            rend.material = materialNormal;
    }

    void Desactivar()
    {
        gameObject.SetActive(false);
    }
}
