using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPC_MirarJugador_IK : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    [Tooltip("Ajusta esto para que mire a los ojos del jugador y no al cielo o a los pies.")]
    public float targetOffsetY = 0.5f;

    [Header("Rotación del Cuerpo")]
    public bool rotarCuerpo = true; 
    public float velocidadGiroCuerpo = 3f;

    [Header("Configuración IK (Cabeza)")]
    [Range(0f, 1f)] public float pesoGeneral = 1f;
    [Range(0f, 1f)] public float pesoCabeza = 0.8f;
    [Range(0f, 1f)] public float pesoCuerpo = 0.3f;
    [Range(0f, 1f)] public float limiteRotacion = 0.7f;
    public float smoothSpeed = 5f;

    [Header("Proximidad")]
    public float activationRange = 8f;

    private Animator _animator;
    private float _currentWeight = 0f;
    private bool _isLooking = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

       
        float distance = Vector3.Distance(transform.position, player.position);
        _isLooking = distance <= activationRange;

        if (_isLooking && rotarCuerpo)
        {
            GirarCuerpoHaciaJugador();
        }
    }

    void GirarCuerpoHaciaJugador()
    {
    
        Vector3 direccionCuerpo = player.position - transform.position;
        direccionCuerpo.y = 0f;

        if (direccionCuerpo.sqrMagnitude > 0.001f) 
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionCuerpo);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadGiroCuerpo);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (player == null) return;

        float targetWeight = _isLooking ? pesoGeneral : 0f;
        _currentWeight = Mathf.Lerp(_currentWeight, targetWeight, Time.deltaTime * smoothSpeed);

        if (_currentWeight > 0.01f)
        {
            Vector3 targetPos = player.position + (Vector3.up * targetOffsetY);

            _animator.SetLookAtWeight(_currentWeight, pesoCuerpo, pesoCabeza, 1f, limiteRotacion);
            _animator.SetLookAtPosition(targetPos);
        }
    }
}