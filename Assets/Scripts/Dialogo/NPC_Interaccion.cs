using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
 

    public void Interaccion(UnityEngine.Transform interactorTransform)
    {
        Debug.Log("Hablar npc");
        animator.SetBool("Hablar", true);
    }

    public void TerminarInteraccion(UnityEngine.Transform interactorTransform)
    {
        Debug.Log("Terminó de hablar el npc");
       
        animator.SetBool("Hablar", false);
    }
    public UnityEngine.Transform GetInteractableTransform()
    {
        return transform;
    }
}
