using UnityEngine;

public class FaunaManager : MonoBehaviour
{
    [Header("Configuración de fauna")]
    public GameObject[] faunaPrefabs;     // Prefabs de aves o peces
    public int maxFauna = 20;             // Cantidad máxima en estado máximo
    public Vector3 areaMin = new Vector3(-10f, 1f, -10f);  // Área de spawn
    public Vector3 areaMax = new Vector3(10f, 5f, 10f);

    private GameObject[] faunaInstanciada;

    void Start()
    {
        // Inicializar array
        faunaInstanciada = new GameObject[maxFauna];
        for (int i = 0; i < maxFauna; i++)
            faunaInstanciada[i] = null;

        // Suscribirse al estado global
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado += ActualizarFauna;
    }

    void ActualizarFauna(float estado)
    {
        // Determinar cuántos deben estar activos
        int cantidadActiva = Mathf.RoundToInt(Mathf.Lerp(0, maxFauna, estado / 10f));

        for (int i = 0; i < maxFauna; i++)
        {
            if (i < cantidadActiva)
            {
                if (faunaInstanciada[i] == null)
                {
                    // Instanciar nueva fauna aleatoria
                    GameObject prefab = faunaPrefabs[Random.Range(0, faunaPrefabs.Length)];
                    Vector3 pos = new Vector3(
                        Random.Range(areaMin.x, areaMax.x),
                        Random.Range(areaMin.y, areaMax.y),
                        Random.Range(areaMin.z, areaMax.z)
                    );
                    faunaInstanciada[i] = Instantiate(prefab, pos, Quaternion.identity);
                    faunaInstanciada[i].transform.parent = transform; // Para mantener orden
                }
            }
            else
            {
                if (faunaInstanciada[i] != null)
                {
                    Destroy(faunaInstanciada[i]);
                    faunaInstanciada[i] = null;
                }
            }
        }
    }

    void OnDestroy()
    {
        if (WorldManager.Instance != null)
            WorldManager.Instance.OnEstadoCambiado -= ActualizarFauna;
    }
}
