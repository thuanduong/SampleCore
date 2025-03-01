using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class UILeaderboardPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private UILeaderboard uiLeaderboard;

    LeaderboardPresenter leaderboard;
    LeaderboardPresenter Leaderboard => leaderboard ??= Container.Inject<LeaderboardPresenter>();
    private LoadingPresenter uiLoadingPresenter;
    private LoadingPresenter UiLoadingPresenter => uiLoadingPresenter ??= this.Container.Inject<LoadingPresenter>();

    PlayerSeasonRank[] ranks;
    UserSeasonRank myRank;
    MasterSeason season;

    public UILeaderboardPresenter(IDIContainer container)
    {
        Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
        UILoader.SafeRelease(ref uiLeaderboard);

        uiLoadingPresenter = default;
        leaderboard = default;
        ranks = default;
        myRank = default;
        season = default;
    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await UiLoadingPresenter.ActiveWaiting(true);
        await FetchData();

        uiLeaderboard ??= await UILoader.Instantiate<UILeaderboard>(token: cts.Token);

        var data = await LoadRankSeason();
        string currentM = myRank.Money.ToMoney();
        string targetM = season.Target.ToMoney();
        var processPercent = CalculatorPercent(myRank, ranks, season);
        var typeOfSeason = season.Upper > 0 ? (season.Alive > 0 ? 1 : 0) : 2;
        var mm = uiLeaderboard.ProcessDefine.First(x => (int)x.Type == typeOfSeason);
        var last = processPercent.Item2 > 0 ? mm.Percent[processPercent.Item2 - 1] : 0;
        var trueMM = processPercent.Item1 * (mm.Percent[processPercent.Item2] - last) + last;
        uiLeaderboard.SetEntity(new UILeaderboard.Entity()
        {
            RewardValue_1 = $"+{(season.BenefitBuyIn * 100)}",
            RewardValue_2 = $"+{(season.BenefitBonus * 100)}",
            RewardValue_3 = $"+{(season.BenefitPayment * 100)}",
            Collect = new FormattedTextComponent.Entity(currentM, targetM),
            Process = new UIProgressBarComponent.Entity() { progress = trueMM },
            ProcessValue = myRank.Rank.ToString(),
            EndTime = new UIComponentCountDownTimer.Entity() { utcTick = season.EndTime, },
            ProcessBGFirst = new IsVisibleComponent.Entity(typeOfSeason == 0),
            ProcessBGCommon = new IsVisibleComponent.Entity(typeOfSeason == 1),
            ProcessBGFinal = new IsVisibleComponent.Entity(typeOfSeason == 2),
            NextIcon = new UIImageComponent.Entity(),
            ListUpperRanks = new UILeaderboardListPlayerRankItem.Entity()
            {
                entities = data.Item1,
                customTypes = data.Item2,
            },
            ListAliveRanks = new UILeaderboardListPlayerRankItem.Entity(),
            ListDownRanks = new UILeaderboardListPlayerRankItem.Entity(),
        });

        await UniTask.Delay(500, cancellationToken: cts.Token);
        await uiLeaderboard.In();
        await UiLoadingPresenter.ActiveWaiting(false);
    }

    public async UniTask HideAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await uiLeaderboard.Out();
    }

    private async UniTask FetchData()
    {
        await Leaderboard.FetchUserRankData();
        await Leaderboard.FetchCurrentRankData();

        myRank = UserData.Instance.GetOne<UserSeasonRank>();
        season = myRank.Season;
        ranks = UserData.Instance.GetAll<PlayerSeasonRank>().OrderBy(x => x.Rank).ToArray();

    }

    private async UniTask<(UILeaderboardPlayerRankItem.Entity[], int[])> LoadRankSeason()
    {
        List<UILeaderboardPlayerRankItem.Entity> ml = new List<UILeaderboardPlayerRankItem.Entity>();
        List<int> mm = new List<int>();

        for (int i = 0; i < ranks.Length; i++)
        {
            var item = ranks[i];
            if (item.Rank == 1)
                mm.Add(1);
            else if (item.Rank == 100)
                mm.Add(2);
            else
                mm.Add(0);

            var reward = item.Season.getRank(item.Rank);

            ml.Add(new UILeaderboardPlayerRankItem.Entity()
            {
                Icon = new UIImageComponent.Entity(),
                Rank = item.Rank.ToRank(),
                Money = new FormattedMoneyTextComponent.Entity(item.Money),
                PlayerName = item.PlayerName,
                Reward = reward != default ? $"X{reward.Ticket}" : "",
                RewardVisible = new IsVisibleComponent.Entity(reward != null)
            });
        }

        return (ml.ToArray(), mm.ToArray());
    }

    public (float, int) CalculatorPercent(UserSeasonRank myRank, PlayerSeasonRank[] all, MasterSeason season)
    {
        int totalSeason = season.Alive > 0 && season.Upper > 0 ? 3 : 2;
        float percent = 0;
        if (season.Upper > 0)
        {
            if (myRank.Rank <= season.Upper)
            {
                percent = 1.0f - ((float)myRank.Rank / season.Upper);
                return (percent, totalSeason - 1);
            }
        }

        if (season.Alive > 0)
        {
            if (myRank.Rank <= season.Alive)
            {
                int to = all.Length - season.Upper;
                if (to == 0) to = 1;
                percent = 1.0f - ((float)(myRank.Rank - season.Upper) / to);
                return (percent, 1);
            }
        }
        else
        {
            int to = all.Length - season.Upper;
            if (to == 0) to = 1;
            percent = 1.0f - ((float)(myRank.Rank - season.Upper)/ to);
            return (percent, 0);
        }

        //Down
        int mm = season.Alive;
        int tot = all.Length - mm;
        if (tot == 0) tot = 1;
        percent = 1.0f - (((float)myRank.Rank - mm) / tot);
        return (percent, 0);
    }
}
