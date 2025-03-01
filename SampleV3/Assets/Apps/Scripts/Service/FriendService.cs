using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;


public class FriendService : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;
    private ISocketClient client;
    private ISocketClient Client => client ??= container.Inject<ISocketClient>();

    private static FriendService instance;
    public static FriendService Instance => instance;

    public static FriendService Instantiate(IDIContainer container)
    {
        if (instance == default)
        {
            instance = new FriendService(container);
        }
        return instance;
    }

    private FriendService(IDIContainer container)
    {
        this.container = container;
        cts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        DisposeUtility.SafeDispose(ref cts);
        client = default;
        instance = default;
    }

    public async UniTask getPlayerFriends()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            UserData.Instance.Drop<UserFriend>();
            for (int i = 0; i < 50; i++)
            {
                string id = i.ToString();
                string name = $"Player {i}";
                int index = i;
                UserData.Instance.Insert(new UserFriend()
                {
                    Id = id,
                    UserName = name,
                    Chip = i * 1000000 + i * 10000,
                    ActiveStatus = TypeOfActiveStatus.Active,
                    UrlAvatar = "",
                    AvatarType = TypeOfAvatar.Asset,
                    Order = index,
                });
            }

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetPlayerFriends(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getPlayerFriends().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

}
