using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPotComponent : UIComponent<UIPotComponent.Entity>
{
    public class Entity
    {
        public FormattedMoneyTextComponent.Entity Money;
    }

    public IsVisibleComponent IsVisible;
    public FormattedMoneyTextComponent Money;

    protected override void OnSetEntity()
    {
        Money.SetEntity(entity.Money);
        setVisible(entity.Money.Money);
    }

    public void SetMoney(long money)
    {
        entity.Money.Money = money;
        Money.SetEntity(entity.Money);
        setVisible(money);
    }

    private void setVisible(double Money)
    {
        IsVisibleComponent.Entity mm = IsVisible.entity;
        if (mm == default)
        {
            mm = new IsVisibleComponent.Entity();
        }
        mm.isVisible = Money > 0;
        IsVisible.SetEntity(mm);
    }
}
