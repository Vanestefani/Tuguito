using Unity.Cinemachine;
using UnityEngine;

public class ControlTutorial : MonoBehaviour
{
    public GameObject panelTutorial;
    public CinemachineCamera camaraJugar;
    public Movimiento_Personaje scriptTuguito; // Script de Tuguito
    public GameObject Tuguito;
    // Esta función se llama desde el Signal del Timeline
    public void AbrirTutorial()
    {
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(true);
            // Esto hace que la cámara se vuelva la principal
            camaraJugar.Priority = 100;
        }
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
    }

}