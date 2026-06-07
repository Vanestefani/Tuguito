using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu_Principal :MonoBehaviour
{
    public GameObject panelOpciones;
    void Start()
    {
        // Esta es la línea mágica que apaga el panel al arrancar
        panelOpciones.SetActive(false);
    }
    public void Btn_Play() {

        SceneManager.LoadScene("01-Demo");
        Debug.Log("Presionastes Jugar...");
    }
    public void Btn_Quit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

    }
}
