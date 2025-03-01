using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIHeader : PopupEntity<UIHeader.Entity>
{
    [System.Serializable]
    public class Entity
    {
        public ButtonEntity Profile;
        public ButtonEntity Settings;
        public ButtonEntity Friends;
        public ButtonEntity Mail;
        public FormattedMoneyTextComponent.Entity Money;
        public string PlayerName;
    }

    public FormattedMoneyTextComponent Money;
    public TextMeshProUGUI PlayerName;
    public UIButtonComponent Profile;
    public UIButtonComponent Settings;
    public UIButtonComponent Friends;
    public UIButtonComponent Mail;

    protected override void OnSetEntity()
    {
        Profile.SetEntity(this.entity.Profile);
        Settings.SetEntity(this.entity.Settings);
        Friends.SetEntity(this.entity.Friends);
        Mail.SetEntity(this.entity.Mail);
        Money.SetEntity(this.entity.Money);
        PlayerName.text = this.entity.PlayerName;
    }

}
