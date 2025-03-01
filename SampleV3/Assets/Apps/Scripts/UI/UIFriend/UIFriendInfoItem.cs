using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFriendInfoItem : UIComponent<UIFriendInfoItem.Entity>
{
    public class Entity {
        public string IdOfUser;
        public UIImageComponent.Entity Icon;
        public string PlayerName;
        public FormattedMoneyTextComponent.Entity CurrentMoney;
        public ButtonEntity btnAdd;
        public ButtonEntity btnJoin;
        public ButtonEntity btnInvite;
        public ButtonEntity btnRemove;
    }

    public UIImageComponent Icon;
    public TextMeshProUGUI PlayerName;
    public FormattedMoneyTextComponent CurrentMoney;
    public UIButtonComponent btnAdd;
    public UIButtonComponent btnJoin;
    public UIButtonComponent btnInvite;
    public UIButtonComponent btnRemove;

    protected override void OnSetEntity()
    {
        Icon.SetEntity(entity.Icon);
        PlayerName.text = entity.PlayerName;
        CurrentMoney.SetEntity(entity.CurrentMoney);
        btnAdd.SetEntity(entity.btnAdd);
        btnJoin.SetEntity(entity.btnJoin);
        btnInvite.SetEntity(entity.btnInvite);
        btnRemove.SetEntity(entity.btnRemove);
    }

}
