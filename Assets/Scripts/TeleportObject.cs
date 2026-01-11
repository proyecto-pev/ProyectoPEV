using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    [Header("Rango de teletransporte")]
    public Vector3 minPos = new Vector3(-5f, 0.5f, -5f);
    public Vector3 maxPos = new Vector3(5f, 2f, 5f);

    // Llamar este método para teletransportar el objeto
    public void Teletransportar()
    {
        float x = Random.Range(minPos.x, maxPos.x);
        float y = Random.Range(minPos.y, maxPos.y);
        float z = Random.Range(minPos.z, maxPos.z);

        transform.position = new Vector3(x, y, z);

        Debug.Log($"Objeto teletransportado a ({x:F2}, {y:F2}, {z:F2})");
    }
}
