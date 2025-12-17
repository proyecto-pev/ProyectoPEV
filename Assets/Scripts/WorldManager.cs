using UnityEngine;
public class WorldManager : MonoBehaviour
{
    public float estadoGlobal = 0f;
    public static WorldManager Instance;

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
    }

    public float GetEstadoActual()
    {
        return estadoGlobal;
    }
}