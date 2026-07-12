using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteracionJugador : MonoBehaviour
{
  
    private IInteractable interactableAnterior;
    private void Update()
    {
     
        var teclado = Keyboard.current;
        var raton = Mouse.current;
        if (teclado == null || raton == null ) return;
        IInteractable interactable = GetInteractableObject();
        if (interactable != interactableAnterior)
        {
            interactableAnterior?.MostrarIndicador(false);
            interactable?.MostrarIndicador(true);
            interactableAnterior = interactable;
        }
        if (teclado.eKey.wasPressedThisFrame || raton.rightButton.wasPressedThisFrame)
        {

           
            if (interactable != null)
            {
                interactable?.Interaccion(transform);
            }
          
        }


    }
    public IInteractable GetInteractableObject()
    {
        if (!this.enabled) return null;
        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 1f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetInteractableTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetInteractableTransform().position))
                {

                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
}