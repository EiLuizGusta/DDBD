using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas pauseCanvas; // Arraste o Canvas do menu de pausa para este campo no Inspector
    public Button continueButton; // Arraste o botão "Continuar" para este campo no Inspector
    public string sceneName; // Nome da cena que você deseja carregar
    public Button sceneButton; // Arraste o botão que mudará de cena para este campo no Inspector

    private bool isPaused = false;
    private GameObject[] objectsWithTag;

    private void Start()
    {
        // Certifique-se de que o menu de pausa esteja desativado no início
        pauseCanvas.gameObject.SetActive(false);

        // Certifique-se de que o menu de pausa esteja ativado no início
        pauseCanvas.gameObject.SetActive(true); // Ative o Canvas

        // Configure o botão "Continuar" para chamar a função "TogglePause" quando for clicado
        continueButton.onClick.AddListener(TogglePause);

        // Configure o botão da cena para chamar a função "ChangeScene" quando for clicado
        sceneButton.onClick.AddListener(ChangeScene);

        // Encontre todos os objetos com a tag "Object" no início
        objectsWithTag = GameObject.FindGameObjectsWithTag("Object");
    }

    private void Update()
    {
        // Verifique se o jogador pressionou a tecla "Esc" ou "Enter" para ativar/desativar o menu de pausa
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            TogglePause();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // Quando o aplicativo recebe foco, como clicar nele, garantir que o cursor esteja travado e visível
        if (hasFocus)
        {
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }
    }

    // Função para ativar/desativar o menu de pausa e pausar/despausar o jogo
    private void TogglePause()
    {
        isPaused = !isPaused;

        // Ativar ou desativar o Canvas do menu de pausa
        pauseCanvas.gameObject.SetActive(isPaused);

        // Pausar ou despausar o jogo
        if (isPaused)
        {
            Time.timeScale = 0; // Pausar o jogo (tempo = 0)
        }
        else
        {
            Time.timeScale = 1; // Despausar o jogo (tempo = 1)
        }

        // Bloquear ou desbloquear o cursor do mouse
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        // Ativar ou desativar objetos com a tag "Object"
        foreach (GameObject obj in objectsWithTag)
        {
            obj.SetActive(!isPaused);
        }
    }
}