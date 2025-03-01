using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestRewardProcessItem : UIComponent<UIQuestRewardProcessItem.Entity>
{
    public class Entity {
        public string Number;
        public bool IsFirst;
        public bool IsLast;
    }

    public TextMeshProUGUI Number;
    public GameObject UpLine;
    public GameObject DownLine;

    protected override void OnSetEntity()
    {
        Number.text = entity.Number;
        UpLine.SetActive(!entity.IsFirst);
        DownLine.SetActive(!entity.IsLast);
    }
}
