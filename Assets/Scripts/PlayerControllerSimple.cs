using UnityEngine;

public class PlayerControllerSimple : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadRotacion = 2f;
    public float rangoInteraccion = 3f;

    void Update()
    {
        // Movimiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimiento = transform.right * horizontal + transform.forward * vertical;
        transform.position += movimiento * velocidad * Time.deltaTime;

        // Rotación con mouse
        float mouseX = Input.GetAxis("Mouse X") * velocidadRotacion;
        transform.Rotate(0, mouseX, 0);

        // Interacción con E
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactuar();
        }
    }

    void Interactuar()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit, rangoInteraccion))
        {
            // Si el objeto tiene script InteractuableSimple
            InteractuableSimple objeto = hit.collider.GetComponent<InteractuableSimple>();
            if (objeto != null)
            {
                objeto.Usar();
            }
        }
    }
}
