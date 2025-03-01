using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : PopupEntity<UIMainMenu.Entity>
{
    public class Entity
    {
        public UISliderView.Entity CashGameList;
    }

    public UISliderView CashGameList;

    protected override void OnSetEntity()
    {
        CashGameList.SetEntity(entity.CashGameList);
    }
}
