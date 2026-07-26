using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Anima un botón con estilo "cozy cartoon": rebote al pasar el mouse,
// aplastamiento al hacer clic, y un bamboleo (wobble) suave mientras
// el cursor está encima. Usa Time.unscaledDeltaTime para funcionar
// aunque el menú esté con Time.timeScale = 0.
[RequireComponent(typeof(RectTransform))]
public class BotonAnimadoCozy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Escala")]
    public float escalaHover = 1.12f;
    public float escalaClic = 0.88f;

    [Header("Tiempos")]
    public float duracionHover = 0.25f;
    public float duracionClic = 0.12f;

    [Header("Bamboleo (wobble) mientras el mouse está encima")]
    public bool bambolearEnHover = true;
    [Range(1f, 15f)] public float anguloBamboleo = 5f;
    public float velocidadBamboleo = 3f;

    [Header("Sonido (opcional, deja vacío para usar el sonido por defecto)")]
    public AudioClip sonidoHoverPersonalizado;
    public AudioClip sonidoClicPersonalizado;

    private RectTransform rect;
    private Coroutine animacionActual;
    private Vector3 escalaBase;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        escalaBase = rect.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UISoundManager.Instance?.ReproducirHover(sonidoHoverPersonalizado);
        Reiniciar(RutinaHover());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Reiniciar(RutinaVolverANormal());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UISoundManager.Instance?.ReproducirClic(sonidoClicPersonalizado);
        Reiniciar(RutinaClic());
    }

    private void Reiniciar(IEnumerator nueva)
    {
        if (!gameObject.activeInHierarchy) return;

        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(nueva);
    }

    private IEnumerator RutinaHover()
    {
        yield return AnimarEscala(escalaBase * escalaHover, duracionHover, EaseOutBack);

        if (bambolearEnHover)
        {
            float t = 0f;
            while (true)
            {
                t += Time.unscaledDeltaTime * velocidadBamboleo;
                float angulo = Mathf.Sin(t) * anguloBamboleo;
                rect.localRotation = Quaternion.Euler(0f, 0f, angulo);
                yield return null;
            }
        }
    }

    private IEnumerator RutinaVolverANormal()
    {
        yield return AnimarRotacion(0f, duracionHover * 0.6f);
        yield return AnimarEscala(escalaBase, duracionHover, EaseOutBack);
    }

    private IEnumerator RutinaClic()
    {
        yield return AnimarEscala(escalaBase * escalaClic, duracionClic * 0.5f, EaseOutQuad);
        yield return AnimarEscala(escalaBase * escalaHover, duracionClic, EaseOutBack);
    }

    private IEnumerator AnimarEscala(Vector3 objetivo, float duracion, Func<float, float> easing)
    {
        Vector3 inicio = rect.localScale;
        float t = 0f;
        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            float progreso = easing(Mathf.Clamp01(t / duracion));
            rect.localScale = Vector3.LerpUnclamped(inicio, objetivo, progreso);
            yield return null;
        }
        rect.localScale = objetivo;
    }

    private IEnumerator AnimarRotacion(float anguloObjetivo, float duracion)
    {
        float inicio = rect.localRotation.eulerAngles.z;
        if (inicio > 180f) inicio -= 360f;
        float t = 0f;
        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            float progreso = Mathf.Clamp01(t / duracion);
            float angulo = Mathf.Lerp(inicio, anguloObjetivo, progreso);
            rect.localRotation = Quaternion.Euler(0f, 0f, angulo);
            yield return null;
        }
        rect.localRotation = Quaternion.identity;
    }

    // ---- Curvas de easing (sin dependencias externas) ----
    private static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        t -= 1f;
        return 1f + c3 * t * t * t + c1 * t * t;
    }

    private static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }
}