using UnityEngine;

public class InteractuableSimple : MonoBehaviour
{
    // Tipos de efecto posibles
    public enum TipoEfecto
    {
        Sumar,
        Restar
    }

    [Header("Efecto")]
    public TipoEfecto tipo = TipoEfecto.Sumar;

    [Range(5, 15)]
    public int puntos = 5;   // 5, 10 o 15 (configurable)

    [Header("Materiales")]
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
            // Si es Restar, los puntos se convierten en negativos
            int valorFinal = (tipo == TipoEfecto.Sumar) ? puntos : -puntos;
            WorldManager.Instance.CambiarEstado(valorFinal);
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
}
