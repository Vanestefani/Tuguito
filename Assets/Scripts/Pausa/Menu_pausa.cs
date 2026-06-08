using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- 1. Añade esto aquí arriba

public class Menu_pausa : MonoBehaviour
{
    public GameObject Panel_pausa;
    public GameObject Panel_Salir;
    public GameObject Panel_Opciones;

    public bool Pausa = false;

    void Start()
    {
        Panel_pausa.SetActive(false);
        Panel_Salir.SetActive(false);
        Panel_Opciones.SetActive(false);
    }

    void Update()
    {
        // 2. Modificamos el Input para usar el Nuevo Input System
        if (Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Pausa == false)
            {
                Abrir_Pausa();
            }
        }
    }

    public void Abrir_Pausa()
    {
        Pausa = true;
        Panel_pausa.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Pausa menu pausa...");
    }

    public void Cerrar_Pausa()
    {
        Pausa = false;
        Panel_pausa.SetActive(false);
        Panel_Salir.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Cerrando menu pausa...");
    }

    public void Btn_Quit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void Btn_Menu()
    {
        SceneManager.LoadScene("0-Menu principal");
        Debug.Log("Presionastes menu...");
    }
}