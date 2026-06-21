using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI de Pantalla (Imagen Estática)")]
    [SerializeField] private GameObject panelInteractuarUI; // Arrastra aquí el objeto "InteractionPanel"

    private GameObject npcActual;

    // Detecta cuando el jugador entra al radio del NPC
    private void OnTriggerEnter(Collider other)
    {
        // Validamos que choque con el collider invisible (Trigger) del NPC
        if (other.CompareTag("NPC") && other.isTrigger)
        {
            npcActual = other.gameObject;

            // Encendemos la imagen en la pantalla
            if (panelInteractuarUI != null)
            {
                panelInteractuarUI.SetActive(true);
            }
        }
    }

    // Detecta cuando el jugador se aleja del NPC
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC") && other.isTrigger)
        {
            // Apagamos la imagen de la pantalla al alejarnos
            CerrarIndicadorUI();
        }
    }

    private void Update()
    {
        // Si hay un NPC cerca, escuchamos si el jugador presiona los botones
        if (npcActual != null)
        {
            // Clic derecho (1) o Tecla X
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
            {
                IniciarDialogo();
            }
        }
    }

    private void IniciarDialogo()
    {
        Debug.Log("¡Iniciando conversación con: " + npcActual.name + "!");

        // Ocultamos la imagen de interacción porque ya empezamos a hablar
        CerrarIndicadorUI();

        // [Aquí llamaremos al sistema de Cinemachine y al cuadro de diálogos más adelante]
    }

    private void CerrarIndicadorUI()
    {
        if (panelInteractuarUI != null)
        {
            panelInteractuarUI.SetActive(false);
        }
        npcActual = null;
    }
}