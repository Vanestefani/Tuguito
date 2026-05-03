using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento_Personaje : MonoBehaviour
{
    public float velocidad = 5f;
    public float rotacion = 200f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        float moverZ = 0f;
        if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed)        moverZ =  1f;
        else if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) moverZ = -1f;

        float rotarY = 0f;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed)        rotarY =  1f;
        else if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed)    rotarY = -1f;

    
        bool estaCaminando     = (moverZ > 0);
        bool estaRetrocediendo = (moverZ < 0);

        if (animator != null)
        {
            animator.SetBool("Caminar", estaCaminando);
          
        }

        transform.Translate(Vector3.forward * moverZ * velocidad * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up * rotarY * rotacion * Time.deltaTime, Space.Self); 
    }
}