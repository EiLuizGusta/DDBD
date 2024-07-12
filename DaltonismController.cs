using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class DaltonismController : MonoBehaviour
{
    public static DaltonismController Instance { get; private set; }

    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    public Button neutralButton;
    public Button daltonismType1Button;
    public Button daltonismType2Button;
    public Button daltonismType3Button;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (postProcessVolume == null)
        {
            Debug.LogError("Post Process Volume is not assigned. Please assign it in the Inspector.");
            return;
        }

        if (!postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            colorGrading = postProcessVolume.profile.AddSettings<ColorGrading>();
        }

        // Configurar as configurações iniciais aqui (por exemplo, filtro neutro)
        SetFilter(1.0f, 1.0f, 1.0f);

        // Associar funções aos botões
        neutralButton.onClick.AddListener(SetNeutralFilter);
        daltonismType1Button.onClick.AddListener(SetDaltonismType1Filter);
        daltonismType2Button.onClick.AddListener(SetDaltonismType2Filter);
        daltonismType3Button.onClick.AddListener(SetDaltonismType3Filter);
    }

    public void SetNeutralFilter()
    {
        SetFilter(1.0f, 1.0f, 1.0f);
    }

    public void SetDaltonismType1Filter()
    {
        SetFilter(1.0f, 0.5f, 0.5f);
    }

    public void SetDaltonismType2Filter()
    {
        SetFilter(0.5f, 1.0f, 0.5f);
    }

    public void SetDaltonismType3Filter()
    {
        SetFilter(0.5f, 0.5f, 1.0f);
    }

    void SetFilter(float redValue, float greenValue, float blueValue)
    {
        colorGrading.postExposure.value = 0.0f;
        colorGrading.colorFilter.value = new Color(redValue, greenValue, blueValue, 1.0f);
        colorGrading.temperature.value = 0.0f;

        Debug.Log("Filter set to: R=" + redValue + ", G=" + greenValue + ", B=" + blueValue);
    }
}