using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ExperienciaUI : MonoBehaviour
{

    public Slider slider;
    public TMP_Text textoValor; 

    public int experienciaPorBarra = 100;

    public float velocidadAnimacion = 3f;

    private Coroutine animacionEnCurso;

    private void OnEnable()
    {
        if (ExperienciaManager.Instance != null)
            ExperienciaManager.Instance.OnExperienciaCambiada += ManejarCambioExperiencia;
    }

    private void OnDisable()
    {
        if (ExperienciaManager.Instance != null)
            ExperienciaManager.Instance.OnExperienciaCambiada -= ManejarCambioExperiencia;
    }

    private void Start()
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }

        if (ExperienciaManager.Instance != null)
        {
            int actual = ExperienciaManager.Instance.ObtenerExperienciaActual();
            if (slider != null) slider.value = CalcularFraccion(actual);
            ActualizarTexto(actual);
        }
    }

    private void ManejarCambioExperiencia(int anterior, int nuevo)
    {
        ActualizarTexto(nuevo);

        if (animacionEnCurso != null)
            StopCoroutine(animacionEnCurso);

        int barraAnterior = anterior / experienciaPorBarra;
        int barraNueva = nuevo / experienciaPorBarra;

        if (barraNueva > barraAnterior)
        {
 
            animacionEnCurso = StartCoroutine(AnimarConReinicio(nuevo));
        }
        else
        {
            animacionEnCurso = StartCoroutine(AnimarHacia(CalcularFraccion(nuevo)));
        }
    }

    private IEnumerator AnimarConReinicio(int experienciaFinal)
    {
        if (slider != null)
        {
     
            yield return AnimarHacia(1f);
            slider.value = 0f;
        }

        yield return AnimarHacia(CalcularFraccion(experienciaFinal));
    }

    private IEnumerator AnimarHacia(float objetivo)
    {
        if (slider == null) yield break;

        while (!Mathf.Approximately(slider.value, objetivo))
        {
            slider.value = Mathf.MoveTowards(slider.value, objetivo, velocidadAnimacion * Time.unscaledDeltaTime);
            yield return null;
        }
        slider.value = objetivo;
    }

    private float CalcularFraccion(int experienciaActual)
    {
        if (experienciaPorBarra <= 0) return 0f;
        int restante = experienciaActual % experienciaPorBarra;
        return (float)restante / experienciaPorBarra;
    }

    private void ActualizarTexto(int experienciaActual)
    {
        if (textoValor == null) return;
        int restante = experienciaPorBarra > 0 ? experienciaActual % experienciaPorBarra : experienciaActual;
        textoValor.text = $"{restante} / {experienciaPorBarra}";
    }
}