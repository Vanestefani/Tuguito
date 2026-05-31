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

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (transform.position.y < limiteCaidaVacio)
        {
            ReiniciarEscena();
            return;
        }

        var teclado = Keyboard.current;
        if (teclado == null) return;

      
        float moverZ = 0f; 
        if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) moverZ = 1f;
        else if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) moverZ = -1f;

        float moverX = 0f; 
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) moverX = 1f;
        else if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) moverX = -1f;

        Vector3 direccionMovimiento = new Vector3(moverX, 0f, moverZ).normalized;
        Vector3 movimientoFinal = direccionMovimiento * velocidad;

        if (direccionMovimiento.magnitude > 0.1f)
        {
       
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
        }

     
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
        else
        {
            velocidadVertical.y -= gravedad * Time.deltaTime; 
        }

        movimientoFinal.y = velocidadVertical.y;

        if (controller != null)
        {
            controller.Move(movimientoFinal * Time.deltaTime);
        }

     
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