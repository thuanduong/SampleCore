using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserQuestReward : UserModel
{
    public string IdOfQuestReward;
    public bool Claimable;
    public bool Claimed;

   

    #region Getter

    private MasterQuestReward _reward;
    public MasterQuestReward Reward
    {
        get
        {
            return _reward ??= Core.Model.MasterData.Instance.Get<MasterQuestReward>(IdOfQuestReward);
        }
    }

    public int IdInt
    {
        get
        {
            return !string.IsNullOrEmpty(Id) ? Id.TryToConvertInt32(defaultValue: -1) : -1;
        }
    }

    #endregion
}
