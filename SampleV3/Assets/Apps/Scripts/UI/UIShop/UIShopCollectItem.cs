using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopCollectItem : UIComponent<UIShopCollectItem.Entity>
{
    public class Entity {
        public ButtonEntity button;
        public UIImageComponent.Entity imageBG;
    }

    public UIButtonComponent button;
    public UIImageComponent imageBG;

    protected override void OnSetEntity()
    {
        button.SetEntity(entity.button);
        imageBG.SetEntity(entity.imageBG);
    }
}
