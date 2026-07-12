using System.Collections.Generic;
using UnityEngine;

public class ReputacionManager : MonoBehaviour
{
    public static ReputacionManager Instance { get; private set; }
    public int reputacionMinima = -100;
    public int reputacionMaxima = 100;
    private readonly Dictionary<string, int> reputaciones = new Dictionary<string, int>();
    private readonly Dictionary<string, HashSet<string>> opcionesUsadas = new Dictionary<string, HashSet<string>>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public int ObtenerReputacion(string npcId)
    {
        return reputaciones.TryGetValue(npcId, out int valor) ? valor : 0;
    }
    public int SumarReputacion(string npcId, int cantidad)
    {
        int actual = ObtenerReputacion(npcId);
        int nuevo = Mathf.Clamp(actual + cantidad, reputacionMinima, reputacionMaxima);
        reputaciones[npcId] = nuevo;
        return nuevo;
    }
    public bool OpcionYaUsada(string npcId, string idOpcion)
    {
        if (string.IsNullOrEmpty(idOpcion)) return false;
        return opcionesUsadas.TryGetValue(npcId, out var usadas) && usadas.Contains(idOpcion);
    }

    public void MarcarOpcionUsada(string npcId, string idOpcion)
    {
        if (string.IsNullOrEmpty(idOpcion)) return;

        if (!opcionesUsadas.TryGetValue(npcId, out var usadas))
        {
            usadas = new HashSet<string>();
            opcionesUsadas[npcId] = usadas;
        }
        usadas.Add(idOpcion);
    }
}
