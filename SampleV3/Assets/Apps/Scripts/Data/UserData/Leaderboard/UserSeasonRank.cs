using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSeasonRank : UserModel
{
    public int Rank { get; set; }
    public double Money { get; set; }
    public string IdOfSeason { get; set; }

    #region Getter

    protected MasterSeason _season;
    public MasterSeason Season
    {
        get
        {
            return _season ??= Core.Model.MasterData.Instance.Get<MasterSeason>(IdOfSeason);
        }
    }

    #endregion
}
