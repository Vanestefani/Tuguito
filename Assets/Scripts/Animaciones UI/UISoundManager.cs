using UnityEngine;
using UnityEngine.Audio;

// Singleton simple que reproduce los sonidos de hover/clic de la UI.
// Ponlo en un GameObject persistente del menú (o en tu GameManager),
// así todos los botones comparten un solo AudioSource en vez de
// tener uno cada uno.
public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; }

    [Header("Clips por defecto")]
    public AudioClip sonidoHoverPorDefecto;
    public AudioClip sonidoClicPorDefecto;

    [Header("Audio")]
    [Range(0f, 1f)] public float volumen = 0.7f;
    public AudioMixerGroup grupoMixer; // opcional, déjalo vacío si no usas AudioMixer

    private AudioSource fuente;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.loop = false;
        if (grupoMixer != null) fuente.outputAudioMixerGroup = grupoMixer;
    }

    public void ReproducirHover(AudioClip clipPersonalizado = null)
    {
        AudioClip clip = clipPersonalizado != null ? clipPersonalizado : sonidoHoverPorDefecto;
        if (clip != null) fuente.PlayOneShot(clip, volumen);
    }

    public void ReproducirClic(AudioClip clipPersonalizado = null)
    {
        AudioClip clip = clipPersonalizado != null ? clipPersonalizado : sonidoClicPorDefecto;
        if (clip != null) fuente.PlayOneShot(clip, volumen);
    }
}
