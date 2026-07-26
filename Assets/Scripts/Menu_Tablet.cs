using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using static UnityEditor.ShaderData;
#endif
public class Menu_Tablet : MonoBehaviour
{
    public GameObject Panel_Tablet;
    public GameObject Panel_Inicio;
    public GameObject Panel_social;
    public GameObject Panel_trabajo;
    public GameObject Panel_Misiones;
    public bool Activo = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Panel_Tablet.SetActive(false);
        Panel_Inicio.SetActive(false);
        Panel_social.SetActive(false);
        Panel_trabajo.SetActive(false);
        Panel_Misiones.SetActive(false);
    }


    void Update()
    {

        if (Keyboard.current.tKey.wasPressedThisFrame )
        {
            if (Activo == false)
            {
                Abrir_Tablet();
            }
        }
    }
    public void Abrir_Tablet()
    {
        Activo = true;
        Panel_Tablet.SetActive(true);
        Panel_Inicio.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
           }
    public void Cerrar_Tablet()
    {
        Activo = false;
        Panel_Tablet.SetActive(false);
        Panel_Inicio.SetActive(false);
        Panel_social.SetActive(false);
        Panel_trabajo.SetActive(false);
        Panel_Misiones.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       
    }
    public void Abrir_Inicip()
    {
        Panel_Inicio.SetActive(true);
        Panel_social.SetActive(false);
        Panel_trabajo.SetActive(false);
        Panel_Misiones.SetActive(false);
   
    }

    public void Abrir_Social()
    {
        Panel_Inicio.SetActive(false);
        Panel_social.SetActive(true);
        Panel_trabajo.SetActive(false);
        Panel_Misiones.SetActive(false);

    }
    public void Abrir_trabajo()
    {
        Panel_Inicio.SetActive(false);
        Panel_social.SetActive(false);
        Panel_trabajo.SetActive(true);
        Panel_Misiones.SetActive(false);

    }
    public void Abrir_Misiones()
    {
        Panel_Inicio.SetActive(false);
        Panel_social.SetActive(false);
        Panel_trabajo.SetActive(false);
        Panel_Misiones.SetActive(true);

    }
}
