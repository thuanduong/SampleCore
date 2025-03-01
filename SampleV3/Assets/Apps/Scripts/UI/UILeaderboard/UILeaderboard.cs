using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILeaderboard : PopupEntity<UILeaderboard.Entity>
{
    public class Entity {
        public string RewardValue_1;
        public string RewardValue_2;
        public string RewardValue_3;

        public UIProgressBarComponent.Entity Process;
        public string ProcessValue;
        public UIImageComponent.Entity NextIcon;
        public UIComponentCountDownTimer.Entity EndTime;

        public IsVisibleComponent.Entity ProcessBGFirst;
        public IsVisibleComponent.Entity ProcessBGCommon;
        public IsVisibleComponent.Entity ProcessBGFinal;

        public FormattedTextComponent.Entity Collect;

        public UILeaderboardListPlayerRankItem.Entity ListUpperRanks;
        public UILeaderboardListPlayerRankItem.Entity ListAliveRanks;
        public UILeaderboardListPlayerRankItem.Entity ListDownRanks;
    }

    public TextMeshProUGUI RewardValue_1;
    public TextMeshProUGUI RewardValue_2;
    public TextMeshProUGUI RewardValue_3;

    public UIProgressBarComponent Process;
    public TextMeshProUGUI ProcessValue;
    public UIImageComponent NextIcon;
    public UIComponentCountDownTimer EndTime;

    public IsVisibleComponent ProcessBGFirst;
    public IsVisibleComponent ProcessBGCommon;
    public IsVisibleComponent ProcessBGFinal;

    public FormattedTextComponent Collect;

    public UILeaderboardListPlayerRankItem ListUpperRanks;
    public UILeaderboardListPlayerRankItem ListAliveRanks;
    public UILeaderboardListPlayerRankItem ListDownRanks;

    [Space, Header("Define")]
    [SerializeField] List<ProcessPercentAttribute> processDefine;

    public List<ProcessPercentAttribute> ProcessDefine => processDefine;

    public enum ProcessType
    {
        FIRST = 0,
        COMMON = 1,
        FINAL = 2,
    }

    [System.Serializable]
    public struct ProcessPercentAttribute
    {
        public ProcessType Type;
        public List<float> Percent;
    }

    protected override void OnSetEntity()
    {
        RewardValue_1.text = entity.RewardValue_1;
        RewardValue_2.text = entity.RewardValue_2;
        RewardValue_3.text = entity.RewardValue_3;

        Process.SetEntity(entity.Process);
        ProcessValue.text = entity.ProcessValue;
        NextIcon.SetEntity(entity.NextIcon);
        EndTime.SetEntity(entity.EndTime);

        ProcessBGFirst.SetEntity(entity.ProcessBGFirst);
        ProcessBGCommon.SetEntity(entity.ProcessBGCommon);
        ProcessBGFinal.SetEntity(entity.ProcessBGFinal);

        Collect.SetEntity(entity.Collect);

        ListUpperRanks.SetEntity(entity.ListUpperRanks);
        ListAliveRanks.SetEntity(entity.ListAliveRanks);
        ListDownRanks.SetEntity(entity.ListDownRanks);
    }
}

