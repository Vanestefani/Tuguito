using UnityEngine;
using UnityEngine.SceneManagement;

public class Deco_BtnVolver : MonoBehaviour
{
    public void Btn_Volver()
    {

        SceneManager.LoadScene("01-inicio");
        Debug.Log("Presionastes Jugar...");
    }
}
