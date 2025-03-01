using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIShopConsumableItem : UIComponent<UIShopConsumableItem.Entity>
{
    public class Entity
    {
        public UIImageComponent.Entity image;
        public string title;
        public ButtonEntity btnOpen;
        public string price;
    }

    public UIImageComponent image;
    public TextMeshProUGUI title;
    public UIButtonComponent btnOpen;
    public TextMeshProUGUI price;

    protected override void OnSetEntity()
    {
        image.SetEntity(entity.image);
        title.text = entity.title;
        btnOpen.SetEntity(entity.btnOpen);
        price.text = entity.price;
    }
}
