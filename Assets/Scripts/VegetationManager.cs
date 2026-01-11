using UnityEngine;
using System.Collections.Generic;

public class VegetationManager : MonoBehaviour
{
    public string tagVegetacion = "Vegetacion";

    public float inicioMarchitamiento = 3f;
    public float estadoDesaparecer = 8f;

    public Vector3 escalaMinima = new Vector3(0.05f, 0.05f, 0.05f);

    Dictionary<Transform, Vector3> plantas = new Dictionary<Transform, Vector3>();

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tagVegetacion);

        foreach (GameObject obj in objs)
        {
            plantas.Add(obj.transform, obj.transform.localScale);
        }
    }

    void Update()
    {
        float estado = WorldManager.Instance.GetEstadoActual();

        foreach (var planta in plantas)
        {
            Transform t = planta.Key;
            Vector3 escalaInicial = planta.Value;

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
