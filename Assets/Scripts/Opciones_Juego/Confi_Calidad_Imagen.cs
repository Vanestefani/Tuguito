using UnityEngine;
using TMPro;

public class Confi_Calidad_Imagen : MonoBehaviour
{
    public TMP_Dropdown dropdownCalidad;

    void Start()
    {
       int calidadGuardada = PlayerPrefs.GetInt("numeroDeCalidad", 3);
        dropdownCalidad.SetValueWithoutNotify(calidadGuardada);
                CambiarCalidad(calidadGuardada);
        dropdownCalidad.onValueChanged.AddListener(CambiarCalidad);
    }

    public void CambiarCalidad(int indiceCalidad)
    {
        QualitySettings.SetQualityLevel(indiceCalidad, true);
        PlayerPrefs.SetInt("numeroDeCalidad", indiceCalidad);
        PlayerPrefs.Save();
    }
}