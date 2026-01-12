using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    [Header("Tag de los objetos a teletransportar")]
    public string tagObjetos = "Teletransportable";

    [Header("Rango de teletransporte")]
    public Vector3 minPos = new Vector3(-5f, 0.5f, -5f);
    public Vector3 maxPos = new Vector3(5f, 2f, 5f);

    // Teletransporta todos los objetos con el tag
    public void TeletransportarTodos()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag(tagObjetos);

        foreach (GameObject obj in objetos)
        {
            float x = Random.Range(minPos.x, maxPos.x);
            float y = Random.Range(minPos.y, maxPos.y);
            float z = Random.Range(minPos.z, maxPos.z);

            obj.transform.position = new Vector3(x, y, z);

            Debug.Log($"Objeto {obj.name} teletransportado a ({x:F2}, {y:F2}, {z:F2})");
        }
    }
}
