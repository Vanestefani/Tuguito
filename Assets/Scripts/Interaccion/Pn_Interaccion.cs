using UnityEngine;

public class Pn_Interaccion : MonoBehaviour
{
    [SerializeField] private GameObject Panel_Interracion;
    [SerializeField] private InteracionJugador playerInteract;

    private bool wasInteractable;

    private void Start()
    {
        if (Panel_Interracion == null)
            Debug.LogError("Panel de interacción no asignado", this);
        if (playerInteract == null)
            Debug.LogError("Referencia a InteraccionJugador no asignada", this);
    }

    private void Update()
    {
        if (playerInteract == null) return;

        bool isInteractable = playerInteract.GetInteractableObject() != null;

        if (isInteractable != wasInteractable)
        {
            wasInteractable = isInteractable;
            Panel_Interracion?.SetActive(isInteractable);
        }
    }
}
