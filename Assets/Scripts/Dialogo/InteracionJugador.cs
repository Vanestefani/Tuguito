using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InteracionJugador : MonoBehaviour
{
   
    private void Update()
    {
     
        var teclado = Keyboard.current;
        var raton = Mouse.current;
        if (teclado == null || raton == null ) return;
        if (teclado.eKey.wasPressedThisFrame || raton.rightButton.wasPressedThisFrame)
        {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null)
            {
                interactable.Interaccion(transform);
                interactable.TerminarInteraccion(transform);
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