using UnityEngine;
using System.Linq;

public class PlayerControllerSimple : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Interacción")]
    public float rangoInteraccion = 3f;      // distancia frontal
    public float radioInteraccion = 1.5f;    // ancho/alto de la zona de interacción

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

    void Mover()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h;

        if (moveDir.magnitude > 0.1f)
        {
            transform.position += moveDir * speed * Time.deltaTime;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    void DetectarObjetosCercanos()
    {
        Vector3 centro = transform.position + transform.forward * (rangoInteraccion / 2f) + Vector3.up * 1.5f;
        Vector3 halfExtents = new Vector3(radioInteraccion, radioInteraccion, rangoInteraccion / 2f);

        // Detecta colliders en un box frontal
        Collider[] colliders = Physics.OverlapBox(centro, halfExtents);

        // Filtrar solo objetos con tag "Interactuable" y no usados
        var objetosInteractuables = colliders
            .Select(c => c.GetComponent<InteractuableSimple>())
            .Where(o => o != null && !o.Usado)
            .ToList();

        // Elegir el más cercano a la cámara
        InteractuableSimple masCercano = null;
        float minDist = float.MaxValue;

        foreach (var obj in objetosInteractuables)
        {
            float dist = Vector3.Distance(cam.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                masCercano = obj;
            }
        }

        // Quitar highlight del anterior
        if (objetoResaltado != null && objetoResaltado != masCercano)
        {
            objetoResaltado.QuitarResaltar();
            objetoResaltado = null;
        }

        // Resaltar el nuevo
        if (masCercano != null && masCercano != objetoResaltado)
        {
            masCercano.Resaltar();
            objetoResaltado = masCercano;
        }
    }

    void Interactuar()
    {
        if (Input.GetKeyDown(KeyCode.E) && objetoResaltado != null)
        {
            objetoResaltado.Usar();
            Debug.Log("Interactuaste con: " + objetoResaltado.name);
        }
    }

    // Para debug: dibuja la caja en la escena
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 centro = transform.position + transform.forward * (rangoInteraccion / 2f) + Vector3.up * 1.5f;
        Vector3 halfExtents = new Vector3(radioInteraccion, radioInteraccion, rangoInteraccion / 2f);
        Gizmos.DrawWireCube(centro, halfExtents * 2);
    }
}
