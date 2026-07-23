using System;
using UnityEngine;

// Reputación GLOBAL del pueblo/tienda (ya no guarda nada por NPC;
// eso ahora vive en AfinidadManager). Sirve para gatear contenido de
// mundo: nuevas zonas, clientes especiales, progreso hacia "Empleado del Mes".
public class ReputacionManager : MonoBehaviour
{
    public static ReputacionManager Instance { get; private set; }

    [Header("Rango de reputación")]
    public int reputacionMinima = 0;
    public int reputacionMaxima = 1000;

    [SerializeField] private int reputacionActual = 0;

    // anterior, nuevo
    public event Action<int, int> OnReputacionCambiada;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int ObtenerReputacion() => reputacionActual;

    public int SumarReputacion(int cantidad)
    {
        int anterior = reputacionActual;
        reputacionActual = Mathf.Clamp(reputacionActual + cantidad, reputacionMinima, reputacionMaxima);
        OnReputacionCambiada?.Invoke(anterior, reputacionActual);
        return reputacionActual;
    }

    public void EstablecerReputacion(int valor)
    {
        int anterior = reputacionActual;
        reputacionActual = Mathf.Clamp(valor, reputacionMinima, reputacionMaxima);
        OnReputacionCambiada?.Invoke(anterior, reputacionActual);
    }
}