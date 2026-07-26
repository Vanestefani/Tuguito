using UnityEngine;
using UnityEngine.UI;
public class BotonSelectorColor : MonoBehaviour
{
    private Image botonImage;
    void Awake()
    {
      
        botonImage = GetComponent<Image>();
    }
    public void SeleccionarEsteColor(MenuColorFondo scriptFondo)
    {
        if (scriptFondo == null)
        {
            Debug.LogError("Falta asignar el script de MenuColorFondo en el evento del botón.");
            return;
        }

        if (botonImage != null)
        {
            Color colorActual = botonImage.color;

            string hexColor = "#" + ColorUtility.ToHtmlStringRGB(colorActual);

            scriptFondo.CambiarColorFondo(hexColor);

        }
    }
}
