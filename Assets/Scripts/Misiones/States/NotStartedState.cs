using UnityEngine;

public class NotStartedState :IMissionState
{
    public string StateName => "NotStarted";

    public void OnEnter(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} entered NotStartedState");
    }

    public void OnExit(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} exited NotStartedState");
    }

    public void TryCompleteObjective(Mission mission)
    {
        Debug.LogWarning($"[MISSION STATE] Cannot complete objective: mission '{mission.title}' not started");
    }

    public void TryFail(Mission mission)
    {
        Debug.LogWarning($"[MISSION STATE] Cannot fail: mission '{mission.title}' not started");
    }

    public void Update(Mission mission)
    {
        // Sin lógica por frame en este estado
    }
}  
   
