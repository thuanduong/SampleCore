using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGameMode : UserModel
{
    public string IdOfGameMode { get; set; }
    public bool IsUnlock { get; set; }

    #region getter

    private MasterGameMode _mode;
    public MasterGameMode Mode
    {
        get { 
            return _mode ??= Core.Model.MasterData.Instance.Get<MasterGameMode>(IdOfGameMode);
        }
    }

    #endregion

}
