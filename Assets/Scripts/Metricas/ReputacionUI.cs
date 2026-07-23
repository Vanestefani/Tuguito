using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ReputacionUI : MonoBehaviour
{
    [Header("Referencias")]
    public Slider slider;
    public TMP_Text textoValor; 

    [Header("Animación")]
    public float velocidadAnimacion = 3f;

    private float valorObjetivoNormalizado;
    private bool suscrito = false;

    private void OnEnable()
    {
        TrySuscribir();
    }

    private void OnDisable()
    {
        if (suscrito && ReputacionManager.Instance != null)
        {
            ReputacionManager.Instance.OnReputacionCambiada -= ManejarCambioReputacion;
            suscrito = false;
        }
    }

    private void Start()
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }

        TrySuscribir();

        if (ReputacionManager.Instance != null)
        {
            int actual = ReputacionManager.Instance.ObtenerReputacion();
            valorObjetivoNormalizado = Normalizar(actual);
            if (slider != null) slider.value = valorObjetivoNormalizado;
            ActualizarTexto(actual);
            Debug.Log($"[ReputacionUI] Start -> reputación inicial: {actual}, fraccion slider: {valorObjetivoNormalizado}");
        }
        else
        {
            Debug.LogWarning("[ReputacionUI] No se encontró ReputacionManager.Instance en Start.");
        }
    }

    private void TrySuscribir()
    {
        if (suscrito) return;
        if (ReputacionManager.Instance == null)
        {
            Debug.LogWarning("[ReputacionUI] ReputacionManager.Instance es null, no se pudo suscribir todavía.");
            return;
        }

        ReputacionManager.Instance.OnReputacionCambiada += ManejarCambioReputacion;
        suscrito = true;
        Debug.Log("[ReputacionUI] Suscrito correctamente a OnReputacionCambiada.");
    }

    private void Update()
    {
        if (slider == null) return;
        if (Mathf.Approximately(slider.value, valorObjetivoNormalizado)) return;

        slider.value = Mathf.MoveTowards(slider.value, valorObjetivoNormalizado, velocidadAnimacion * Time.unscaledDeltaTime);
    }

    private void ManejarCambioReputacion(int anterior, int nuevo)
    {
        Debug.Log($"[ReputacionUI] Evento recibido: {anterior} -> {nuevo} (fraccion objetivo: {Normalizar(nuevo)})");
        valorObjetivoNormalizado = Normalizar(nuevo);
        ActualizarTexto(nuevo);
    }

    private float Normalizar(int valor)
    {
        var m = ReputacionManager.Instance;
        if (m == null || m.reputacionMaxima <= m.reputacionMinima) return 0f;
        return Mathf.InverseLerp(m.reputacionMinima, m.reputacionMaxima, valor);
    }

    private void ActualizarTexto(int valorActual)
    {
        if (textoValor == null || ReputacionManager.Instance == null) return;
        textoValor.text = $"{valorActual} / {ReputacionManager.Instance.reputacionMaxima}";
    }
}