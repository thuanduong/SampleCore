using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShopPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private LoadingPresenter loadingPresenter;
    private LoadingPresenter LoadingPresenter => loadingPresenter ??= Container.Inject<LoadingPresenter>();


    private IAPPresenter iapPresenter;

    private string IAPProductCatalog;
    private Core.Store.StoreDatabase store;
    private bool isInit = false;

    public ShopPresenter(IDIContainer container)
    {
        this.Container = container;
        iapPresenter = new IAPPresenter(this.Container);
        iapPresenter.UseFakeStore = true;
        isInit = false;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;

        iapPresenter.Dispose();
        iapPresenter = default;
    }

    public async UniTask FetchData()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        if (isInit) return;
        isInit = true;

        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        await operation.WithCancellation(cts.Token);
        if (operation.isDone && operation.asset != default)
        {
            IAPProductCatalog = (operation.asset as TextAsset).text;
        }
        store = await ObjectLoader.LoadObject<Core.Store.StoreDatabase>("Object/Store/StoreDatabase", cts.Token);

        Debug.Log(IAPProductCatalog);
        Debug.Log(store != default);

        await iapPresenter.InitAsync();
        iapPresenter.HandleIAPCatalogLoaded(IAPProductCatalog);
        await StoreService.GetPlayerStore(cts.Token);

    }

    public string GetProductPrice(string id)
    {
        return "Free";
    }

    public int GetProductPriceInt(string id)
    {
        return 0;
    }

    public void BuyProduct(string idOfProduct)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        BuyProductAsync(idOfProduct).Forget();
    }

    private async UniTask BuyProductAsync(string idOfProduct)
    {
        await LoadingPresenter.ActiveWaiting(true);
        bool wait = false;
        iapPresenter.Purchase(idOfProduct, () => { wait = true; });
        await UniTask.WaitUntil(() => wait == false);
        await LoadingPresenter.ActiveWaiting(false);
    }
}
