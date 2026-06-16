using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float crouchSpeed = 1.5f;

    [Header("Stamina")]
    [SerializeField] float maxStamina = 5f;
    [SerializeField] float staminaDrainRate = 1f;
    [SerializeField] float staminaRegenRate = 0.5f;

    [Header("Crouch")]
    [SerializeField] float standHeight = 2f;
    [SerializeField] float crouchHeight = 1f;

    [Header("Camera")]
    [SerializeField] Transform cameraTransform; // WAJIB assign di Inspector: drag child Camera ke sini
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float maxLookAngle = 80f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 4f;

    CharacterController controller;
    float currentStamina;
    float verticalVelocity;
    float cameraXRotation;
    bool isCrouching;

    const float gravity = -15f;

    public float Stamina => currentStamina;
    public float MaxStamina => maxStamina;
    public bool IsCrouching => isCrouching;
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }

    // Dipakai GhostDetection: 0 = diam/jongkok, 0.5 = jalan, 1 = lari/lompat
    public float NoiseLevel => (IsRunning || IsJumping) ? 1f : isCrouching ? 0f : 0.5f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        controller.height = standHeight;
        controller.center = new Vector3(0, standHeight / 2f, 0);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        HandleMouseLook();
        HandleCrouch();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (cameraTransform == null) return; // cameraTransform belum di-assign di Inspector

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotasi badan player kiri-kanan (360 derajat bebas, tidak di-clamp)
        transform.Rotate(Vector3.up * mouseX, Space.World);

        // Rotasi kamera atas-bawah (clamp -80 s/d 80 agar tidak balik)
        cameraXRotation -= mouseY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(cameraXRotation, 0f, 0f);
    }

    void HandleCrouch()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl)) return;

        isCrouching = !isCrouching;
        controller.height = isCrouching ? crouchHeight : standHeight;
        controller.center = new Vector3(0, controller.height / 2f, 0);
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool moving = h != 0 || v != 0;
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && !isCrouching && currentStamina > 0;

        // Update stamina
        if (wantsToRun && moving)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
            IsRunning = true;
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            IsRunning = false;
        }

        float speed = isCrouching ? crouchSpeed : (IsRunning ? runSpeed : walkSpeed);

        // Horizontal movement (relatif ke arah hadap player)
        Vector3 moveDir = transform.right * h + transform.forward * v;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f); // diagonal tidak lebih cepat

        // Jump
        if (controller.isGrounded)
        {
            IsJumping = false;
            if (verticalVelocity < 0f) verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space) && !isCrouching)
            {
                verticalVelocity = jumpForce;
                IsJumping = true;
            }
        }

        // Gravity
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDir * speed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
