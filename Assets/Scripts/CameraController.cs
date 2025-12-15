using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;       // El Player
    public float distance = 5f;    // Distancia detrás del player
    public float height = 2f;      // Altura de la cámara
    public float mouseSensitivity = 3f;
    public float minY = -30f;
    public float maxY = 60f;

    float rotX;
    float rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Movimiento del ratón
        rotY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotX = Mathf.Clamp(rotX, minY, maxY);

        // Rotación de la cámara
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);

        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
