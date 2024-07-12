using UnityEngine;

public class Doors : MonoBehaviour
{
    public float rotationSpeed = 30.0f; // Velocidade de rotação em graus por segundo
    public string playerTag = "Player"; // Tag do objeto do jogador
    public float targetRotationY = 65.0f; // Ângulo alvo de rotação no eixo Y (pode ser ajustado no Inspector)

    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool isOpening = false;
    private bool playerInsideCollider = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInsideCollider = false;
        }
    }

    private void Start()
    {
        originalRotation = transform.rotation; // Salva a rotação original
        targetRotation = Quaternion.Euler(-90.0f, targetRotationY, 0.0f); // Rotação alvo (após pressionar "E")
    }

    private void Update()
    {
        if (playerInsideCollider && Input.GetKeyDown(KeyCode.E))
        {
            isOpening = !isOpening; // Inverte o estado da porta ao pressionar "E"
        }

        Quaternion target = isOpening ? targetRotation : originalRotation;

        // Rotação suave em direção ao alvo
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
    }
}