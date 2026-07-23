using System;
using System.Collections.Generic;
using UnityEngine;

public class AfinidadManager : MonoBehaviour
{
    public static AfinidadManager Instance { get; private set; }
    public int afinidadMinima = -100;
    public int afinidadMaxima = 100;
        private readonly Dictionary<string, int> afinidades = new Dictionary<string, int>();
    private readonly Dictionary<string, HashSet<string>> opcionesUsadas = new Dictionary<string, HashSet<string>>();
    public event Action<string, int, int> OnAfinidadCambiada;
     private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int ObtenerAfinidad(string npcId)
    {
        return afinidades.TryGetValue(npcId, out int valor) ? valor : 0;
    }

    public int SumarAfinidad(string npcId, int cantidad)
    {
        int actual = ObtenerAfinidad(npcId);
        int nuevo = Mathf.Clamp(actual + cantidad, afinidadMinima, afinidadMaxima);
        afinidades[npcId] = nuevo;
        OnAfinidadCambiada?.Invoke(npcId, actual, nuevo);
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

    // ---------- Guardado / Carga (JSON con JsonUtility) ----------
    [Serializable]
    public class AfinidadGuardada
    {
        public List<string> npcIds = new List<string>();
        public List<int> valores = new List<int>();
    }

    public AfinidadGuardada ExportarDatos()
    {
        var datos = new AfinidadGuardada();
        foreach (var kvp in afinidades)
        {
            datos.npcIds.Add(kvp.Key);
            datos.valores.Add(kvp.Value);
        }
        return datos;
    }

    public void ImportarDatos(AfinidadGuardada datos)
    {
        afinidades.Clear();
        if (datos == null) return;
        for (int i = 0; i < datos.npcIds.Count; i++)
            afinidades[datos.npcIds[i]] = datos.valores[i];
    }
}
