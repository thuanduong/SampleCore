using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILeaderboardPlayerRankItem : UIComponent<UILeaderboardPlayerRankItem.Entity>
{
    public class Entity
    {
        public UIImageComponent.Entity Icon;
        public string Rank;
        public string PlayerName;
        public FormattedMoneyTextComponent.Entity Money;
        public string Reward;
        public IsVisibleComponent.Entity RewardVisible;
    }

    public UIImageComponent Icon;
    public TextMeshProUGUI Rank;
    public TextMeshProUGUI PlayerName;
    public FormattedMoneyTextComponent Money;
    public TextMeshProUGUI Reward;
    public IsVisibleComponent RewardVisible;
    protected override void OnSetEntity()
    {
        Icon.SetEntity(entity.Icon);
        Rank.text = entity.Rank;
        PlayerName.text = entity.PlayerName;
        Money.SetEntity(entity.Money);
        Reward.text = entity.Reward;
        RewardVisible.SetEntity(entity.RewardVisible);
    }
}
