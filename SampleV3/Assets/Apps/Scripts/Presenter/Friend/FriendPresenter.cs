using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;


public class FriendPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private LoadingPresenter loadingPresenter;
    private LoadingPresenter LoadingPresenter => loadingPresenter ??= Container.Inject<LoadingPresenter>();

    public FriendPresenter(IDIContainer container)
    {
        this.Container = container;
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

        await FriendService.GetPlayerFriends(cts.Token);
    }

    public async UniTask InviteFriend(string friendID)
    {
        await LoadingPresenter.ActiveWaiting(true);
        await UniTask.Delay(1000);
        await LoadingPresenter.ActiveWaiting(false);
    }

    public async UniTask JoinGameFriend(string friendID)
    {
        await LoadingPresenter.ActiveWaiting(true);
        await UniTask.Delay(1000);
        await LoadingPresenter.ActiveWaiting(false);
    }

    public async UniTask<bool> RemoveFriend(string friendID)
    {
        await LoadingPresenter.ActiveWaiting(true);
        await UniTask.Delay(1000);
        var m = Core.Model.UserData.Instance.Get<UserFriend>(friendID);
        Core.Model.UserData.Instance.Delete<UserFriend>(m);

        await LoadingPresenter.ActiveWaiting(false);
        return true;
    }
}
