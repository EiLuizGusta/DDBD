using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BedScript : MonoBehaviour
{
    public GameObject tvOff;
    public GameObject tvOn;
    public GameObject jogador;
    public GameObject deitado;

    [SerializeField]
    private TextMeshProUGUI mensagemPressionarE; // TextMeshProUGUI para a mensagem de pressionar E
    [SerializeField]
    private TextMeshProUGUI mensagemPressionarR;

    public string cenaParaCarregar; // Nome da cena para carregar

    private KeyCode Interact = KeyCode.E;
    private KeyCode Dormir = KeyCode.R;

    private bool tvLigada = false;

    private void Start()
    {
        // Desativa as mensagens inicialmente
        mensagemPressionarE.gameObject.SetActive(false);
        mensagemPressionarR.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(Interact) && IsPlayerNearTV())
        {
            ToggleTVObjects();
        }

        if (IsPlayerNearDeitado())
        {
            // Ativa a mensagem ao estar na área de deitar
            mensagemPressionarR.gameObject.SetActive(true);

            // Se o jogador pressionar a tecla R, leva para outra cena
            if (Input.GetKeyDown(Dormir))
            {
                IrParaOutraCena();
            }
        }
        else
        {
            // Desativa a mensagem se o jogador não estiver na área de deitar
            mensagemPressionarR.gameObject.SetActive(false);
        }

        if (IsPlayerNearTV())
        {
            // Sobrepor a mensagem ao estar na área da TVOff
            mensagemPressionarE.text = "Aperte 'E' para ligar a TV";

            // Ativa a mensagem
            mensagemPressionarE.gameObject.SetActive(true);

            // Desativa a mensagem quando a tecla E for pressionada
            if (Input.GetKeyDown(Interact))
            {
                mensagemPressionarE.gameObject.SetActive(false);
            }
        }
        else
        {
            // Limpar a mensagem se o jogador não estiver na área da TVOff
            mensagemPressionarE.text = "";
            mensagemPressionarE.gameObject.SetActive(false);
        }
    }

    private bool IsPlayerNearTV()
    {
        // Verifica se o jogador está perto da TVOff usando o collider
        Collider tvCollider = tvOff.GetComponent<Collider>();
        return tvCollider.bounds.Intersects(jogador.GetComponent<Collider>().bounds);
    }

    private bool IsPlayerNearDeitado()
    {
        // Verifica se o jogador está perto da área de deitar usando o collider
        Collider deitadoCollider = deitado.GetComponent<Collider>();
        return deitadoCollider.bounds.Intersects(jogador.GetComponent<Collider>().bounds);
    }

    private void ToggleTVObjects()
    {
        // Inverte a ativação/desativação dos objetos
        tvOff.SetActive(!tvOff.activeSelf);
        tvOn.SetActive(!tvOn.activeSelf);
    }

    private void IrParaOutraCena()
    {
        if (!string.IsNullOrEmpty(cenaParaCarregar))
        {
            SceneManager.LoadScene(cenaParaCarregar);
        }
        else
        {
            Debug.LogError("Nome da cena para carregar não definido!");
        }
    }
}