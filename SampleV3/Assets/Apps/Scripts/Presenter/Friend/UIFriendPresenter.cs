using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class UIFriendPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private UIFriend uiFriend;
    private UserFriend[] friends;

    private List<string> inviteDelay = new List<string>();


    private FriendPresenter friendPresenter;
    private FriendPresenter FriendPresenter => friendPresenter ??= Container.Inject<FriendPresenter>();

    public event Action OnClose = ActionUtility.EmptyAction.Instance;

    public UIFriendPresenter(IDIContainer container)
    {
        Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
        UILoader.SafeRelease(ref uiFriend);
    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await FetchData();

        uiFriend ??= await UILoader.Instantiate<UIFriend>(token: cts.Token);
        uiFriend.SetEntity(new UIFriend.Entity()
        {
            btnClose = new ButtonEntity(OnClose),
            ListFriend = new UIFriendListInfoItem.Entity()
            {
                entities = await loadListFriend(),
                Start = 0,
            },
        });
        await UniTask.WhenAll(uiFriend.In());
    }

    public async UniTask HideAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await uiFriend.Out();
    }

    private async UniTask FetchData()
    {
        await FriendPresenter.FetchData();
        friends = UserData.Instance.GetAll<UserFriend>().OrderBy(x => x.Order).ToArray();
    }

    private async UniTask<UIFriendInfoItem.Entity[]> loadListFriend()
    {
        List<UIFriendInfoItem.Entity> ml = new List<UIFriendInfoItem.Entity>();
        if (friends != default)
        {
            for(int i = 0; i < friends.Length; i++)
            {
                var f = friends[i];
                var type = f.ActiveStatus;
                int index = i;
                bool canInvite = type == TypeOfActiveStatus.Active || type == TypeOfActiveStatus.Watching;
                bool disableInvite = inviteDelay.Contains(f.Id);
                bool canJoin = type == TypeOfActiveStatus.Playing;
                ml.Add(new UIFriendInfoItem.Entity()
                {
                    IdOfUser = f.Id,
                    PlayerName = f.UserName,
                    CurrentMoney = new FormattedMoneyTextComponent.Entity(f.Chip),
                    Icon = new UIImageComponent.Entity(),
                    btnAdd = new ButtonEntity() { isDisable = true },
                    btnInvite = canInvite ? new ButtonEntity(()=> OnInvite(index).Forget(), isInteractable: !disableInvite) : new ButtonEntity() { isDisable = true },
                    btnJoin = canJoin ? new ButtonEntity(() => OnJoin(index).Forget()) : new ButtonEntity() { isDisable = true },
                    btnRemove = new ButtonEntity(() => OnRemoveFriend(index).Forget()),
                });
            }
        }
        return ml.ToArray();
    }

    private async UniTask OnInvite(int index)
    {
        var f = friends[index];
        inviteDelay.Add(f.Id);
        var buttonIndex = uiFriend.ListFriend.instanceList.FindIndex(x => x.entity.IdOfUser == f.Id);
        if (buttonIndex != -1)
        {
            uiFriend.ListFriend.instanceList[buttonIndex].btnInvite.DelayButtonPress(5);
        }
        Debug.Log("Invite Friend " + f.Id);
        await FriendPresenter.InviteFriend(f.Id);
        waitToRemoveDelay(f.Id, 5.0f).Forget();
        await UniTask.CompletedTask;
    }

    private async UniTask waitToRemoveDelay(string id, float duration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: cts.Token);
        inviteDelay.Remove(id);
        var buttonIndex = uiFriend.ListFriend.instanceList.FindIndex(x => x.entity.IdOfUser == id);
        if (buttonIndex != -1)
        {
            uiFriend.ListFriend.instanceList[buttonIndex].btnInvite.SetInteracble(true);
        }
    }

    private async UniTask OnJoin(int index)
    {
        var f = friends[index];
        await FriendPresenter.JoinGameFriend(f.Id);
        await UniTask.CompletedTask;
    }

    private async UniTask OnRemoveFriend(int index)
    {
        var f = friends[index];
        bool kq = await FriendPresenter.RemoveFriend(f.Id);
        if (kq)
        {
            friends = UserData.Instance.GetAll<UserFriend>().OrderBy(x=>x.Order).ToArray();
            uiFriend.SetListFriend(await loadListFriend());
        }
        await UniTask.CompletedTask;
    }
}
