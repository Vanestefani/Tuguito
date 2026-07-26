using UnityEngine;

public class InProgressState : IMissionState
{
    public string StateName => "InProgress";

    public void OnEnter(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} entered InProgressState");
    }

    public void OnExit(Mission mission)
    {
        Debug.Log($"[MISSION STATE] {mission.title} exited InProgressState");
    }

    public void TryCompleteObjective(Mission mission)
    {
        var currentObjective = mission.GetCurrentObjective();

        if (currentObjective == null)
        {
            Debug.LogWarning($"[MISSION STATE] No objective to complete in '{mission.title}'");
            return;
        }

        currentObjective.Complete();
        mission.InvokeObjectiveCompleted(currentObjective);

        Debug.Log($"[MISSION STATE] Objective completed: {currentObjective.description}");

        mission.AdvanceObjective();

        if (mission.GetCurrentObjective() == null)
        {
            mission.TransitionToState(new CompletedState());
        }
    }

    public void TryFail(Mission mission)
    {
        Debug.Log($"[MISSION STATE] Failing mission: {mission.title}");
        mission.TransitionToState(new FailedState());
    }

    public void Update(Mission mission)
    {

    }
}

