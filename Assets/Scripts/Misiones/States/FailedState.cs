using UnityEngine;

public class FailedState : IMissionState
{
    public string StateName => "Failed";

    public void OnEnter(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} entered FailedState");

        mission.InvokeMissionFailed();
    }

    public void OnExit(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} exited FailedState");
    }

    public void TryCompleteObjective(Mission mission)
    {
        Debug.LogWarning($"[MISSION STATE] Cannot complete objective: mission '{mission.title}' failed");
    }

    public void TryFail(Mission mission)
    {
        Debug.LogWarning($"[MISSION STATE] Mission '{mission.title}' already failed");
    }

    public void Update(Mission mission)
    {

    }
}

