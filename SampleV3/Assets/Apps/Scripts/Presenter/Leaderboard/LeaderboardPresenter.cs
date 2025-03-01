using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;


public class LeaderboardPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private LoadingPresenter loadingPresenter;
    private LoadingPresenter LoadingPresenter => loadingPresenter ??= Container.Inject<LoadingPresenter>();

    private bool isInit = false;

    public LeaderboardPresenter(IDIContainer container)
    {
        this.Container = container;
        isInit = false;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
    }

    public async UniTask FetchData()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await LeaderboardService.GetMasterDataSeasons(cts.Token);
    }

    public async UniTask FetchCurrentRankData()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await LeaderboardService.GetPlayerSeasonRank(cts.Token);
    }

    public async UniTask FetchUserRankData()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await LeaderboardService.GetUserSeasonRank(cts.Token);
    }

}
