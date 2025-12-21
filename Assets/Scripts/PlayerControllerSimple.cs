using UnityEngine;
using System.Linq;

public class PlayerControllerSimple : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Interacción")]
    public float rangoHighlight = 2.5f;      // Distancia para resaltar
    public float rangoInteraccion = 2f;    // Distancia real para usar

    Transform cam;
    private InteractuableSimple objetoResaltado = null;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        Mover();
        DetectarObjetosCercanos();
        Interactuar();
    }

    // Movimiento relativo a la cámara
    void Mover()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDir = camForward.normalized * v + camRight.normalized * h;

        if (moveDir.magnitude > 0.1f)
        {
            transform.position += moveDir * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDir),
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // Detecta objetos cercanos SOLO para resaltar
    void DetectarObjetosCercanos()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangoHighlight);

        var interactuables = colliders
            .Select(c => c.GetComponent<InteractuableSimple>())
            .Where(o => o != null && !o.Usado)
            .ToList();

        InteractuableSimple masCercano = null;
        float minDist = float.MaxValue;

        foreach (var obj in interactuables)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                masCercano = obj;
            }
        }

        if (objetoResaltado != null && objetoResaltado != masCercano)
        {
            objetoResaltado.QuitarResaltar();
            objetoResaltado = null;
        }

        if (masCercano != null && masCercano != objetoResaltado)
        {
            masCercano.Resaltar();
            objetoResaltado = masCercano;
        }
    }

    // Permite interactuar con E si estás dentro del rango
    void Interactuar()
    {
        if (Input.GetKeyDown(KeyCode.E) && objetoResaltado != null)
        {
            float distancia = Vector3.Distance(transform.position, objetoResaltado.transform.position);

            if (distancia <= rangoInteraccion)
            {
                objetoResaltado.Usar();
                objetoResaltado = null;
            }
        }
    }

    // Gizmos para debug
    void OnDrawGizmosSelected()
    {
        // Zona de highlight
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoHighlight);

        // Zona real de interacción
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoInteraccion);
    }
}
