using UnityEngine;
using TMPro;

public class InteracaoDeitar : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI mensagemText;
    public KeyCode teclaDeitar = KeyCode.F;

    private bool playerNaArea = false;
    private bool jogadorDeitado = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerNaArea = true;
            AtualizarMensagem("Aperte 'F' para deitar");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerNaArea = false;
            LimparMensagem();
        }
    }

    void Update()
    {
        if (playerNaArea && Input.GetKeyDown(teclaDeitar))
        {
            if (!jogadorDeitado)
            {
                // Adicione aqui a lógica para deitar, se necessário
                Debug.Log("Player deitou!");
                jogadorDeitado = true;
                LimparMensagem();
            }
            else
            {
                // Adicione aqui a lógica para levantar, se necessário
                Debug.Log("Player levantou!");
                jogadorDeitado = false;
                AtualizarMensagem("Aperte 'F' para deitar");
            }
        }
    }

    void AtualizarMensagem(string mensagem)
    {
        // Atualiza o texto da mensagem
        if (mensagemText != null)
        {
            mensagemText.text = mensagem;
        }
    }

    void LimparMensagem()
    {
        // Limpa o texto da mensagem
        if (mensagemText != null)
        {
            mensagemText.text = "";
        }
    }
}