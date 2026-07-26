using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Hudmetricasui : MonoBehaviour
{
    [SerializeField] private TMP_Text txtMonedas;
    [SerializeField] private Slider sliderExperiencia;
    [SerializeField] private TMP_Text txtExperiencia;
    [SerializeField] private int experienciaMaximaActual = 100;
    [SerializeField] private Slider sliderReputacion;
    [SerializeField] private TMP_Text txtReputacion;
    private Coroutine rutinaEconomia;
    private Coroutine rutinaExperiencia;
    private Coroutine rutinaReputacion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        UnityEngine.Debug.Log("[HUD] OnEnable ejecutado en frame " + Time.frameCount);
        rutinaEconomia = StartCoroutine(EsperarEconomia());
        rutinaExperiencia = StartCoroutine(EsperarExperiencia());
        rutinaReputacion = StartCoroutine(EsperarReputacion());
    }

    private void OnDisable()
    {
        UnityEngine.Debug.Log("[HUD] OnDisable ejecutado en frame " + Time.frameCount);
        if (EconomiaManager.Instance != null)
            EconomiaManager.Instance.OnMonedasCambiadas -= ActualizarMonedas;
        if (ExperienciaManager.Instance != null)
            ExperienciaManager.Instance.OnExperienciaCambiada -= ActualizarExperiencia;
        if (ReputacionManager.Instance != null)
            ReputacionManager.Instance.OnReputacionCambiada -= ActualizarReputacion;

        if (rutinaEconomia != null) StopCoroutine(rutinaEconomia);
        if (rutinaExperiencia != null) StopCoroutine(rutinaExperiencia);
        if (rutinaReputacion != null) StopCoroutine(rutinaReputacion);
    }
    private IEnumerator EsperarEconomia()
    {
        yield return new WaitUntil(() => EconomiaManager.Instance != null);
        EconomiaManager.Instance.OnMonedasCambiadas += ActualizarMonedas;
        ActualizarMonedas(0, EconomiaManager.Instance.ObtenerMonedas());
    }

    private IEnumerator EsperarExperiencia()
    {
        yield return new WaitUntil(() => ExperienciaManager.Instance != null);
        ExperienciaManager.Instance.OnExperienciaCambiada += ActualizarExperiencia;
        ActualizarExperiencia(0, ExperienciaManager.Instance.ObtenerExperienciaActual());
    }

    private IEnumerator EsperarReputacion()
    {
        yield return new WaitUntil(() => ReputacionManager.Instance != null);
        Debug.Log("[HUD] Suscrito a ReputacionManager: " + ReputacionManager.Instance.GetInstanceID());
        ReputacionManager.Instance.OnReputacionCambiada += ActualizarReputacion;
        ActualizarReputacion(0, ReputacionManager.Instance.ObtenerReputacion());
    }

    private void OnDestroy()
    {
        if (EconomiaManager.Instance != null)
            EconomiaManager.Instance.OnMonedasCambiadas -= ActualizarMonedas;

        if (ExperienciaManager.Instance != null)
            ExperienciaManager.Instance.OnExperienciaCambiada -= ActualizarExperiencia;

        if (ReputacionManager.Instance != null)
            ReputacionManager.Instance.OnReputacionCambiada -= ActualizarReputacion;
    }
    private void ActualizarMonedas(int anterior, int nuevo)
    {
        if (txtMonedas != null)
            txtMonedas.text = nuevo.ToString();
    }

    private void ActualizarExperiencia(int anterior, int nuevo)
    {
        if (txtExperiencia != null)
            txtExperiencia.text = $"{nuevo}/{experienciaMaximaActual}";

        if (sliderExperiencia != null)
        {
            sliderExperiencia.maxValue = experienciaMaximaActual;
            sliderExperiencia.value = nuevo;
        }
    }
    private void ActualizarReputacion(int anterior, int nuevo)
    {
                Debug.Log($"[HUD] ActualizarReputacion llamado: {anterior} -> {nuevo}");
        int min = ReputacionManager.Instance != null ? ReputacionManager.Instance.reputacionMinima : 0;
        int max = ReputacionManager.Instance != null ? ReputacionManager.Instance.reputacionMaxima : 1000;

        if (txtReputacion != null)
            txtReputacion.text = $"{nuevo}/{max}";

        if (sliderReputacion != null)
        {
            sliderReputacion.minValue = min;
            sliderReputacion.maxValue = max;
            sliderReputacion.value = nuevo;
        }
    }

}
