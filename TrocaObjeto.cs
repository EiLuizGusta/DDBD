using UnityEngine;
using TMPro;

public class TrocaObjeto : MonoBehaviour
{
    [System.Serializable]
    public struct ObjetoParaTrocar
    {
        public GameObject ligado;
        public GameObject desligado;
        public string descricao;
        public string mensagemPersonalizada; // Nova variável para a mensagem personalizada
    }

    public ObjetoParaTrocar[] objetosTroca;
    public GameObject jogador;
    public KeyCode teclaInteracao = KeyCode.E;
    public TextMeshProUGUI listaObjetosText;
    public TextMeshProUGUI mensagemText; // Adicionando o TextMeshProUGUI para a mensagem personalizada

    private bool jogadorNaArea = false;
    private int objetosConcluidos = 0;

    private void Start()
    {
        if (jogador == null)
        {
            Debug.LogError("A referência do jogador não foi atribuída no inspector.");
        }

        // Inscreva-se no evento OnDialogueComplete para exibir a lista de objetos após o diálogo
        DoctorDialogue.OnDialogueComplete += ExibirListaObjetos;

        // Desativar o TextMeshProUGUI inicialmente
        mensagemText.gameObject.SetActive(false);
    }

    private void Update()
    {
        jogadorNaArea = false; // Reinicializa a variável antes de verificar novamente

        foreach (ObjetoParaTrocar objetoTroca in objetosTroca)
        {
            Collider jogadorCollider = jogador.GetComponent<Collider>();
            Collider objetoLigadoCollider = objetoTroca.ligado.GetComponent<Collider>();

            if (jogadorCollider.bounds.Intersects(objetoLigadoCollider.bounds))
            {
                jogadorNaArea = true;
                // Atualiza a mensagem personalizada quando o jogador está na área
                mensagemText.text = objetoTroca.mensagemPersonalizada;
                break;
            }
        }

        // Ativa ou desativa o TextMeshProUGUI com base na presença do jogador na área
        mensagemText.gameObject.SetActive(jogadorNaArea);

        if (jogadorNaArea && Input.GetKeyDown(teclaInteracao))
        {
            foreach (ObjetoParaTrocar objetoTroca in objetosTroca)
            {
                Collider jogadorCollider = jogador.GetComponent<Collider>();
                Collider objetoLigadoCollider = objetoTroca.ligado.GetComponent<Collider>();

                if (jogadorCollider.bounds.Intersects(objetoLigadoCollider.bounds))
                {
                    TrocarObjetos(objetoTroca);
                    AtualizarListaObjetos();

                    if (++objetosConcluidos == objetosTroca.Length)
                    {
                        listaObjetosText.text = "";
                        ProgressoController.Instance.CompleteObjetosTroca();
                    }

                    // Desativa o TextMeshProUGUI após a interação
                    mensagemText.gameObject.SetActive(false);

                    return;
                }
            }
        }
    }

    private void ExibirListaObjetos()
    {
        // Verifica se a lista de objetos está ativada
        if (listaObjetosText.gameObject.activeSelf)
        {
            string listaObjetos = "Itens a Fazer:\n";

            foreach (ObjetoParaTrocar objetoTroca in objetosTroca)
            {
                if (objetoTroca.ligado.activeSelf)
                {
                    listaObjetos += $"{objetoTroca.descricao}\n";
                }
            }

            listaObjetosText.text = listaObjetos;
        }
    }

    private void TrocarObjetos(ObjetoParaTrocar objetoTroca)
    {
        objetoTroca.ligado.SetActive(false);
        objetoTroca.desligado.SetActive(true);

        Debug.Log("Objeto Trocado: " + objetoTroca.ligado.name + " desligado, " + objetoTroca.desligado.name + " ligado.");
    }

    private void AtualizarListaObjetos()
    {
        ExibirListaObjetos();
    }
}