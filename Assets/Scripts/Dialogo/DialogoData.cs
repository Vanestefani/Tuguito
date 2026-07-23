using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "Dialogos/Arbol de Dialogo")]
public class DialogoData : ScriptableObject
{
  
    public string idNodoInicial = "inicio";

    public List<NodoDialogo> nodos = new List<NodoDialogo>();

    private Dictionary<string, NodoDialogo> _cache;

    public NodoDialogo ObtenerNodo(string id)
    {
        if (_cache == null)
        {
            _cache = new Dictionary<string, NodoDialogo>();
            foreach (var nodo in nodos)
            {
                if (!string.IsNullOrEmpty(nodo.id) && !_cache.ContainsKey(nodo.id))
                    _cache.Add(nodo.id, nodo);
            }
        }

        _cache.TryGetValue(id, out var resultado);
        return resultado;
    }
}

[Serializable]
public class NodoDialogo
{
   
    public string id;

    [TextArea(2, 5)]
    public string texto;

    public NPCEmocion emocion = NPCEmocion.Neutral;


    public bool esFinal = false;

    public List<OpcionDialogo> opciones = new List<OpcionDialogo>();
}

[Serializable]
public class OpcionDialogo
{
    [TextArea(1, 3)]
    public string texto;

    public int afinidadRequerida = 0;
    public int reputacionRequerida = 0;
    public int cambioAfinidad = 0;
    public int cambioreputacion = 0;
    public int experienciaGanada = 0;
    public int monedasGanadas = 0;
     public int reputacionGlobalRequerida = 0;
       public int cambioReputacionGlobal = 0;
        public int costoMonedas = 0;
    public string siguienteNodoId;
    public bool unaSolaVez = false;
    public string idOpcion;
}