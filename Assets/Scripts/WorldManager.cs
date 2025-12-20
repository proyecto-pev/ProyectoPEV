using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
    public float estadoGlobal = 0f;
    public static WorldManager Instance;

    // EVENTO OBSERVER
    public event Action<float> OnEstadoCambiado;

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
        estadoGlobal = Mathf.Clamp(estadoGlobal, 0f, 100f);

        // NOTIFICAR A LOS OBSERVERS
        OnEstadoCambiado?.Invoke(estadoGlobal);
    }

    public float GetEstadoActual()
    {
        return estadoGlobal;
    }
}
