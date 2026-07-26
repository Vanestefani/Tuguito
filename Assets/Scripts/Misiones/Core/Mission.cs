using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public string missionId;
    public string title;
    public string description;

    private IMissionState currentState;
    private List<MissionObjective> objectives = new List<MissionObjective>();
    private int currentObjectiveIndex = 0;
    public delegate void MissionStateChanged(Mission mission, string newState);
    public event MissionStateChanged OnStateChanged;
    public delegate void ObjectiveCompleted(Mission mission, MissionObjective objective);
    public event ObjectiveCompleted OnObjectiveCompleted;

    public delegate void MissionCompleted(Mission mission);
    public event MissionCompleted OnMissionCompleted;

    public delegate void MissionFailed(Mission mission);
    public event MissionFailed OnMissionFailed;

    public Mission(string id, string missionTitle, string missionDescription)
    {
        missionId = id;
        title = missionTitle;
        description = missionDescription;

    }
    public void TransitionToState(IMissionState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;
        currentState.OnEnter(this);

        OnStateChanged?.Invoke(this, currentState.StateName);
        Debug.Log($"[MISSION] {title} transitioned to {currentState.StateName}");
    }
    public IMissionState GetCurrentState()
    {
        return currentState;
    }
    public string GetStateName()
    {
        return currentState?.StateName ?? "None";
    }
    public void AddObjective(string objectiveId, string objectiveDescription)
    {
        objectives.Add(new MissionObjective(objectiveId, objectiveDescription));
    }
    public void Start()
    {
        if (currentState.StateName == "NotStarted" && objectives.Count > 0)
        {
            Debug.Log($"[MISSION] Starting {title}");
        }
    }
    public void CompleteCurrentObjective()
    {
        currentState.TryCompleteObjective(this);
    }
    public void Fail()
    {
        currentState.TryFail(this);
    }
    public MissionObjective GetCurrentObjective()
    {
        if (currentObjectiveIndex < objectives.Count)
            return objectives[currentObjectiveIndex];
        return null;
    }
    public List<MissionObjective> GetAllObjectives()
    {
        return objectives;
    }
    public float GetProgress()
    {
        if (objectives.Count == 0) return 0;
        return (float)currentObjectiveIndex / objectives.Count;
    }
    public void AdvanceObjective()
    {
        currentObjectiveIndex++;
    }
    public void InvokeObjectiveCompleted(MissionObjective objective)
    {
        OnObjectiveCompleted?.Invoke(this, objective);
    }
    public void InvokeMissionCompleted()
    {
        OnMissionCompleted?.Invoke(this);
    }
    public void InvokeMissionFailed()
    {
        OnMissionFailed?.Invoke(this);
    }
    public void Reset()
    {
        currentObjectiveIndex = 0;
        foreach (var obj in objectives)
        {
            obj.completed = false;
        }
    
    }
}
