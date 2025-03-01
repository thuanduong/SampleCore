
public class MasterQuestChallenge : MasterModel
{
    public TypeOfChallenge TypeOfChallenge { get; set; }
    public float Max { get; set; }
    public TypeOfReward TypeOfReward { get; set; }
    public int RewardValue { get; set; }
    public string IdOfReward { get; set; } //Extra
}
