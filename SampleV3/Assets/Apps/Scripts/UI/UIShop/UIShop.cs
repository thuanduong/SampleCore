using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;


public class UIShop : PopupEntity<UIShop.Entity>
{
    public class Entity {
        public UIShopListCollectItem.Entity listFreePack;
        public UIShopListConsumableItem.Entity listMoneyPack;
    }

    public UIShopListCollectItem listFreePack;
    public UIShopListConsumableItem listMoneyPack;
    public ContentSizeFitter filter;
    public CanvasGroup filterGroup;

    private CancellationTokenSource cts;

    private void OnDestroy()
    {
        cts.SafeCancelAndDispose();
        cts = default;
    }

    protected override void OnSetEntity()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        listFreePack.SetEntity(entity.listFreePack);
        listMoneyPack.SetEntity(entity.listMoneyPack);
        DelayPanel().Forget();
    }

    private async UniTask DelayPanel()
    {
        filter.enabled = false;
        filterGroup.alpha = 0.0f;
        await UniTask.Delay(100, cancellationToken: cts.Token);
        filter.enabled = true;
        filterGroup.alpha = 1.0f;
    }
}
