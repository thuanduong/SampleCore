using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayerInfo : PopupEntity<UIPlayerInfo.Entity>
{
    public class Entity
    {
        public string PlayerName;
        public string PlayerAction;
        public int TypeOfAction;
        public IsVisibleComponent.Entity ActionVisible;
        public UIImageComponent.Entity Avatar;
        public long Money;
        public UIHandType.Entity HandType;
        public UIImageFillCountDownTimer.Entity CountDown;
    }

    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerAction;
    public IsVisibleComponent ActionVisible;
    public UIImageComponent Avatar;
    public TextMeshProUGUI Money;
    public UIImageComponent ActionImg;
    public UIHandType HandType;
    public UIImageFillCountDownTimer CountDown;

    [Space, Header("Config")]
    public List<Sprite> ActionSprites;

    protected override void OnSetEntity()
    {
        if (PlayerName != null)
            PlayerName.text = entity.PlayerName;
        ActionVisible.SetEntity(entity.ActionVisible);
        if (entity.ActionVisible != null && entity.ActionVisible.isVisible) 
            SetAction(entity.PlayerAction, entity.TypeOfAction);
        if (Avatar != null)
            Avatar.SetEntity(entity.Avatar);
        Money.text = entity.Money.ToString();
        HandType.SetEntity(entity.HandType);
        CountDown.SetEntity(entity.CountDown);
    }

    protected void setActionImg(int type)
    {
        var s = ActionImg.entity;
        if (s == default)
        {
            s = new UIImageComponent.Entity();
            ActionImg.SetEntity(s);
        }
        if (ActionSprites.Count == 0)
        {
            s.sprite = null;
            ActionImg.SetEntity(s);
        }
        var _type = Mathf.Clamp(type, 0, ActionSprites.Count - 1);
        s.sprite = ActionSprites[_type];
        ActionImg.SetEntity(s);
    }

    public void SetAction(string text, int type)
    {
        entity.PlayerAction = text;
        entity.TypeOfAction = type;
        if (PlayerAction != null)
            PlayerAction.text = entity.PlayerAction;
        setActionImg(type);
    }
    
    public void ActiveAction(bool active)
    {
        ActionVisible.entity.isVisible = active;
        ActionVisible.SetEntity(ActionVisible.entity);
    }

}
