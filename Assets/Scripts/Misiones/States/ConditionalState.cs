using UnityEngine;

public class ConditionalState : IMissionState
{
    private System.Func<bool> condition;
    private string conditionDescription;
    public string StateName => "Conditional";
    public ConditionalState(System.Func<bool> checkCondition, string description)
    {
        condition = checkCondition;
        conditionDescription = description;
    }

    public void OnEnter(Mission mission)
    {
        Debug.Log($"[CONDITIONAL STATE] {mission.title} requires: {conditionDescription}");
    }

    public void OnExit(Mission mission)
    {
        Debug.Log($"[CONDITIONAL STATE] Exiting conditional state for {mission.title}");
    }

    public void TryCompleteObjective(Mission mission)
    {
        if (condition != null && !condition())
        {
            Debug.LogWarning($"[CONDITIONAL STATE] Condition not met for '{mission.title}': {conditionDescription}");
            return;
        }

        var currentObjective = mission.GetCurrentObjective();
        if (currentObjective != null)
        {
            currentObjective.Complete();
            mission.InvokeObjectiveCompleted(currentObjective);
            mission.AdvanceObjective();

            if (mission.GetCurrentObjective() == null)
            {
                mission.TransitionToState(new CompletedState());
            }
        }
    }
    public void TryFail(Mission mission)
    {
        Debug.Log($"[CONDITIONAL STATE] Failing mission: {mission.title}");
        mission.TransitionToState(new FailedState());
    }
    public void Update(Mission mission)
    {
      
        if (condition != null && !condition())
        {
 
        }
    }
    public bool IsConditionMet()
    {
        return condition != null && condition();
    }
}
