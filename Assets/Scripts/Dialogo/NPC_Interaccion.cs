using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;

    public float tiempoEsperaTerminar = 3f;
    public string npcId;
    public string nombreVisible;
    public DialogoData dialogoData;
    public RetratoEmocion[] retratos;
    public GameObject indicadorInteraccion;
    private bool enDialogo = false;
    public DialogueCameraControl camaraDialogo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (indicadorInteraccion != null)
            indicadorInteraccion.SetActive(false);
    }

    public void Interaccion(UnityEngine.Transform interactorTransform)
    {
        if (enDialogo) return;
        if (dialogoData == null)
        {
            Debug.LogWarning($"{name}: no tiene DialogoData asignado.");
            return;
        }
        enDialogo = true;
        MostrarIndicador(false);

        if (camaraDialogo != null)
        {
            camaraDialogo.ActivateDialogueCamera(this.transform);
        }
        animator.SetBool("Hablar", true);
        DialogueManager.Instance.IniciarDialogo(this);
    }

    public void TerminarInteraccion(UnityEngine.Transform interactorTransform)
    {
        enDialogo = false;
        animator.SetBool("Hablar", false);
        if (camaraDialogo != null)
        {
            camaraDialogo.DeactivateDialogueCamera(this.transform);
        }
    }

    public void MostrarIndicador(bool mostrar)
    {
        if (enDialogo) mostrar = false;
        if (indicadorInteraccion != null)
            indicadorInteraccion.SetActive(mostrar);
    }

    public UnityEngine.Transform GetInteractableTransform()
    {
        return transform;
    }

    public bool EstaEnDialogo() => enDialogo;

    public Sprite ObtenerRetrato(NPCEmocion emocion)
    {
        foreach (var retrato in retratos)
        {
            if (retrato.emocion == emocion)
                return retrato.sprite;
        }
        return retratos.Length > 0 ? retratos[0].sprite : null;
    }

    // ---- Afinidad individual con este NPC (antes vivía en ReputacionManager) ----
    public int ObtenerAfinidad()
    {
        return AfinidadManager.Instance != null ? AfinidadManager.Instance.ObtenerAfinidad(npcId) : 0;
    }

    public void SumarAfinidad(int cantidad)
    {
        if (AfinidadManager.Instance != null)
            AfinidadManager.Instance.SumarAfinidad(npcId, cantidad);
    }

    public bool OpcionYaUsada(string idOpcion)
    {
        return AfinidadManager.Instance != null && AfinidadManager.Instance.OpcionYaUsada(npcId, idOpcion);
    }

    public void MarcarOpcionUsada(string idOpcion)
    {
        AfinidadManager.Instance?.MarcarOpcionUsada(npcId, idOpcion);
    }
}

[System.Serializable]
public struct RetratoEmocion
{
    public NPCEmocion emocion;
    public Sprite sprite;
}