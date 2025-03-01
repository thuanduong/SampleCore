using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;


public class GamePokerState : InjectedBState
{
    private LoadingPresenter uiLoadingPresenter;
    private LoadingPresenter UiLoadingPresenter => uiLoadingPresenter ??= this.Container.Inject<LoadingPresenter>();

    private BackgroundPresenter backgroundPresenter;
    public BackgroundPresenter BackgroundPresenter => backgroundPresenter ??= this.Container.Inject<BackgroundPresenter>();

    private AudioPresenter audioPresenter;
    private AudioPresenter AudioPresenter => audioPresenter ??= Container.Inject<AudioPresenter>();

    private CancellationTokenSource cts;

    private PokerPresenter poker;

    public override void Enter()
    {
        base.Enter();
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        backgroundPresenter = Container.Inject<BackgroundPresenter>();

        poker = new PokerPresenter(Container);

        SubcribeEvents();

        if (backgroundPresenter.FLAG_NO_CHANGE == false)
        {
            HideBackgroundAsync().Forget();
        }
        else
            backgroundPresenter.FLAG_NO_CHANGE = false;

        ShowAsync().AttachExternalCancellation(cts.Token).Forget();
        AudioPresenter.PlayMusic("MainTheme");
    }

    public override void Exit()
    {
        base.Exit();
        UnSubcribeEvents();
        uiLoadingPresenter = default;
        poker.Dispose();
        poker = default;

        cts.SafeCancelAndDispose();
        cts = default;
    }

    private void SubcribeEvents()
    {
    }

    private void UnSubcribeEvents()
    {
    }

    private async UniTask ShowBackGroundAsync()
    {
        Debug.Log("Load Background");
        await backgroundPresenter.LoadBackground("Image/BG_MainMenu", FromAsset: true);
        UiLoadingPresenter.HideLoading();
    }

    private async UniTask HideBackgroundAsync()
    {
        await backgroundPresenter.HideBackground();
        UiLoadingPresenter.HideLoading();
    }

    private async UniTask ShowAsync()
    {
        await ShowBackGroundAsync();
        await poker.ShowAsync();
        await UniTask.Delay(100);
        await HideBackgroundAsync();
    }

    private void ToMainMenuState()
    {
        this.Machine.ChangeState<MainMenuState>();
    }


}
