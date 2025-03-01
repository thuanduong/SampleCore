using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDefine
{
    
}

//Store
namespace App.Store
{
    public enum TypeOfProduct
    {
        VideoAds,
        Collector,
        Consumable,
        NonConsumable,
        Subscription,
    }
}

public enum TypeOfChallenge
{
    WinMoney,
    WinMoneyInOneGame,
    WinOneGameWithHand,
}

public enum TypeOfReward
{
    ChallengePoint,
    Chip,
}

public enum TypeOfActiveStatus
{
    InActive = 0,
    Active = 1,
    Playing = 2,
    Watching = 3,
}

public enum TypeOfAvatar
{
    Asset,
    URL,
    Compress,
}

public enum TypeOfAccount
{
    JUST_REGISTERED,
    EMAIL_VERIFIED
}

public enum TypeOfGameMode
{
    PRACTICE = 0,
    CASH = 1,
}

public enum TypeOfGamePlay
{
    LOCAL = 0,
    MULTIPLAYER = 1,
    HISTORY = 2,
}