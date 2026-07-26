using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using static UnityEditor.ShaderData;
#endif
public class MenuColorFondo : MonoBehaviour
{
    public GameObject Panel_colores;
    private bool PanelActivo = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Panel_colores.SetActive(false);
        Cursor.visible = true;
    }

    public void Abrir_MenuColores()
    {
        if (!PanelActivo)
        {
            PanelActivo = true;
            Panel_colores.SetActive(true);
            Debug.Log("Panel de colores abierto");
        }
    }
    public void Cerrar_MenuColores()
    {
        if (PanelActivo)
        {
            PanelActivo = false;
            Panel_colores.SetActive(false);
            Debug.Log("Panel de colores cerrado");
        }
    }
    public void Alternar_MenuColores()
    {
        if (PanelActivo)
            Cerrar_MenuColores();
        else
            Abrir_MenuColores();
    }

    public void CambiarColorFondo(string hexColor)
    {
        if (!hexColor.StartsWith("#"))
        {
            hexColor = "#" + hexColor;
        }

        if (ColorUtility.TryParseHtmlString(hexColor, out Color nuevoColor))
        {
            if (Camera.main != null)
            {
                Camera.main.clearFlags = CameraClearFlags.SolidColor;
                Camera.main.backgroundColor = nuevoColor;
                Debug.Log($"Fondo cambiado a: {hexColor}");
            }
            else
            {
                Debug.LogWarning("Cámara principal no encontrada");
            }
        }
        else
        {
            Debug.LogError($"Color hexadecimal inválido: {hexColor}");
        }
    }
}
