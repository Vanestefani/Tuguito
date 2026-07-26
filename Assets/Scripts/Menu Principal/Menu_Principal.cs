using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu_Principal :MonoBehaviour
{
    public GameObject panelOpciones;
    void Start()
    {
        panelOpciones.SetActive(false);
        Cursor.visible = true;
    }
    public void Btn_Play() {

        SceneManager.LoadScene("01-inicio");
        Debug.Log("Presionastes Jugar...");
    }
    public void Btn_Quit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

    }
}
