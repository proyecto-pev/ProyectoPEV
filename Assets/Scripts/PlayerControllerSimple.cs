using UnityEngine;

public class PlayerControllerSimple : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadRotacion = 2f;
    public float rangoInteraccion = 3f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        
        forward.y = 0;
        right.y = 0;

       
        forward.Normalize();
        right.Normalize();
       
        Vector3 movimiento = forward * vertical + right * horizontal;
        transform.position += movimiento * velocidad * Time.deltaTime;

        
        float mouseX = Input.GetAxis("Mouse X") * velocidadRotacion;
        transform.Rotate(0, mouseX, 0);
        
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
