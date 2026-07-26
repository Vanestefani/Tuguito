using UnityEngine;

public class Letreros_Interaccion : MonoBehaviour, IInteractable
{

    public GameObject indicadorInteraccion;
    [SerializeField] private Sprite imagenLetrero;
    [SerializeField] private string tituloLetrero;
    private void Awake()
    {
       
        if (indicadorInteraccion != null)
            indicadorInteraccion.SetActive(false);
    }
    public void Interaccion(UnityEngine.Transform interactorTransform)
    {
        Debug.Log("Letrero: Interaccion() fue llamada");
        if (Visorletreroui.Instancia == null)
        {
            Debug.LogWarning($"No se encontro VisorLetreroUI en la escena. No se puede mostrar el letrero '{name}'.");
            return;
        }
        Visorletreroui.Instancia.Mostrar(imagenLetrero, tituloLetrero);
    }

    public void TerminarInteraccion(UnityEngine.Transform interactorTransform)
    {
        if (Visorletreroui.Instancia != null)
            Visorletreroui.Instancia.Cerrar();
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
