using UnityEngine;
using UnityEngine.UI;

public class Config_Volumen : MonoBehaviour
{
    public Slider slider;
    public Image imagenMute;

    void Awake()
    {
        // Cargamos el volumen en el Awake para ganarle al inicio del panel
        float volumenGuardado = PlayerPrefs.GetFloat("volumenAudio", 0.5f);

        if (slider != null)
        {
            // El truco mágico: cambia el valor del slider SIN avisarle a nadie
            // Esto evita que se disparen eventos raros si el panel está oculto
            slider.SetValueWithoutNotify(volumenGuardado);
        }

        AudioListener.volume = volumenGuardado;
        RevisarSiEstoyMute(volumenGuardado);
    }

    void OnEnable()
    {
        // Cada vez que el panel se ENCIENDA, empezamos a escuchar al slider
        if (slider != null)
        {
            slider.onValueChanged.AddListener(ChangeSlider);
        }
    }

    void OnDisable()
    {
        // Cada vez que el panel se APAGUE, dejamos de escucharlo para evitar errores en segundo plano
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