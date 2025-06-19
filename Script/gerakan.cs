using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class gerakan : MonoBehaviour
{
    private float moveSpeed = 1.5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        //animasi
        bool tekanWASD = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool tekanShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        animator.SetBool("Jalan", tekanWASD && !tekanShift);
        animator.SetBool("Lari", tekanWASD && tekanShift);
        animator.SetBool("Diam", !tekanWASD);

        // Ubah kecepatan tergantung animasi
        if (animator.GetBool("Lari"))
        {
            moveSpeed = 4f;
        }
        else if (animator.GetBool("Jalan"))
        {
            moveSpeed = 1.5f;
        }

        // Cek apakah karakter menyentuh tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset kecepatan jatuh agar nempel di tanah
        }

        // Input gerakan horizontal dan vertikal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Ambil arah kamera utama
        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        // Hilangkan efek vertikal dari arah kamera
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Hitung arah gerakan berdasarkan kamera
        Vector3 move = camRight * x + camForward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravitasi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
