using TMPro;
using UnityEngine;

public class AfinidadItemUI : MonoBehaviour
{
    [Header("NPC al que corresponde esta fila")]
    public NPCData npcData;

    [Header("Texto donde se muestra la afinidad")]
    public TMP_Text textoAfinidad;

    private void OnEnable()
    {
        if (AfinidadManager.Instance != null)
            AfinidadManager.Instance.OnAfinidadCambiada += AlCambiarAfinidad;

        Refrescar();
    }

    private void OnDisable()
    {
        if (AfinidadManager.Instance != null)
            AfinidadManager.Instance.OnAfinidadCambiada -= AlCambiarAfinidad;
    }

    private void AlCambiarAfinidad(string id, int anterior, int nueva)
    {
        if (npcData != null && id == npcData.npcId)
            Refrescar();
    }

    public void Refrescar()
    {
        if (textoAfinidad == null || npcData == null) return;

        int valor = 0;
        int maximo = 100;

        if (AfinidadManager.Instance != null)
        {
            valor = AfinidadManager.Instance.ObtenerAfinidad(npcData.npcId);
            maximo = AfinidadManager.Instance.afinidadMaxima;
        }

        textoAfinidad.text = $"{valor}/{maximo}";
    }
}