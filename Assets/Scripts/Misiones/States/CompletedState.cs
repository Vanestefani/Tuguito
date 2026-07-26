using UnityEngine;

public class CompletedState : IMissionState
{
    public string StateName => "Completed";
    public void OnEnter(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} entered CompletedState");
        mission.InvokeMissionCompleted();
    }
    public void OnExit(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} exited CompletedState");
    }
    public void TryCompleteObjective(Mission mission)
    {
        Debug.LogWarning($"[MISSION STATE] Cannot complete objective: mission '{mission.title}' already completed");
    }
    public void TryFail(Mission mission)
    {
        Debug.LogWarning($"[MISSION STATE] Cannot fail: mission '{mission.title}' already completed");
    }
    public void Update(Mission mission)
    {
        
    }
}

