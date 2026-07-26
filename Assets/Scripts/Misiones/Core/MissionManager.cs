using System.Collections.Generic;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    private Dictionary<string, Mission> missions = new Dictionary<string, Mission>();
    private List<string> activeMissionIds = new List<string>();
    public delegate void GlobalMissionChanged(Mission mission, string newState);
    public event GlobalMissionChanged OnMissionStateChanged;

    private static MissionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static MissionManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("[MISSION MANAGER] MissionManager no encontrado en la escena. Crea un GameObject con el script MissionManager");
            }
            return instance;
        }
    }
    public Mission CreateMission(string missionId, string title, string description)
    {
        if (missions.ContainsKey(missionId))
        {
            Debug.LogWarning($"[MISSION MANAGER] Mission {missionId} already exists!");
            return missions[missionId];
        }

        var mission = new Mission(missionId, title, description);
        missions[missionId] = mission;

       
        mission.TransitionToState(new NotStartedState());

       
        mission.OnStateChanged += (m, state) => HandleMissionStateChanged(m, state);

        return mission;
    }
    public Mission GetMission(string missionId)
    {
        if (missions.TryGetValue(missionId, out var mission))
            return mission;
        Debug.LogWarning($"[MISSION MANAGER] Mission {missionId} not found!");
        return null;
    }
    public void StartMission(string missionId)
    {
        var mission = GetMission(missionId);
        if (mission != null)
        {
            mission.Start();

            if (mission.GetStateName() == "NotStarted" && mission.GetAllObjectives().Count > 0)
            {
                mission.TransitionToState(new InProgressState());
            }

            if (!activeMissionIds.Contains(missionId))
                activeMissionIds.Add(missionId);
        }
    }
    public List<Mission> GetActiveMissions()
    {
        var activeMissions = new List<Mission>();
        foreach (var id in activeMissionIds)
        {
            if (missions.TryGetValue(id, out var mission) && mission.GetStateName() == "InProgress")
                activeMissions.Add(mission);
        }
        return activeMissions;
    }
    public List<Mission> GetAllMissions()
    {
        return new List<Mission>(missions.Values);
    }
    public List<Mission> GetMissionsByState(string stateName)
    {
        var result = new List<Mission>();
        foreach (var mission in missions.Values)
        {
            if (mission.GetStateName() == stateName)
                result.Add(mission);
        }
        return result;
    }
    private void HandleMissionStateChanged(Mission mission, string newState)
    {
        OnMissionStateChanged?.Invoke(mission, newState);

        if (newState == "Completed" || newState == "Failed")
        {
            activeMissionIds.Remove(mission.missionId);
        }
    }
    private void Update()
    {
        foreach (var missionId in activeMissionIds)
        {
            if (missions.TryGetValue(missionId, out var mission))
            {
                mission.GetCurrentState().Update(mission);
            }
        }
    }
    public void ClearAllMissions()
    {
        missions.Clear();
        activeMissionIds.Clear();
    }
    public void RemoveMission(string missionId)
    {
        missions.Remove(missionId);
        activeMissionIds.Remove(missionId);
    }
}
