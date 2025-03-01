using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;
public class UIShopPresenter : IDisposable
{
    private IDIContainer Container { get; }
    public UIShop uiShop;
    private CancellationTokenSource cts;

    private ShopPresenter shopPresenter;
    private ShopPresenter ShopPresenter => shopPresenter ??= Container.Inject<ShopPresenter>();

    private MasterShopData[] listMasterShop;

    public UIShopPresenter(IDIContainer container)
    {
        Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
        UILoader.SafeRelease(ref uiShop);

    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await FetchData();
        uiShop ??= await UILoader.Instantiate<UIShop>(token: cts.Token);
        uiShop.SetEntity(new UIShop.Entity()
        {
            listFreePack = new UIShopListCollectItem.Entity() { entities = await LoadFreePack() },
            listMoneyPack = new UIShopListConsumableItem.Entity() { entities = await LoadMoneyPack() },
        });
        await uiShop.In();
    }

    public async UniTask HideAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await uiShop.Out();
    }

    private async UniTask TransitionToAsync(Action action)
    {
        await uiShop.Out();
        action();
    }

    private async UniTask FetchData()
    {
        await ShopPresenter.FetchData();
        listMasterShop = MasterData.Instance.GetAll<MasterShopData>().ToArray();
    }

    private async UniTask<UIShopCollectItem.Entity[]> LoadFreePack()
    {
        List<UIShopCollectItem.Entity> ml = new List<UIShopCollectItem.Entity>();
        var lF = listMasterShop.Where(x => x.TypeOfProduct == App.Store.TypeOfProduct.VideoAds || x.TypeOfProduct == App.Store.TypeOfProduct.Collector).ToList();
        for (int i = 0; i < lF.Count; i++)
        {
            UIShopCollectItem.Entity e = new UIShopCollectItem.Entity()
            {
                button = new ButtonEntity(),
                imageBG = new UIImageComponent.Entity(),
            };
            ml.Add(e);
        }

        return ml.ToArray();
    }

    private async UniTask<UIShopConsumableItem.Entity[]> LoadMoneyPack()
    {
        List<UIShopConsumableItem.Entity> ml = new List<UIShopConsumableItem.Entity>();
        var lF = listMasterShop.Where(x => x.TypeOfProduct == App.Store.TypeOfProduct.Consumable).ToList();
        for (int i = 0; i < lF.Count; i++)
        {
            int index = i;
            var item = lF[i];
            var price = ShopPresenter.GetProductPrice(item.IdOfProduct);
            UIShopConsumableItem.Entity e = new UIShopConsumableItem.Entity()
            {
                btnOpen = new ButtonEntity(()=>OnPurchaseProduct(item.IdOfProduct)),
                image = new UIImageComponent.Entity(),
                price = ShopPresenter.GetProductPrice(item.IdOfProduct),
                title = item.Name,
            };
            ml.Add(e);
        }

        return ml.ToArray();
    }

    private void OnPurchaseProduct(string id)
    {
        shopPresenter.BuyProduct(id);
    }
}
