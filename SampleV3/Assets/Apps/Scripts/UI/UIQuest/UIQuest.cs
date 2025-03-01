using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuest : PopupEntity<UIQuest.Entity>
{
    public class Entity {
        public UIToggleComponent.Entity ToggleChallenge;
        public UIToggleComponent.Entity ToggleReward;
        public UIQuestChallengeTab.Entity Challenges;
        public UIQuestRewardTab.Entity Rewards;
    }

    public UIToggleComponent ToggleChallenge;
    public UIToggleComponent ToggleReward;
    public UIQuestChallengeTab Challenges;
    public UIQuestRewardTab Rewards;

    protected override void OnSetEntity()
    {
        ToggleChallenge.SetEntity(entity.ToggleChallenge);
        ToggleReward.SetEntity(entity.ToggleReward);
        Challenges.SetEntity(entity.Challenges);
        Rewards.SetEntity(entity.Rewards);
    }
}
