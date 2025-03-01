using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPopupBuyIn : PopupEntity<UIPopupBuyIn.Entity>
{
    public class Entity {
        public string Title;
        public string NumPlayer;
        public ButtonEntity ButtonSub;
        public ButtonEntity ButtonAdd;
        public FormattedMoneyTextComponent.Entity BuyIn;
        public FormattedMoneyTextComponent.Entity Balance;
        public FormattedMoneyTextComponent.Entity Min;
        public FormattedMoneyTextComponent.Entity Max;
        public UIProgressBarComponent.Entity Process;
        public UIToggleComponent.Entity AutoRebuy;
        public UIToggleComponent.Entity AutoTopOff;
        public ButtonEntity buttonBuyIn;
        public ButtonEntity buttonClose;
    }

    public TextMeshProUGUI Title;
    public TextMeshProUGUI NumPlayer;
    public UIButtonComponent ButtonSub;
    public UIButtonComponent ButtonAdd;
    public FormattedMoneyTextComponent BuyIn;
    public FormattedMoneyTextComponent Balance;
    public FormattedMoneyTextComponent Min;
    public FormattedMoneyTextComponent Max;
    public UIProgressBarComponent Process;
    public UIToggleComponent AutoRebuy;
    public UIToggleComponent AutoTopOff;
    public UIButtonComponent buttonBuyIn;
    public UIButtonComponent buttonClose;

    protected override void OnSetEntity()
    {
        Title.text = entity.Title;
        NumPlayer.text = entity.NumPlayer;
        BuyIn.SetEntity(entity.BuyIn);
        Balance.SetEntity(entity.Balance);
        Min.SetEntity(entity.Min);
        Max.SetEntity(entity.Max);
        Process.SetEntity(entity.Process);
        AutoRebuy.SetEntity(entity.AutoRebuy);
        AutoTopOff.SetEntity(entity.AutoTopOff);
        buttonBuyIn.SetEntity(entity.buttonBuyIn);
        buttonClose.SetEntity(entity.buttonClose);
        ButtonSub.SetEntity(entity.ButtonSub);
        ButtonAdd.SetEntity(entity.ButtonAdd);
    }

    public void SetBuyIn(double value)
    {
        entity.BuyIn.Money = value;
        BuyIn.SetEntity(entity.BuyIn);
    }
}
