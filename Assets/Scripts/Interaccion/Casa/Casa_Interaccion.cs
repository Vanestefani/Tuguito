using UnityEngine;
using UnityEngine.SceneManagement;
public class Casa_Interaccion : MonoBehaviour, IInteractable
{
    public GameObject indicadorInteraccion;
    private void Awake()
    {

        if (indicadorInteraccion != null)
            indicadorInteraccion.SetActive(false);
    }
    public void Interaccion(UnityEngine.Transform interactorTransform)
    {
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("02-Modo Construccion");
    }
    public void TerminarInteraccion(UnityEngine.Transform interactorTransform)
    {
        Debug.Log("Casa: Cerrar Interaccion() fue llamada");
    }
    public void MostrarIndicador(bool mostrar)
    {
        if (indicadorInteraccion != null)
            indicadorInteraccion.SetActive(mostrar);
    }
    public UnityEngine.Transform GetInteractableTransform()
    {
        return transform;
    }
}
