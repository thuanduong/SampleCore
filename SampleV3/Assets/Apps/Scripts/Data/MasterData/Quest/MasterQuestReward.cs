using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterQuestReward : MasterModel
{
    public TypeOfReward TypeOfReward { get; set; }
    public int RewardValue { get; set; }
    public string IdOfReward { get; set; } //Extra

    public int Vip { get; set; }
    public int ChallengePoint { get; set; }

}
