using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;

public class QuestPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private LoadingPresenter loadingPresenter;
    private LoadingPresenter LoadingPresenter => loadingPresenter ??= Container.Inject<LoadingPresenter>();

    private bool isInit = false;

    public QuestPresenter(IDIContainer container)
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
        await QuestService.GetPlayerQuest(cts.Token);
        await QuestService.GetPlayerQuestChallenge(cts.Token);
        await QuestService.GetPlayerQuestReward(cts.Token);
    }

    public async UniTask<bool> ClaimChallenge(string IdOfChallenge)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        var kq = await QuestService.ClaimChallenge(IdOfChallenge, cts.Token);
        return kq;
    }

    public async UniTask<bool> ClaimReward(string IdOfReward)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        var kq =  await QuestService.ClaimReward(IdOfReward, cts.Token);
        return kq;
    }
}
