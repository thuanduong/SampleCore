using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestRewardItem : UIComponent<UIQuestRewardItem.Entity>
{
    public class Entity 
    {
        public UIImageComponent.Entity Icon;
        public string Value;
        public ButtonEntity Button;
    }

    public UIImageComponent Icon;
    public TextMeshProUGUI Value;
    public UIButtonComponent Button;

    protected override void OnSetEntity()
    {
        Icon.SetEntity(entity.Icon);
        Value.text = entity.Value;
        Button.SetEntity(entity.Button);
    }
}
