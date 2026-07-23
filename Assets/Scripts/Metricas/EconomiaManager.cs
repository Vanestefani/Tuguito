using System;
using UnityEngine;

public class EconomiaManager : MonoBehaviour
{
    public static EconomiaManager Instance { get; private set; }

    [SerializeField] private int monedasActuales = 0;

    public event Action<int, int> OnMonedasCambiadas; 
    public event Action<int> OnCompraFallida;  

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int ObtenerMonedas() => monedasActuales;
    public bool PuedeComprar(int costo) => monedasActuales >= costo;

    public void GanarMonedas(int cantidad)
    {
        if (cantidad <= 0) return;
        int anterior = monedasActuales;
        monedasActuales += cantidad;
        OnMonedasCambiadas?.Invoke(anterior, monedasActuales);
    }

    public bool GastarMonedas(int cantidad)
    {
        if (cantidad <= 0) return true;

        if (monedasActuales < cantidad)
        {
            OnCompraFallida?.Invoke(cantidad - monedasActuales);
            return false;
        }

        int anterior = monedasActuales;
        monedasActuales -= cantidad;
        OnMonedasCambiadas?.Invoke(anterior, monedasActuales);
        return true;
    }

    [Serializable]
    public class EconomiaGuardada
    {
        public int monedas;
    }

    public EconomiaGuardada ExportarDatos() => new EconomiaGuardada { monedas = monedasActuales };

    public void ImportarDatos(EconomiaGuardada datos)
    {
        if (datos == null) return;
        monedasActuales = Mathf.Max(0, datos.monedas);
    }
}