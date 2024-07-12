using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float speed = 10.0f;
    public float runSpeed = 15.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -180.0f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    public Camera playerCamera;
    public float mouseSensitivity = 100.0f;

    public float yOffset = 1.0f; // Variável pública para ajustar a altura quando sentado
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float jumpTimer = 0.0f;
    private bool isJumping = false;
    private float cameraRotationX = 0.0f;
    private bool isSitting = false; // Variável para rastrear se o jogador está sentado
    private Vector3 originalPosition; // Para rastrear a posição original do personagem
    private Collider[] sitObjects; // Para rastrear os objetos "sit" nas proximidades

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("No Camera found in the scene.");
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        originalPosition = transform.position; // Salve a posição original
    }

    void Update()
    {
        if (!isSitting) {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;
            controller.Move(move * currentSpeed * Time.deltaTime);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2.0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && isJumping == false)
            {
                isJumping = true;
                jumpTimer = 1.0f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }

            if (isJumping)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    isJumping = false;
                }
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        // Rotacionar o jogador com a câmera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate around the y-axis
        transform.Rotate(Vector3.up * mouseX);

        // Rotate around the x-axis, but clamp the rotation to a certain range
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80.0f, 80.0f);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);

        // Verificar se o jogador está tentando sentar
        if (Input.GetKeyDown(KeyCode.F)) {
            if (isSitting) {
                StandUpFromSitObject();
            } else {
                // Encontrar os objetos "sit" nas proximidades usando a hitbox do Sphere Collider
                sitObjects = Physics.OverlapSphere(transform.position, controller.radius + 0.1f);

                foreach (Collider sitObject in sitObjects) {
                    if (sitObject.CompareTag("sit")) {
                        SitOnSitObject(sitObject.transform);
                        break;
                    }
                }
            }
        }
    }

    private void SitOnSitObject(Transform sitObjectTransform)
    {
        isSitting = true;
        // Salvar a posição original
        originalPosition = transform.position;

        // Posicionar o jogador no centro do objeto "sit" e ajustar a posição Y
        Vector3 newPosition = sitObjectTransform.position;
        newPosition.y += yOffset;

        // Desabilitar o CharacterController para evitar movimento
        controller.enabled = false;

        // Posicionar o jogador manualmente (evitando o CharacterController.Move)
        transform.position = newPosition;
    }

    private void StandUpFromSitObject()
    {
        isSitting = false;

        // Reposicionar o jogador para a posição original
        transform.position = originalPosition;

        // Reativar o CharacterController
        controller.enabled = true;
    }
}