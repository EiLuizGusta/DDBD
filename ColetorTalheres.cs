using System.Collections;
using UnityEngine;
using TMPro;

public class ColetorTalheres : MonoBehaviour
{
    public GameObject jogador;
    public GameObject objetoTalher;
    public GameObject objetoMarmita;
    public TextMeshProUGUI mensagemText;
    public Canvas canvas;
    public Camera mainCamera;

    public float velocidadeMovimentoMarmita = 2.5f;
    public float distanciaMinimaCamera = 2f;
    public float distanciaMaximaMarmita = 2f;

    private bool talheresColetados = false;

    void Start()
    {
        // Ativa o Canvas no início do jogo
        canvas.enabled = true;
        AtualizarMensagemColeta("");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider jogadorCollider = jogador.GetComponent<Collider>();
            Collider talherCollider = objetoTalher.GetComponent<Collider>();

            if (jogadorCollider.bounds.Intersects(talherCollider.bounds))
            {
                ColetarTalheres();
                AtualizarMensagemColeta("Pressione 'E' para pegar os talheres");
            }
            else if (talheresColetados && VerificarProximidadeMarmita())
            {
                TentarComerMarmita();
            }
            else
            {
                AtualizarMensagemColeta("Pressione 'E' para pegar os talheres");
            }
        }
    }

    void ColetarTalheres()
    {
        DesativarMeshRenderer(objetoTalher); // Desativa o MeshRenderer do talher
        talheresColetados = true;

        // Teleporta os talheres para uma posição abaixo do cenário
        objetoTalher.transform.position = new Vector3(objetoTalher.transform.position.x, -90f, objetoTalher.transform.position.z);

        // Desativa o Canvas quando os talheres são coletados
        canvas.enabled = false;

        // Adicione aqui qualquer lógica adicional que você precisa quando os talheres são coletados
        AtualizarMensagemColeta(""); // Limpa a mensagem de coleta
    }

    void TentarComerMarmita()
    {
        if (objetoMarmita != null)
        {
            AnimacaoMarmita();
        }
        else
        {
            AtualizarMensagemComer("Não há marmita para comer. Encontre uma marmita primeiro.");
        }
    }

    bool VerificarProximidadeMarmita()
    {
        if (objetoMarmita != null)
        {
            float distanciaParaMarmita = Vector3.Distance(jogador.transform.position, objetoMarmita.transform.position);
            AtualizarMensagemColeta("Pressione 'E' para comer");
            return distanciaParaMarmita <= distanciaMaximaMarmita;
        }
        return false;
    }

    void AnimacaoMarmita()
    {
        StartCoroutine(MoverParaCamera(objetoMarmita));
    }

    IEnumerator MoverParaCamera(GameObject objeto)
    {
        Vector3 posicaoInicial = objeto.transform.position;
        Vector3 posicaoFinal = mainCamera.transform.position;

        float distancia = Vector3.Distance(posicaoInicial, posicaoFinal);
        float tempoTotal = distancia / velocidadeMovimentoMarmita;
        float tempoPassado = 0f;

        while (tempoPassado < tempoTotal)
        {
            tempoPassado += Time.deltaTime;
            float t = tempoPassado / tempoTotal;
            objeto.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, t);

            yield return null;
        }

        // Aguarda um pequeno intervalo antes de destruir o objeto
        yield return new WaitForSeconds(0.5f);

        // Destroi o objeto após a animação
        AtualizarMensagemColeta("");
        Destroy(objeto);
        ProgressoController.Instance.MarmitaApagada();
    }

    void AtualizarMensagem(string mensagem)
    {
        // Atualiza o texto da mensagem
        if (mensagemText != null)
        {
            mensagemText.text = mensagem;
        }
    }

        void AtualizarMensagemColeta(string mensagem)
    {
        // Atualiza o texto da mensagem de coleta
        if (mensagemText != null)
        {
            mensagemText.text = mensagem;
        }
    }

    void AtualizarMensagemComer(string mensagem)
    {
        // Atualiza o texto da mensagem para comer
        if (mensagemText != null)
        {
            mensagemText.text = mensagem;
        }
    }

    void DesativarMeshRenderer(GameObject objeto)
    {
        // Desativa o MeshRenderer do objeto
        MeshRenderer meshRenderer = objeto.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }
}