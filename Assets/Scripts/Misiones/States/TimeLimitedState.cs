using UnityEngine;

public class TimeLimitedState : IMissionState
{
    private float timeRemaining;
    private float maxTime;
    private bool timeExpired = false;
    public string StateName => "TimeLimited";
    public TimeLimitedState(float maxTimeSeconds)
    {
        maxTime = maxTimeSeconds;
        timeRemaining = maxTimeSeconds;
    }

    public void OnEnter(Mission mission)
    {
        Debug.Log($"[TIMER STATE] {mission.title} started with {maxTime} seconds remaining");
        timeExpired = false;
    }

    public void OnExit(Mission mission)
    {
        Debug.Log($"[TIMER STATE] Timer state exited for {mission.title}");
    }
    public void TryCompleteObjective(Mission mission)
    {
        if (timeRemaining <= 0)
        {
            Debug.LogWarning($"[TIMER STATE] Time's up! Cannot complete objective for '{mission.title}'");
            if (!timeExpired)
            {
                mission.Fail();
            }
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
        Debug.Log($"[TIMER STATE] Failing mission: {mission.title}");
        mission.TransitionToState(new FailedState());
    }

    public void Update(Mission mission)
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0 && !timeExpired)
        {
            timeExpired = true;
            Debug.Log($"[TIMER STATE] Time expired for {mission.title}");
            mission.Fail();
        }
    }
    public float GetTimeRemaining()
    {
        return Mathf.Max(0, timeRemaining);
    }
    public float GetTimeProgress()
    {
        return 1f - (timeRemaining / maxTime);
    }
}
