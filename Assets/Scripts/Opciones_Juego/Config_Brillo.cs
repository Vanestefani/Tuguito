using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class Config_Brillo : MonoBehaviour
{
    public Slider sliderBrillo;
    public Volume volumenGlobal; // Arrastra aquí tu Global Volume
    private LiftGammaGain effect_LiftGammaGain;
    void Start()
    {
        if (volumenGlobal.profile.TryGet(out LiftGammaGain lgg))
        {
            effect_LiftGammaGain = lgg;
        }
        float brilloGuardado = PlayerPrefs.GetFloat("BrilloPostGuardado", 0f);
        sliderBrillo.value = brilloGuardado;
        ActualizarBrillo(brilloGuardado);
        sliderBrillo.onValueChanged.AddListener(ActualizarBrillo);
    }
    public void ActualizarBrillo(float valorSlider)
    {
        if (effect_LiftGammaGain != null)
        {
      
            Vector4 nuevoGamma = effect_LiftGammaGain.gamma.value;
            nuevoGamma.w = valorSlider;
            effect_LiftGammaGain.gamma.value = nuevoGamma;
        }

        PlayerPrefs.SetFloat("BrilloPostGuardado", valorSlider);
    }
}

