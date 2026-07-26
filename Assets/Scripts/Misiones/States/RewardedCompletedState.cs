using UnityEngine;

public class RewardedCompletedState : CompletedState
{
    private string rewardId;
    private int rewardAmount;
    public RewardedCompletedState(string reward, int amount)
    {
        rewardId = reward;
        rewardAmount = amount;
    }
    public new void OnEnter(Mission mission)
    {
        
        base.OnEnter(mission);

        Debug.Log($"[REWARD STATE] Giving reward: {rewardAmount}x {rewardId}");
        GiveReward(rewardId, rewardAmount);
    }
    private void GiveReward(string id, int amount)
    {
       
        Debug.Log($">>> REWARD GRANTED: +{amount} {id}");

    }
    public (string id, int amount) GetRewardInfo()
    {
        return (rewardId, rewardAmount);
    }
}
