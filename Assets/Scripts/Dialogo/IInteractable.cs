using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    void Interaccion(Transform interactorTransform);
    void TerminarInteraccion(Transform interactorTransform);
    Transform GetInteractableTransform();
}