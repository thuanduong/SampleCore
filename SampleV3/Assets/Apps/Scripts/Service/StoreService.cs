using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class StoreService : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;
    private ISocketClient client;
    private ISocketClient Client => client ??= container.Inject<ISocketClient>();

    private static StoreService instance;
    public static StoreService Instance => instance;

    public static StoreService Instantiate(IDIContainer container)
    {
        if (instance == default) { 
            instance = new StoreService(container);
        }
        return instance;
    }

    private StoreService(IDIContainer container)
    {
        this.container = container;
    }

    public void Dispose()
    {
        DisposeUtility.SafeDispose(ref cts);
        client = default;
        instance = default;
    }

    public async UniTask getStoreData(CancellationToken token)
    {
        try
        {
            //await Client.Send

            //FakeData
            //MasterData
            MasterData.Instance.Drop<MasterShopData>();
            MasterData.Instance.Insert(new MasterShopData()
            {
                Id = "Video_Ads",
                IdOfProduct = "",
                IsIAPProduct = false,
                Name = "Free Chips",
                TypeOfProduct = App.Store.TypeOfProduct.VideoAds,
            });
            MasterData.Instance.Insert(new MasterShopData()
            {
                Id = "money_pack_100",
                IdOfProduct = "money_pack_100",
                IsIAPProduct = false,
                Name = "Money Pack 100",
                TypeOfProduct = App.Store.TypeOfProduct.Consumable,
            });

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;
    }

    public static async UniTask GetStoreData(CancellationToken token)
    {
        if (instance == default) return;

        await instance.getStoreData(token);

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// Get Player Shop Status => 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask getPlayerStore(CancellationToken token)
    {
        try
        {
            //await Client.Send

            //FakeData
            UserData.Instance.Drop<UserShopData>();
            UserData.Instance.Insert(new UserShopData()
            {
                Id = "1",
                IdOfProduct = "Video_Ads",
                IsAvai = true,
            });
            UserData.Instance.Insert(new UserShopData()
            {
                Id = "1",
                IdOfProduct = "money_pack_100",
                IsAvai = true,
            });

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;
    }

    public static async UniTask GetPlayerStore(CancellationToken token)
    {
        if (instance == default) return;

        await instance.getPlayerStore(token);

        await UniTask.CompletedTask;
    }
}
