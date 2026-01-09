using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
    public float estadoGlobal = 0f;
    public static WorldManager Instance;

    // EVENTOS OBSERVER
    public event Action<float> OnEstadoCambiado;
    public event Action<int> OnObjetoNegativoInteractuado; // Nuevo evento para objetos negativos

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CambiarEstado(float cantidad)
    {
        estadoGlobal += cantidad;
        estadoGlobal = Mathf.Clamp(estadoGlobal, 0f, 10f);

        // NOTIFICAR A LOS OBSERVADORES
        OnEstadoCambiado?.Invoke(estadoGlobal);

        // Si es positivo (interacción con objeto negativo), disparar evento adicional
        if (cantidad > 0)
        {
            OnObjetoNegativoInteractuado?.Invoke((int)cantidad);
        }
    }

    public float GetEstadoActual()
    {
        return estadoGlobal;
    }
}