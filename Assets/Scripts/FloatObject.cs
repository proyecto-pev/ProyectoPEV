using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [Header("Movimiento vertical")]
    public float altura = 0.5f;    // Amplitud del movimiento (qué tan alto/sube)
    public float velocidad = 1f; // Velocidad del movimiento

    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position; // Guardamos la posición inicial
    }

    void Update()
    {
        // Mathf.Sin devuelve un valor entre -1 y 1
        float yOffset = Mathf.Sin(Time.time * velocidad) * altura;
        transform.position = posInicial + new Vector3(0f, yOffset, 0f);
    }
}
