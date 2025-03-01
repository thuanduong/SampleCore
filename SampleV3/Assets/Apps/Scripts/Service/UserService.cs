using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;


public class UserService : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;
    private ISocketClient client;
    private ISocketClient Client => client ??= container.Inject<ISocketClient>();

    private static UserService instance;
    public static UserService Instance => instance;

    private UserProfileModel _current;
    public UserProfileModel Current
    {
        get
        {
            return _current ??= UserData.Instance.GetOne<UserProfileModel>();
        }
    }

    public void Save()
    {
        if (_current != default)
        {
            UserData.Instance.InsertOrUpdate(_current);
        }
    }

    public static UserService Instantiate(IDIContainer container)
    {
        if (instance == default)
        {
            instance = new UserService(container);
        }
        return instance;
    }

    private UserService(IDIContainer container)
    {
        this.container = container;
    }

    public void Dispose()
    {
        DisposeUtility.SafeDispose(ref cts);
        client = default;
        instance = default;
        _current = default;
    }

    //Game Mode
    public async UniTask getGameModeData()
    {
        //FakeData
        try
        {
            //await Client.Send
            MasterData.Instance.Drop<MasterGameMode>();
            MasterData.Instance.Insert(new MasterGameMode()
            {
                Id = "0",
                Name = $"{100000}-{1000000}",
                MoneyMin = 100000,
                MoneyMax = 1000000,
                Step = 10000,
                Order = 0,
                GameType = TypeOfGameMode.PRACTICE,
            });
            for (int i = 1; i < 10; i++)
            {
                var id = i.ToString();
                var index = i;
                long min = (i + 1 + (i * 10)) * 10000;
                long max = (i + 10 + (i * 10)) * 10000;
                MasterData.Instance.Insert(new MasterGameMode()
                {
                    Id = id,
                    Name = $"{min}-{max}",
                    MoneyMin = min,
                    MoneyMax = max,
                    Step = 10000,
                    Order = index,
                    GameType = TypeOfGameMode.CASH,
                });
            }
            
        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetGameModeData(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getGameModeData().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

    //Game Mode
    public async UniTask getStatusGameModeData()
    {
        //FakeData
        try
        {
            //await Client.Send
            UserData.Instance.Drop<UserGameMode>();
            for (int i = 0; i < 10; i++)
            {
                var id = i.ToString();
                bool isUnlock = i < 3;
                UserData.Instance.Insert(new UserGameMode()
                {
                    Id = id,
                    IdOfGameMode = id,
                    IsUnlock = isUnlock,
                });
            }
        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetStatusGameModeData(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getStatusGameModeData().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }
}
