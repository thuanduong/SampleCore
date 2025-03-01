using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestChallengeTab : PopupEntity<UIQuestChallengeTab.Entity>
{
    public class Entity {
        public IsVisibleComponent.Entity TopVisible;
        public UIComponentCountDownTimer.Entity ResetTime;
        public UIQuestListChallengeItem.Entity ListQuests;
    }

    public IsVisibleComponent TopVisible;
    public UIComponentCountDownTimer ResetTime;
    public UIQuestListChallengeItem ListQuests;

    protected override void OnSetEntity()
    {
        TopVisible.SetEntity(entity.TopVisible);
        ResetTime.SetEntity(entity.ResetTime);
        ListQuests.SetEntity(entity.ListQuests);
    }

    public void SetListQuest(UIQuestChallengeItem.Entity[] list)
    {
        entity.ListQuests.entities = list;
        ListQuests.SetEntity(entity.ListQuests);
    }
}
