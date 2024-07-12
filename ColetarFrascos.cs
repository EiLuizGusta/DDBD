using UnityEngine;
using System.Collections;
using TMPro;

public class ColetorFrascos : MonoBehaviour
{
    public GameObject jogador;
    public GameObject[] objetosColetaveis;
    public TextMeshProUGUI contadorFrascosText;
    public Canvas canvas;
    public float velocidadeSubida = 5f;
    public float distanciaMinimaCamera = 2f;
    public float velocidadeDescida = 2f;

    public Transform mainCameraTransform; // Adiciona esta variável pública

    private bool[] frascosColetados;
    private int frascosRestantes;
    public TextMeshProUGUI mensagemColetaText; // Adiciona esta variável pública para a mensagem de coleta

    void Start()
    {
        frascosRestantes = objetosColetaveis.Length;
        frascosColetados = new bool[frascosRestantes];
        canvas.enabled = true;
        mensagemColetaText.text = "";
    }

    void Update()
    {
        for (int i = 0; i < objetosColetaveis.Length; i++)
        {
            if (!frascosColetados[i]) // Verifica se o frasco ainda não foi coletado
            {
                Collider jogadorCollider = jogador.GetComponent<Collider>();
                Collider objetoCollider = objetosColetaveis[i].GetComponent<Collider>();

                if (jogadorCollider.bounds.Intersects(objetoCollider.bounds))
                {
                    mensagemColetaText.text = "Aperte 'E' para coletar o frasco";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ColetarObjeto(i);
                    }
                }
                else
                {
                    mensagemColetaText.text = "";
                }
            }
        }
    }

    void ColetarObjeto(int index)
    {
        GameObject objetoColetado = objetosColetaveis[index];

        StartCoroutine(AnimacaoColeta(objetoColetado));
        frascosColetados[index] = true;
        frascosRestantes--;

        contadorFrascosText.text = $"Frascos Restantes: {frascosRestantes}";

        if (frascosRestantes == 0)
        {
            canvas.enabled = false;
            StartCoroutine(NotificarProgressoController());
        }
    }

    IEnumerator AnimacaoColeta(GameObject objeto)
    {
        // Salva a posição inicial e final
        Vector3 posicaoInicial = objeto.transform.position;
        Vector3 posicaoFinal = mainCameraTransform.position + mainCameraTransform.forward * distanciaMinimaCamera;

        float tempo = 0f;

        while (tempo < 1f)
        {
            if (objeto == null) yield break; // Verifica se o objeto foi destruído durante a animação
            tempo += Time.deltaTime * velocidadeSubida;
            
            // Calcula uma posição intermediária usando uma curva de Bezier para suavizar o movimento
            float t = Mathf.SmoothStep(0f, 1f, tempo);
            Vector3 posicaoIntermediaria = Vector3.Lerp(posicaoInicial, posicaoFinal, t) + new Vector3(0, Mathf.Sin(t * Mathf.PI) * 0.5f, 0);

            // Atualiza a posição do objeto
            objeto.transform.position = posicaoIntermediaria;

            yield return null;
        }

        // Aguarda um pequeno intervalo antes de destruir o objeto
        yield return new WaitForSeconds(0.5f);

        // Destroi o objeto após a animação
        Destroy(objeto);
    }

    IEnumerator NotificarProgressoController()
    {
        yield return new WaitForSeconds(1f);
        ProgressoController.Instance.CompleteColetaFrascos();
    }
}