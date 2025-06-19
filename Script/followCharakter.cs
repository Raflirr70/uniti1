using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;               // Karakter yang diikuti
    public float distance = 0.7f;            // Jarak awal kamera
    public float zoomSpeed = 2f;           // Kecepatan zoom
    public float minDistance = 0.5f;         // Zoom minimum
    public float maxDistance = 1.2f;        // Zoom maksimum

    public float rotationSpeed = 5f;       // Kecepatan orbit
    private float currentX = 0f;
    private float currentY = 15f;
    public float minY = -60f;              // Batas rotasi vertikal
    public float maxY = 30f;

    void LateUpdate()
    {


        if (target == null) return;

        // Mouse orbit
        if (Input.GetMouseButton(1)) // Klik kanan tahan untuk rotasi
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minY, maxY);
        }

        // Mouse scroll zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Hitung posisi kamera
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 dir = new Vector3(0, 0.3f, -distance);
        Vector3 position = target.position + rotation * dir;

        transform.position = position;
        transform.LookAt(target);
    }
}