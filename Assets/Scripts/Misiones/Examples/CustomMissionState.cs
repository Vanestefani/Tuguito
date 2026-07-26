using UnityEngine;
namespace Missions.Examples
{

    public class CustomMissionState : IMissionState
    {

        private float customProperty = 0f;
        private bool someFlag = false;
        public string StateName => "CustomState";
        public CustomMissionState(float customValue = 0f)
        {
            customProperty = customValue;
        }

        public void OnEnter(Mission mission)
        {
            Debug.Log($"[CUSTOM STATE] Entering CustomState for '{mission.title}'");

        }

     
        public void OnExit(Mission mission)
        {
            Debug.Log($"[CUSTOM STATE] Exiting CustomState for '{mission.title}'");

        }

        public void TryCompleteObjective(Mission mission)
        {
            Debug.Log($"[CUSTOM STATE] Attempting to complete objective in '{mission.title}'");


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
            Debug.Log($"[CUSTOM STATE] Failing mission '{mission.title}'");
            mission.TransitionToState(new FailedState());
        }

        public void Update(Mission mission)
        {
         
        }

        public void DoSomething()
        {
            Debug.Log("[CUSTOM STATE] Doing something custom");
           
        }

        public string GetStateInfo()
        {
            return $"CustomState - Property: {customProperty}, Flag: {someFlag}";
        }
    }

    public class CounterState : IMissionState
    {
        private int completionCount = 0;
        private int maxCompletions = 3;

        public string StateName => "Counter";

        public CounterState(int max = 3)
        {
            maxCompletions = max;
        }

        public void OnEnter(Mission mission)
        {
            Debug.Log($"[COUNTER STATE] Starting counter for '{mission.title}' (max: {maxCompletions})");
            completionCount = 0;
        }

        public void OnExit(Mission mission)
        {
            Debug.Log($"[COUNTER STATE] Completed {completionCount}/{maxCompletions} times");
        }

        public void TryCompleteObjective(Mission mission)
        {
            completionCount++;

            if (completionCount >= maxCompletions)
            {
                Debug.Log($"[COUNTER STATE] Reached max completions!");
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
            else
            {
                Debug.Log($"[COUNTER STATE] {completionCount}/{maxCompletions}");
            }
        }

        public void TryFail(Mission mission)
        {
            mission.TransitionToState(new FailedState());
        }

        public void Update(Mission mission)
        {
        }

        public int GetCompletionCount()
        {
            return completionCount;
        }
    }

    public class ProgressiveState : IMissionState
    {
        private float progress = 0f;
        private float requiredProgress = 100f;

        public string StateName => "Progressive";

        public ProgressiveState(float required = 100f)
        {
            requiredProgress = required;
        }

        public void OnEnter(Mission mission)
        {
            Debug.Log($"[PROGRESSIVE STATE] Starting progressive mission for '{mission.title}' (required: {requiredProgress})");
            progress = 0f;
        }

        public void OnExit(Mission mission)
        {
        }

        public void TryCompleteObjective(Mission mission)
        {
                  progress += 25f;

            Debug.Log($"[PROGRESSIVE STATE] Progress: {progress}/{requiredProgress}");

            if (progress >= requiredProgress)
            {
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
        }

        public void TryFail(Mission mission)
        {
            mission.TransitionToState(new FailedState());

        }

        public void Update(Mission mission)
        {
        }

        public float GetProgress()
        {
            return progress / requiredProgress;
        }
    }
}