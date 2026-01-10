using UnityEngine;
using System.Collections.Generic;

public class TreeManager : MonoBehaviour
{
    public string tagArbol = "Arbol";

    public float inicioMarchitamiento = 3f;
    public float estadoDesaparecer = 8f;

    public Vector3 escalaMinima = new Vector3(0.1f, 0.1f, 0.1f);

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

            if (estado >= estadoDesaparecer)
            {
                t.gameObject.SetActive(false);
                continue;
            }

            t.gameObject.SetActive(true);

            if (estado <= inicioMarchitamiento)
            {
                t.localScale = escalaInicial;
            }
            else
            {
                float t01 = Mathf.InverseLerp(inicioMarchitamiento, estadoDesaparecer, estado);
                t.localScale = Vector3.Lerp(escalaInicial, escalaMinima, t01);
            }
        }
    }
}
