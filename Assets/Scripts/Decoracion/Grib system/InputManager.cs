using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;
    private Vector3 lastMousePos = Vector3.zero;

    [SerializeField]
    private LayerMask placementLayermask;
    public event Action OnClicked, OnExit;
    public event Action OnRotateRight, OnRotateLeft;
    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            OnClicked?.Invoke();
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            OnExit?.Invoke();
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            {
                OnRotateLeft?.Invoke();
            }
            else
            {
                OnRotateRight?.Invoke();
            }
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    private void Start()
    {
        lastPosition = Vector3.zero;

        if (sceneCamera == null)
        {
            sceneCamera = Camera.main;   
        }      
    }

    public Vector3 GetSelectedMapPosition()
    {
        if (Mouse.current == null || sceneCamera == null)
        {
            Debug.LogWarning("InputManager: Mouse o Camera no disponibles");
            return lastPosition;
        }


        Vector3 mousePos = Mouse.current.position.ReadValue();
        lastMousePos = mousePos;    
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, placementLayermask))
        {
            lastPosition = hit.point;

        }
        else
        {
          // Debug.LogWarning("InputManager: Raycast no golpeó nada en la capa de placement");
        }

        return lastPosition;
    
    }
}