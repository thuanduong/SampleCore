using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestChallengeItem : UIComponent<UIQuestChallengeItem.Entity>
{
    public class Entity {
        public UIImageComponent.Entity Icon;
        public string Title;
        public UIProgressBarComponent.Entity Process;
        public string ProcessValue;

        public UIImageComponent.Entity RewardIcon;
        public string Reward;
        public ButtonEntity Button;
    }

    public UIImageComponent Icon;
    public TextMeshProUGUI Title;
    public UIProgressBarComponent Process;
    public TextMeshProUGUI ProcessValue;
    public UIImageComponent RewardIcon;
    public TextMeshProUGUI Reward;
    public UIButtonComponent Button;

    protected override void OnSetEntity()
    {
        Icon.SetEntity(entity.Icon);
        Title.text = entity.Title;
        Process.SetEntity(entity.Process);
        ProcessValue.text = entity.ProcessValue;
        RewardIcon.SetEntity(entity.RewardIcon);
        Reward.text = entity.Reward;
        Button.SetEntity(entity.Button);
    }
}
