using UnityEngine;
using TMPro;

public class DoctorDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float delayAfterDialogue = 3f; // Atraso em segundos
    private bool playerInRange = false;

    // Adicione uma referência direta ao TrocaObjeto
    public TrocaObjeto trocaObjeto;

    // Crie um evento para sinalizar quando o diálogo foi completamente exibido
    public delegate void DialogueCompleteEvent();
    public static event DialogueCompleteEvent OnDialogueComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueText.text = "";
        }
    }

    private void StartDialogue()
    {
        if (playerInRange && ProgressoController.Instance != null)
        {
            int currentProgress = ProgressoController.Instance.Progress;

            if (currentProgress == 0)
            {
                dialogueText.text = "Doutor: Ah, você acordou, bom dia! Seria prudente ir tomar seus remédios, um deles você deixou aqui na minha mesa";
                ProgressoController.Instance.Progress = 1;
            }
            else if (currentProgress == 1)
            {
                dialogueText.text = "Doutor: Achou seus remédios? Precisa toma-los";
            }
            else if (currentProgress == 2)
            {
                dialogueText.text = "Doutor: Você não tinha alguns afazeres diários?";
                ProgressoController.Instance.Progress = 3;
            }
            else if (currentProgress == 4)
            {
                dialogueText.text = "Doutor: Muito bem! Pode comer sua refeição já, não se esqueça dos talheres aqui na minha mesa";
            }
            else if (currentProgress == 5)
            {
                dialogueText.text = "Doutor: O que acha de ligar a TV e descansar um pouco?";
            }

            // Agende uma chamada para a função ClearDialogue após o atraso especificado
            Invoke("ClearDialogue", delayAfterDialogue);

            // Chame o evento para sinalizar que o diálogo foi completamente exibido
            if (OnDialogueComplete != null)
            {
                OnDialogueComplete();
            }
        }
        else
        {
            Debug.LogError("ProgressoController não encontrado ou jogador fora de alcance.");
        }
    }

    private void ClearDialogue()
    {
        dialogueText.text = "";
        int currentProgress = ProgressoController.Instance.Progress;

        // Chame o evento para sinalizar que o diálogo foi completamente exibido
        if (currentProgress == 3)
        {
            OnDialogueComplete();
        }
    }
}