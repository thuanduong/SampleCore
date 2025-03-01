using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShopState : InjectedBState
{
    private LoadingPresenter uiLoadingPresenter;
    private LoadingPresenter UiLoadingPresenter => uiLoadingPresenter ??= this.Container.Inject<LoadingPresenter>();

    private UIShopPresenter uiShopPresenter;
    private BackgroundPresenter backgroundPresenter;
    private UIHeaderPresenter uiHeaderPresenter;
    private UIHeaderPresenter UIHeaderPresenter => uiHeaderPresenter ??= Container.Inject<UIHeaderPresenter>();

    private CancellationTokenSource cts;

    public override void Enter()
    {
        base.Enter();
        ShowBackGroundAsync().Forget();
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        uiShopPresenter ??= new UIShopPresenter(this.Container);
        backgroundPresenter = Container.Inject<BackgroundPresenter>();
        uiShopPresenter.ShowAsync().Forget();
        SubcribeEvents();
    }

    public override void Exit()
    {
        base.Exit();
        UnSubcribeEvents();
        HideBackgroundAsync().Forget();
        uiShopPresenter.Dispose();
        uiShopPresenter = default;
        uiLoadingPresenter = default;
        backgroundPresenter = default;
        uiHeaderPresenter = default;
        cts.SafeCancelAndDispose();
        cts = default;

    }

    private async UniTask ShowBackGroundAsync()
    {
        await backgroundPresenter.ChangeBackgroundImage("ImageBG/Background_Shop");
        UiLoadingPresenter.HideLoading();
    }

    private async UniTask HideBackgroundAsync()
    {
        await backgroundPresenter.HideBackground();
        UiLoadingPresenter.HideLoading();
    }


    private void SubcribeEvents()
    {
        
    }

    private void UnSubcribeEvents()
    {

    }

    private void ToMainMenuState()
    {
        //backgroundPresenter.FLAG_NO_CHANGE = true;
        uiHeaderPresenter.FLAG_NO_CHANGE = true;
        this.Machine.ChangeState<MainMenuState>();
    }


}
