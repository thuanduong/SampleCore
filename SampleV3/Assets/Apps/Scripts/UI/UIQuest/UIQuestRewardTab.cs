using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestRewardTab : PopupEntity<UIQuestRewardTab.Entity>
{
    public class Entity {
        public UIQuestRewardListItem.Entity ListFree;
        public UIQuestRewardListItem.Entity ListVIP;
        public UIQuestRewardListProcessItem.Entity ListProcess;
        public UIProgressBarComponent.Entity Process;
        public string ProcessValue;
    }

    public UIQuestRewardListItem ListFree;
    public UIQuestRewardListItem ListVIP;
    public UIQuestRewardListProcessItem ListProcess;
    public UIProgressBarComponent Process;
    public TextMeshProUGUI ProcessValue;

    public GridLayoutGroup freeGrid;
    public GridLayoutGroup vipGrid;
    public GridLayoutGroup processGrid;

    private void Start()
    {
        freeGrid.enabled = false;
        vipGrid.enabled = false;
        processGrid.enabled = false;
    }

    protected override void OnSetEntity()
    {
        ListFree.SetEntity(entity.ListFree);
        ListVIP.SetEntity(entity.ListVIP);
        ListProcess.SetEntity(entity.ListProcess);
        Process.SetEntity(entity.Process);
        ProcessValue.text = entity.ProcessValue;
    }

    public void HandleGrid(bool active)
    {
        freeGrid.enabled = active;
        vipGrid.enabled = active;
        processGrid.enabled = active;
    }
}
