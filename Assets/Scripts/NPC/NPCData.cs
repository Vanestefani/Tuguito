using System.Collections.Generic;
using UnityEngine;
public enum RolNPC { Protagonista, Antagonista, Jefe, JefeBodega, Logistica, Cajero }

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Objects/NPCData")]

public class NPCData : ScriptableObject
{
    [Header("Debe coincidir con el npcId del NPCInteractable en escena")]
    public string npcId;
    [Header("Narrativa")]
    public RolNPC rol;
    [TextArea] public string arquetipo;
    [TextArea] public string descripcionPersonalidad;
    public bool esAliado;


}
