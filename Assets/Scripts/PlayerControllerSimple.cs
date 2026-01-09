using UnityEngine;
using System.Linq;

public class PlayerControllerSimple : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Interacción")]
    public float rangoHighlight = 2.5f;
    public float rangoInteraccion = 2f;

    [Header("Feedback")]
    public AudioClip sonidoInteraccionPositiva;
    public AudioClip sonidoInteraccionNegativa;

    Transform cam;
    private InteractuableSimple objetoResaltado = null;
    private AudioSource audioSource;

    void Start()
    {
        cam = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

    void Interactuar()
    {
        if (Input.GetKeyDown(KeyCode.E) && objetoResaltado != null)
        {
            float distancia = Vector3.Distance(transform.position, objetoResaltado.transform.position);

            if (distancia <= rangoInteraccion)
            {
                // Reproducir sonido según tipo de objeto
                if (audioSource != null)
                {
                    AudioClip clip = objetoResaltado.esNegativo ?
                        sonidoInteraccionNegativa : sonidoInteraccionPositiva;

                    if (clip != null)
                    {
                        audioSource.PlayOneShot(clip);
                    }
                }

                objetoResaltado.Usar();

                // Opcional: feedback visual
                StartCoroutine(FeedbackInteraccion());

                objetoResaltado = null;
            }
        }
    }

    System.Collections.IEnumerator FeedbackInteraccion()
    {
        // Pequeño feedback de movimiento
        Vector3 originalPos = transform.position;
        transform.position += Vector3.up * 0.1f;
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoHighlight);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoInteraccion);
    }
}