using System;
using UnityEngine;

public class ExperienciaManager : MonoBehaviour
{
    public static ExperienciaManager Instance { get; private set; }

    [SerializeField] private int experienciaActual = 0;

    public event Action<int> OnExperienciaGanada;
    public event Action<int, int> OnExperienciaCambiada; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int ObtenerExperienciaActual() => experienciaActual;

    public void GanarExperiencia(int cantidad)
    {
        if (cantidad <= 0) return;

        int anterior = experienciaActual;
        experienciaActual += cantidad;
        OnExperienciaGanada?.Invoke(cantidad);
        OnExperienciaCambiada?.Invoke(anterior, experienciaActual);
    }

    [Serializable]
    public class ExperienciaGuardada
    {
        public int experiencia;
    }

    public ExperienciaGuardada ExportarDatos() => new ExperienciaGuardada { experiencia = experienciaActual };

    public void ImportarDatos(ExperienciaGuardada datos)
    {
        if (datos == null) return;
        experienciaActual = Mathf.Max(0, datos.experiencia);
    }
}