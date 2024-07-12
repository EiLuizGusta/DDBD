using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public float fadeSpeed = 1.5f;
    public Graphic panelGraphic; // Variável pública para o painel
    public TextMeshProUGUI infoTextMesh; // Variável pública para o TextMeshProUGUI
    public float displayTime = 3f; // Tempo que o TextMeshProUGUI será exibido

    void Start()
    {
        if (panelGraphic == null)
        {
            Debug.LogError("Panel Graphic not assigned! Please assign it in the Inspector.");
            return;
        }

        StartCoroutine(FadeInRoutine());

        // Exibir texto por um tempo e limpar
        if (infoTextMesh != null)
        {
            infoTextMesh.text = "Vá ver o doutor, pegando o corredor à esquerda, primeira porta à esquerda!";
            StartCoroutine(DisplayTextAndClear());
        }
    }

    IEnumerator FadeInRoutine()
    {
        Color currentColor = panelGraphic.color;

        while (currentColor.a > 0f)
        {
            currentColor.a -= Time.deltaTime * fadeSpeed;
            SetAlpha(currentColor.a);
            yield return null;
        }

        // Desativar o painel após o fade in
        gameObject.SetActive(false);
    }

    IEnumerator DisplayTextAndClear()
    {
        yield return new WaitForSeconds(displayTime);

        // Exibir o TextMeshProUGUI por um tempo
        infoTextMesh.gameObject.SetActive(true);

        // Aguarde um pouco antes de limpar o TextMeshProUGUI
        yield return new WaitForSeconds(displayTime);

        // Limpar o TextMeshProUGUI
        infoTextMesh.text = "";

        // Desativar diretamente o componente TextMeshProUGUI
        infoTextMesh.gameObject.SetActive(false);
    }

    void SetAlpha(float alpha)
    {
        Color currentColor = panelGraphic.color;
        currentColor.a = alpha;
        panelGraphic.color = currentColor;
    }
}