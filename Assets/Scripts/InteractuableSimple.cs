using UnityEngine;

public class InteractuableSimple : MonoBehaviour
{
    [Header("Configuraci�n")]
    public bool esNegativo = true; // True = objeto negativo, False = positivo
    public float puntos = 1f; // Puntos que suma/resta
    public bool Usado { get; private set; } = false;

    [Header("Outline")]
    public Outline outlineComponent;
    private Color colorOriginal;

    void Start()
    {
        // Buscar componente Outline si no est� asignado
        if (outlineComponent == null)
        {
            outlineComponent = GetComponent<Outline>();
            if (outlineComponent == null)
            {
                outlineComponent = GetComponentInChildren<Outline>();
            }
        }

        // Desactivar outline al inicio
        if (outlineComponent != null)
        {
            outlineComponent.enabled = false;
        }

        // Establecer color inicial
        colorOriginal = esNegativo ? Color.red : Color.green;
    }

    // Llamado cuando el jugador est� cerca
    public void Resaltar()
    {
        if (outlineComponent != null && !Usado)
        {
            outlineComponent.enabled = true;

            // Asignar color seg�n tipo de objeto
            outlineComponent.OutlineColor = esNegativo ? Color.red : Color.green;
            outlineComponent.OutlineWidth = 3f; // Ancho del outline
        }
    }

    // Llamado cuando el jugador se aleja
    public void QuitarResaltar()
    {
        if (outlineComponent != null)
        {
            outlineComponent.enabled = false;
        }
    }

    // Llamado al interactuar (presionar E)
    public void Usar()
    {
        if (Usado) return;

        Usado = true;

        // Aplicar efecto visual de usado
        if (outlineComponent != null)
        {
            outlineComponent.OutlineColor = Color.gray;
            outlineComponent.OutlineWidth = 1f;
        }

        // Desactivar collider para evitar m�ltiples interacciones
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        // Llamar al teletransporte de todos los objetos con tag "Teletransportable"
        FindObjectOfType<TeleportObject>()?.TeletransportarTodos();

        // Modificar el estado global
        float cantidad = esNegativo ? puntos : -puntos;
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.CambiarEstado(cantidad);
        }

        Debug.Log($"Interactuado con {(esNegativo ? "negativo" : "positivo")}: {cantidad} puntos");
        Destroy(gameObject, 0.5f);

    }
}