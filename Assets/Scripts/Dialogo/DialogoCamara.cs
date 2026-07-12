using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class DialogueCameraControl : MonoBehaviour
{
    [Header("Cinemachine References")]
    public CinemachineCamera dialogueCamera;

    public CinemachineCamera JuegoCamera;
    public CinemachineTargetGroup targetGroup;

    [Header("Settings")]
    public float npcWeight = 1f; 
    public float npcRadius = 2f;
    private CinemachineInputAxisController inputCamara;
    public Movimiento_Personaje scriptTuguito;
    public GameObject Tuguito;
    public void ActivateDialogueCamera(Transform npcTransform)
    {
     
        targetGroup.Targets.Add(new CinemachineTargetGroup.Target
        {
            Object = npcTransform,
            Weight = npcWeight,
            Radius = npcRadius
        });
        Debug.Log(targetGroup);
        if (inputCamara != null)
        {
            inputCamara.enabled = false;
        }
        dialogueCamera.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (scriptTuguito != null)
        {
            scriptTuguito.enabled = false;
        }
        JuegoCamera.Priority = 0;
        dialogueCamera.Priority = 100;

        Debug.Log(dialogueCamera.Priority);
    }
    public void DeactivateDialogueCamera(Transform npcTransform)
    { 

        dialogueCamera.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (scriptTuguito != null && Tuguito != null)
        {
            Tuguito.SetActive(true);
            scriptTuguito.enabled = true;
        }

        if (inputCamara != null)
        {
            inputCamara.enabled = true;
        }
        dialogueCamera.Priority = 0;
        JuegoCamera.Priority = 100;
        for (int i = 0; i < targetGroup.Targets.Count; i++)
        {
            if (targetGroup.Targets[i].Object == npcTransform)
            {
                targetGroup.Targets.RemoveAt(i);
                break;
            }
        }
        Debug.Log(targetGroup);
        Debug.Log(dialogueCamera.Priority);
    }
}