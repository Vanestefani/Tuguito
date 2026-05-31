using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Movimiento_Personaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 6f;
    public float velocidadRotacion = 15f;

    [Header("Físicas de Gravedad")]
    public float gravedad = 15f;
    public float limiteCaidaVacio = -10f;

    private Animator animator;
    private CharacterController controller;
    private Vector3 velocidadVertical;
    private Transform camaraTransform; // Guardará la orientación de tu cámara Cinemachine

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // Buscamos la cámara principal en la escena
        if (Camera.main != null)
        {
            camaraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Control de Caída al Vacío
        if (transform.position.y < limiteCaidaVacio)
        {
            ReiniciarEscena();
            return;
        }

        var teclado = Keyboard.current;
        if (teclado == null) return;

        // Capturar Inputs (W, S, A, D)
        float moverZ = 0f;
        if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) moverZ = 1f;
        else if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) moverZ = -1f;

        float moverX = 0f;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) moverX = 1f;
        else if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) moverX = -1f;

        // --- AQUÍ OCURRE LA MAGIA RELATIVA A LA CÁMARA ---
        Vector3 direccionMovimiento = Vector3.zero;

        if (camaraTransform != null)
        {
            // Conseguimos los vectores "Adelante" y "Derecha" de la cámara
            Vector3 camaraAdelante = camaraTransform.forward;
            Vector3 camaraDerecha = camaraTransform.right;

            // Ignoramos la inclinación vertical (Y) para que el personaje no intente volar o hundirse
            camaraAdelante.y = 0f;
            camaraDerecha.y = 0f;
            camaraAdelante.Normalize();
            camaraDerecha.Normalize();

            // Calculamos la dirección final multiplicando tus teclas por la orientación de la cámara
            direccionMovimiento = (camaraAdelante * moverZ + camaraDerecha * moverX).normalized;
        }
        else
        {
            // Copia de seguridad por si no encuentra la cámara
            direccionMovimiento = new Vector3(moverX, 0f, moverZ).normalized;
        }

        Vector3 movimientoFinal = direccionMovimiento * velocidad;

        // Rotación Cartoon Suave (Apunta hacia la dirección del movimiento final)
        if (direccionMovimiento.magnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
        }

        // Sistema de Gravedad
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
        else
        {
            velocidadVertical.y -= gravedad * Time.deltaTime;
        }

        movimientoFinal.y = velocidadVertical.y;

        // Aplicar Movimiento
        if (controller != null)
        {
            controller.Move(movimientoFinal * Time.deltaTime);
        }

        // Control de Animaciones
        if (animator != null)
        {
            bool estaCaminando = direccionMovimiento.magnitude > 0.1f;
            animator.SetBool("Caminar", estaCaminando);
        }
    }

    private void ReiniciarEscena()
    {
        Scene escenaActiva = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActiva.name);
    }
}