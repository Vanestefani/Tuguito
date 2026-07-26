using UnityEngine;

public class MissionObjective 
{
    public string id;
    public string description;
    public bool completed;
    public MissionObjective(string objectiveId, string objectiveDescription)
    {
        id = objectiveId;
        description = objectiveDescription;
        completed = false;
    }

    public void Complete()
    {
        completed = true;
    }
}
