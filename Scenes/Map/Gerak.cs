using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerak objek
    public float sprintMultiplier = 3.5f;
    public float rotationSpeed = 2f; // Kecepatan rotasi kamera

    private float rotationY;
    private float rotationX;
    private float rotationZ;

    void Start()
    {
        // Simpan rotasi awal objek
        Cursor.lockState = CursorLockMode.Locked;
        rotationY = transform.eulerAngles.y;
        rotationX = transform.eulerAngles.x;
        rotationZ = transform.eulerAngles.z;
    }

    void Update()
    {
        // Mengambil input dari keyboard
        float moveX = Input.GetAxis("Horizontal");
        float moveY = -1*Input.GetAxis("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier; // Gandakan kecepatan saat Shift ditekan
        }

        // Menggerakkan objek pada sumbu X dan Y
        Vector3 move = (transform.right * moveX + transform.up * moveY).normalized;
        transform.position += move * currentSpeed * Time.deltaTime;

        // Mengambil input dari mouse untuk rotasi
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        rotationY += mouseX;

        // Memutar objek berdasarkan pergerakan mouse tanpa mengubah rotasi awal
        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
    }
}
