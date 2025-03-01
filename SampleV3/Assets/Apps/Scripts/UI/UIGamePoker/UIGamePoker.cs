using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class UIGamePoker : PopupEntity<UIGamePoker.Entity>
{
    public class Entity {
        public UIPlayerInfo.Entity[] Players;
        public UIPotComponent.Entity MainPot;
        public UIListPotComponent.Entity SidePots;
    }

    public UIPlayerInfo[] Players;
    public UIPotComponent MainPot;
    public UIListPotComponent SidePots;

    protected override void OnSetEntity()
    {
        if (Players != default) {
            for (int i = 0; i < Players.Length; i++)
            {
                if (i < entity.Players.Length)
                    Players[i].SetEntity(entity.Players[i]);
            }
        }

        if (entity.SidePots != default)
        {
            SidePots.SetEntity(entity.SidePots);
        }
        if (entity.MainPot != null)
            MainPot.SetEntity(entity.MainPot);
    }

    public async UniTask ShowAsync()
    {
        for (int i = 0; i < entity.Players.Length; i++)
        {
            await Players[i].In();
        }
    }

    public void SetSidePots(UIListPotComponent.Entity e)
    {
        entity.SidePots = e;
        SidePots.SetEntity(entity.SidePots);
    }

    public void SetMainPot(long money)
    {
        MainPot.SetMoney(money);
    }

}
