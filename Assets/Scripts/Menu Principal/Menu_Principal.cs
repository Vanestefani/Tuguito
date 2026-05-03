using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu_Principal :MonoBehaviour
{
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
