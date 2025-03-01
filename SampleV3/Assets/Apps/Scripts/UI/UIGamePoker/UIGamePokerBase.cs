using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePokerBase : PopupEntity<UIGamePokerBase.Entity>
{
    public class Entity
    {
        public ButtonEntity btnCall;
        public ButtonEntity btnRaise;
        public ButtonEntity btnAllIn;
        public ButtonEntity btnFold;
        public IsVisibleComponent.Entity visibleProcess;
        public UIProgressBarComponent.Entity moneyProcess;
        public string money;
        public UIImageComponent.Entity HandType;
    }

    public UIButtonComponent btnCall;
    public UIButtonComponent btnRaise;
    public UIButtonComponent btnAllIn;
    public UIButtonComponent btnFold;
    public UIProgressBarComponent moneyProcess;
    public IsVisibleComponent visibleProcess;
    public TMPro.TextMeshProUGUI moneyText;
    public UIImageComponent HandType;

    protected override void OnSetEntity()
    {
        btnCall.SetEntity(entity.btnCall);
        btnRaise.SetEntity(entity.btnRaise);
        btnAllIn.SetEntity(entity.btnAllIn);
        btnFold.SetEntity(entity.btnFold);
        moneyProcess.SetEntity(entity.moneyProcess);
        visibleProcess.SetEntity(entity.visibleProcess);
        moneyText.text = entity.money;
        if (HandType != null)
            HandType.SetEntity(entity.HandType);
    }

    public void SetMoney(string money)
    {
        entity.money = money;
        moneyText.text = entity.money;
    }

    public void setProcess(float percent)
    {
        moneyProcess.SetPercent(percent);
    }

    public int GetMoney()
    {
        try
        {
            int m = System.Convert.ToInt32(entity.money);
            return m;
        }
        catch
        {
            return 0;
        }
    }

    public void SetData(int moneyLeft, int minRaise, int moneyToCall, int myMoneyInTheRound)
    {
        
    }

    public void SetHandType(Sprite s)
    {
        if (entity.HandType == default)
            entity.HandType = new UIImageComponent.Entity() { sprite = s };
        else
            entity.HandType.sprite = s;

        HandType.SetEntity(entity.HandType);
    }
}
