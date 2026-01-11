using UnityEngine;
using System.Collections.Generic;

public class TreeGrowManager : MonoBehaviour
{
    public string tagArbol = "Arbol";

    [Header("Estado")]
    public float inicioCrecimiento = 3f;
    public float estadoMaximo = 10f;

    [Header("Escalado")]
    public Vector3 escalaMaxima = new Vector3(2f, 2f, 2f);

    Dictionary<Transform, Vector3> arboles = new Dictionary<Transform, Vector3>();

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tagArbol);

        foreach (GameObject obj in objs)
        {
            arboles.Add(obj.transform, obj.transform.localScale);
        }
    }

    void Update()
    {
        float estado = WorldManager.Instance.GetEstadoActual();

        foreach (var arbol in arboles)
        {
            Transform t = arbol.Key;
            Vector3 escalaInicial = arbol.Value;

            if (estado <= inicioCrecimiento)
            {
                t.localScale = escalaInicial;
            }
            else
            {
                float t01 = Mathf.InverseLerp(inicioCrecimiento, estadoMaximo, estado);
                t.localScale = Vector3.Lerp(escalaInicial, escalaMaxima, t01);
            }
        }
    }
}
