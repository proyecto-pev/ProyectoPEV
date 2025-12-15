using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // Valor único del estado (0 a 1)
    public float estadoGlobal = 0f;

    // Referencia estática para acceder fácilmente
    public static WorldManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Método para cambiar el estado
    public void CambiarEstado(float cantidad)
    {
        estadoGlobal += cantidad;

        // Limitar entre 0 y 1
        if (estadoGlobal > 1f) estadoGlobal = 1f;
        if (estadoGlobal < 0f) estadoGlobal = 0f;
    }

    // Método para obtener estado actual
    public float GetEstadoActual()
    {
        return estadoGlobal;
    }
}
