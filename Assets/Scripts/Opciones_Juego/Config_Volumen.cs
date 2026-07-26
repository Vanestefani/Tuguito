using UnityEngine;
using UnityEngine.UI;

public class Config_Volumen : MonoBehaviour
{
    public Slider slider;
    public Image imagenMute;

    void Awake()
    {

        float volumenGuardado = PlayerPrefs.GetFloat("volumenAudio", 0.5f);

        if (slider != null)
        {

            slider.SetValueWithoutNotify(volumenGuardado);
        }

        AudioListener.volume = volumenGuardado;
        RevisarSiEstoyMute(volumenGuardado);
    }

    void OnEnable()
    {
        
        if (slider != null)
        {
            slider.onValueChanged.AddListener(ChangeSlider);
        }
    }

    void OnDisable()
    {
       
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(ChangeSlider);
        }
    }

    public void ChangeSlider(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("volumenAudio", valor);
        RevisarSiEstoyMute(valor);
    }

    private void RevisarSiEstoyMute(float valorActual)
    {
        if (imagenMute != null)
        {
            imagenMute.enabled = (valorActual <= 0.001f);
        }
    }
}