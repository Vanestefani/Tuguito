using UnityEngine;

public interface IMissionState
{
    string StateName { get; }
    void OnEnter(Mission mission);
    void OnExit(Mission mission);
    void TryCompleteObjective(Mission mission);
    void TryFail(Mission mission);
    void Update(Mission mission);
}
