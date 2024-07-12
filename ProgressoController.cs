using UnityEngine;

public class ProgressoController : MonoBehaviour
{
    private static ProgressoController _instance;
    public static ProgressoController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ProgressoController>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ProgressoController");
                    _instance = go.AddComponent<ProgressoController>();
                }
            }
            return _instance;
        }
    }

    private int progress = 0;
    private int objetosColetados = 0; // Novo contador para objetos coletados

    // Referência para o objeto que contém o ColetorFrascos
    public GameObject objetoColetorFrascos;
    public GameObject objetoVazioTrocaObjeto; // Objeto vazio com o TrocaObjeto
    public GameObject objetoProgresso4; // Novo objeto para Progresso == 4
    public GameObject Sleep;
    public GameObject list;

    public int Progress
    {
        get { return progress; }
        set
        {
            Debug.Log($"Valor do progresso alterado: {progress} -> {value}");
            progress = value;

            // Verifica se o progresso está entre 1 e 2 para ativar ou desativar o objetoColetorFrascos
            if (progress == 1)
            {
                // Ativa o objetoColetorFrascos
                objetoColetorFrascos.SetActive(true);
            }
            else if (progress == 2)
            {
                // Desativa o objetoColetorFrascos
                objetoColetorFrascos.SetActive(false);
                objetoVazioTrocaObjeto.SetActive(true);
            }
            else if (progress == 4)
            {
                // Desativa o objetoVazioTrocaObjeto quando o progresso atinge 3
                objetoVazioTrocaObjeto.SetActive(false);
                objetoProgresso4.SetActive(true);
                list.SetActive(false);
            }
            else if (progress == 5)
            {
                objetoProgresso4.SetActive(false);
                Sleep.SetActive(true);
                Debug.Log("Progresso atingiu 4!");
            }
        }
    }

    public int ObjetosColetados
    {
        get { return objetosColetados; }
        set
        {
            Debug.Log($"Quantidade de objetos coletados alterada: {objetosColetados} -> {value}");
            objetosColetados = value;
        }
    }

    public void IncreaseProgress(int amount)
    {
        Debug.Log($"Aumentando o progresso em {amount}. Valor atual: {progress} -> {progress + amount}");
        progress += amount;
    }

    // Função para aumentar a quantidade de objetos coletados
    public void IncreaseObjetosColetados(int amount)
    {
        Debug.Log($"Aumentando a quantidade de objetos coletados em {amount}. Valor atual: {objetosColetados} -> {objetosColetados + amount}");
        objetosColetados += amount;
    }

    public void CompleteColetaFrascos()
    {
        Debug.Log("Todos os frascos foram coletados! Progresso alterado para 2.");
        Progress = 2;
    }

    // Função para ser chamada quando todos os objetos de troca foram concluídos
    public void CompleteObjetosTroca()
    {
        Debug.Log("Todos os objetos de troca foram concluídos! Progresso alterado para 3.");
        Progress = 4;
    }

    // Função para ser chamada quando a marmita for apagada
    public void MarmitaApagada()
    {
        // Verifica se o progresso é 3 e, em seguida, incrementa para 4
        if (progress == 4)
        {
            Progress = 5;
        }
    }
}