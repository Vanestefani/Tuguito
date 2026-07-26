using System.Collections;
using UnityEngine;

// Solo hace UNA cosa: cuando este panel se activa (SetActive(true)),
// aparece con una animación cozy (crece desde 0 con un pequeño rebote
// y se desvanece hacia adentro). No swap, no lógica de ocultar,
// no depende de otro panel. Actívalo/desactívalo como siempre
// (panel.SetActive(true)) y la animación pasa sola.
[RequireComponent(typeof(RectTransform))]
public class PanelAparecerCozy : MonoBehaviour
{
    [Header("Configuración")]
    public float duracion = 0.35f;
    [Tooltip("Si está activo, el panel entra con un pequeño giro que se endereza")]
    public bool usarGiro = false;
    [Range(5f, 30f)] public float anguloGiro = 15f;

    private RectTransform rect;
    private CanvasGroup grupo;
    private Vector3 escalaOriginal;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        grupo = GetComponent<CanvasGroup>();
        if (grupo == null) grupo = gameObject.AddComponent<CanvasGroup>();

        // Guarda la escala real que tiene el panel en el editor,
        // sea (1,1,1) o cualquier otra cosa, para no deformarlo.
        escalaOriginal = rect.localScale;
    }

    // Se dispara solo cada vez que el panel pasa de inactivo a activo
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Aparecer());
    }

    private IEnumerator Aparecer()
    {
        rect.localScale = Vector3.zero;
        grupo.alpha = 0f;
        float rotacionInicial = usarGiro ? -anguloGiro : 0f;

        float t = 0f;
        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            float progreso = Mathf.Clamp01(t / duracion);
            float progresoEasado = EaseOutBack(progreso);

            rect.localScale = Vector3.LerpUnclamped(Vector3.zero, escalaOriginal, progresoEasado);
            grupo.alpha = progreso;
            if (usarGiro)
                rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(rotacionInicial, 0f, progresoEasado));

            yield return null;
        }

        rect.localScale = escalaOriginal;
        rect.localRotation = Quaternion.identity;
        grupo.alpha = 1f;
    }

    private static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        t -= 1f;
        return 1f + c3 * t * t * t + c1 * t * t;
    }
}