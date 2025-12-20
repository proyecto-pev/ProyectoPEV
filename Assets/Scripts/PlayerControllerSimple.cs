using UnityEngine;
using System.Linq;

public class PlayerControllerSimple : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Interacción")]
    public float rangoHighlight = 3f;      // Distancia para resaltar
    public float rangoInteraccion = 1.5f;  // Distancia real para usar
    public float radioInteraccion = 1.5f;

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
        Vector3 centro = transform.position + transform.forward * (rangoHighlight / 2f) + Vector3.up * 1.5f;
        Vector3 halfExtents = new Vector3(radioInteraccion, radioInteraccion, rangoHighlight / 2f);

        Collider[] colliders = Physics.OverlapBox(centro, halfExtents);

        var interactuables = colliders
            .Select(c => c.GetComponent<InteractuableSimple>())
            .Where(o => o != null && !o.Usado)
            .ToList();

        InteractuableSimple masCercano = null;
        float minDist = float.MaxValue;

        foreach (var obj in interactuables)
        {
            float dist = Vector3.Distance(cam.position, obj.transform.position);
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

    // Solo permite interactuar si está MUY cerca
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
        Vector3 centroH = transform.position + transform.forward * (rangoHighlight / 2f) + Vector3.up * 1.5f;
        Vector3 halfH = new Vector3(radioInteraccion, radioInteraccion, rangoHighlight / 2f);
        Gizmos.DrawWireCube(centroH, halfH * 2);

        // Zona real de interacción
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoInteraccion);
    }
}
