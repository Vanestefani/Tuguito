using Unity.Cinemachine;
using UnityEngine;

public class ControlTutorial : MonoBehaviour
{
    public GameObject panelTutorial;
    public CinemachineCamera camaraJugar;
    public Movimiento_Personaje scriptTuguito; // Script de Tuguito
    public GameObject Tuguito;

    // Guardaremos el componente que controla el mouse en Cinemachine v3
    private CinemachineInputAxisController inputCamara;

    private void Start()
    {
        // Al arrancar el juego, buscamos el componente de rotación dentro de tu cámara
        if (camaraJugar != null)
        {
            inputCamara = camaraJugar.GetComponent<CinemachineInputAxisController>();
        }
    }

    // Esta función se llama desde el Signal del Timeline
    public void AbrirTutorial()
    {
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(true);
            // Esto hace que la cámara se vuelva la principal
            camaraJugar.Priority = 100;
        }

        // 1. DESACTIVAR ROTACIÓN: Bloquea los ejes X e Y del mouse en Cinemachine
        if (inputCamara != null)
        {
            inputCamara.enabled = false;
        }

        // 2. DESACTIVAR MOVIMIENTO: Asegura que Tuguito no camine de fondo
        if (scriptTuguito != null)
        {
            scriptTuguito.enabled = false;
        }

        // 3. LIBERAR CURSOR: Muestra la flecha del mouse para poder pulsar el botón "Cerrar"
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Esta función se llama desde el botón "Cerrar" en el Inspector
    public void CerrarTutorial()
    {
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(false);
        }

        if (scriptTuguito != null && Tuguito != null)
        {
            Tuguito.SetActive(true);
            scriptTuguito.enabled = true;
        }

        // 1. ACTIVAR ROTACIÓN: La cámara vuelve a responder al movimiento del mouse
        if (inputCamara != null)
        {
            inputCamara.enabled = true;
        }

        // 2. OCULTAR CURSOR: Esconde la flecha en el centro de la pantalla para jugar cómodamente
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}