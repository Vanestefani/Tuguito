using System.Collections;
using UnityEngine;
public class PanelSwapCozy : MonoBehaviour
{
    [Header("Paneles")]
    public RectTransform panelActual;
    public RectTransform panelNuevo;

    [Header("Configuración")]
    public float duracion = 0.35f;
    [Tooltip("Si está activo, el panel nuevo entra con un pequeño giro que se endereza")]
    public bool usarGiro = false;
    [Range(5f, 30f)] public float anguloGiro = 15f;

    private bool animando = false;

    public void CambiarAPanelNuevo()
    {
        if (animando) return;
        StartCoroutine(TransicionSwap());
    }

    private IEnumerator TransicionSwap()
    {
        animando = true;

        yield return AnimarPanel(panelActual, saliendo: true);
        panelActual.gameObject.SetActive(false);

        panelNuevo.gameObject.SetActive(true);
        panelNuevo.localScale = Vector3.zero;
        var grupoNuevo = panelNuevo.GetComponent<CanvasGroup>();
        if (grupoNuevo != null) grupoNuevo.alpha = 0f;

        yield return AnimarPanel(panelNuevo, saliendo: false);

        animando = false;
    }

    private IEnumerator AnimarPanel(RectTransform panel, bool saliendo)
    {
        CanvasGroup grupo = panel.GetComponent<CanvasGroup>();
        Vector3 escalaInicio = saliendo ? Vector3.one : Vector3.zero;
        Vector3 escalaFin = saliendo ? Vector3.zero : Vector3.one;
        float rotacionInicial = usarGiro && !saliendo ? -anguloGiro : 0f;

        float t = 0f;
        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            float progreso = Mathf.Clamp01(t / duracion);
            float progresoEasado = saliendo ? EaseInBack(progreso) : EaseOutBack(progreso);

            panel.localScale = Vector3.LerpUnclamped(escalaInicio, escalaFin, progresoEasado);
            if (grupo != null) grupo.alpha = saliendo ? 1f - progreso : progreso;
            if (usarGiro)
                panel.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(rotacionInicial, 0f, progresoEasado));

            yield return null;
        }

        panel.localScale = escalaFin;
        panel.localRotation = Quaternion.identity;
        if (grupo != null) grupo.alpha = saliendo ? 0f : 1f;
    }
    private static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        t -= 1f;
        return 1f + c3 * t * t * t + c1 * t * t;
    }

    private static float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return c3 * t * t * t - c1 * t * t;
    }
}
