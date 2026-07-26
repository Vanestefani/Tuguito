using TMPro;
#if UNITY_EDITOR
using UnityEditor.PackageManager;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Visorletreroui : MonoBehaviour
{
    public static Visorletreroui Instancia { get; private set; }
    [SerializeField] private GameObject panelVentana;
    [SerializeField] private Image imagenDisplay;
    [SerializeField] private TMP_Text textoTitulo;
    [SerializeField] private RectTransform contenedorImagen;
    [SerializeField] private RectTransform marcoVisible;
    [SerializeField] private Button botonCerrar;
    [SerializeField] private float zoomMinimo = 1f;
    [SerializeField] private float zoomMaximo = 3f;
    [SerializeField] private float velocidadZoom = 2f;
    [SerializeField] private InteracionJugador interaccionJugador;
    [SerializeField] private Behaviour[] componentesCamara;
    [SerializeField] private bool controlarCursor = true;
    private float zoomActual = 1f;
    private bool arrastrando;
    public bool EstaAbierto => panelVentana != null && panelVentana.activeSelf;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;

        if (panelVentana != null)
            panelVentana.SetActive(false);

        if (botonCerrar != null)
            botonCerrar.onClick.AddListener(Cerrar);
    }
    private void Update()
    {

        if (!EstaAbierto) return;
        if (Time.timeScale == 0f) return;
        if (panelVentana == null || !panelVentana.activeSelf) return;

        var teclado = Keyboard.current;
        var raton = Mouse.current;
        if (raton == null) return;
        float scroll = raton.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            zoomActual = Mathf.Clamp(zoomActual + Mathf.Sign(scroll) * velocidadZoom * Time.unscaledDeltaTime, zoomMinimo, zoomMaximo);
            AplicarZoom();
        }
        if (raton.leftButton.wasPressedThisFrame)
        {
            arrastrando = true;
        }
        if (raton.leftButton.wasReleasedThisFrame)
        {
            arrastrando = false;
        }
          if (arrastrando && zoomActual > zoomMinimo)
        {
            Vector2 delta = raton.delta.ReadValue();
            contenedorImagen.anchoredPosition += delta;
            LimitarArrastre();
        }
    }
    public void Mostrar(Sprite imagen, string titulo = "")
    {
        Debug.Log("VisorLetreroUI: Mostrar() fue llamado, panel = " + panelVentana);
        if (imagen == null)
        {
            Debug.LogWarning("Se intento mostrar un letrero sin imagen asignada.");
            return;
        }
    
        imagenDisplay.sprite = imagen;
      

        if (textoTitulo != null)
            textoTitulo.text = titulo;
        
        zoomActual = zoomMinimo;
        arrastrando = false;

        if (contenedorImagen != null)
        {
            contenedorImagen.anchoredPosition = Vector2.zero;
            AplicarZoom();
            
        }
       
        panelVentana.SetActive(true);


        if (controlarCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1;
          
        }
    }
 
    public void Cerrar()
    {
        if (panelVentana == null || !panelVentana.activeSelf) return;

        panelVentana.SetActive(false);
      


        if (controlarCursor && Time.timeScale != 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    private void AplicarZoom()
    {
        if (contenedorImagen != null)
            contenedorImagen.localScale = Vector3.one * zoomActual;
      
    }

    private void LimitarArrastre()
    {
     
        if (contenedorImagen == null) return;
        float limite = 200f * (zoomActual - 1f);
        Vector2 pos = contenedorImagen.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -limite, limite);
        pos.y = Mathf.Clamp(pos.y, -limite, limite);
        contenedorImagen.anchoredPosition = pos;
    }


}
