using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;


public class MainMenuState : InjectedBState
{
    private LoadingPresenter uiLoadingPresenter;
    private LoadingPresenter UiLoadingPresenter => uiLoadingPresenter ??= this.Container.Inject<LoadingPresenter>();

    private UIMainMenuPresenter uiMainMenuPresenter;
    private UIShopPresenter uiShopPresenter;
    private UIQuestPresenter uiQuestPresenter;
    private UILeaderboardPresenter uiLeaderboardPresenter;

    private UIFriendPresenter uiFriendPresener;

    private BackgroundPresenter backgroundPresenter;
    private BackgroundPresenter BackgroundPresenter => backgroundPresenter ??= Container.Inject<BackgroundPresenter>();


    private UIHeaderPresenter uiHeaderPresenter;
    private UIHeaderPresenter UIHeaderPresenter => uiHeaderPresenter ??= Container.Inject<UIHeaderPresenter>();

    private UIBottomMenuPresenter bottom;
    private UIBottomMenuPresenter Bottom => bottom ??= Container.Inject<UIBottomMenuPresenter>();


    private AudioPresenter audioPresenter;
    private AudioPresenter AudioPresenter => audioPresenter ??= Container.Inject<AudioPresenter>();

    private GameModePresenter gameMode;
    private GameModePresenter GameMode => gameMode ??= Container.Inject<GameModePresenter>();

    private enum TAB
    {
        PLAY = 0,
        SHOP = 1,
        QUEST = 2,
        LEADERBOARD = 3,
        CARD = 4,
        FRIEND = 5,
    }
    
    private CancellationTokenSource cts;
    private TAB tab = TAB.PLAY;


    public override void Enter()
    {
        base.Enter();
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        
        uiMainMenuPresenter ??= new UIMainMenuPresenter(this.Container);
        uiShopPresenter ??= new UIShopPresenter(this.Container);
        uiQuestPresenter ??= new UIQuestPresenter(this.Container);
        uiLeaderboardPresenter ??= new UILeaderboardPresenter(this.Container);
        uiFriendPresener ??= new UIFriendPresenter(this.Container);

        SubcribeEvents();

        if (BackgroundPresenter.FLAG_NO_CHANGE == false)
        {
            HideBackgroundAsync().Forget();
        }
        else
            BackgroundPresenter.FLAG_NO_CHANGE = false;


        OnStartState().Forget();
        ShowBackGroundAsync().Forget();
        AudioPresenter.PlayMusic("MainTheme");
    }

    public override void Exit()
    {
        base.Exit();
        UnSubcribeEvents();

        uiMainMenuPresenter.Dispose();
        uiMainMenuPresenter = default;
        uiFriendPresener.Dispose();
        uiFriendPresener = default;
        uiShopPresenter.Dispose();
        uiShopPresenter = default;
        uiQuestPresenter.Dispose();
        uiQuestPresenter = default;
        uiLeaderboardPresenter.Dispose();
        uiLeaderboardPresenter = default;


        uiLoadingPresenter = default;
        uiHeaderPresenter = default;
        bottom = default;
        gameMode = default;

        cts.SafeCancelAndDispose();
        cts = default;
    }

    private async UniTask ShowBackGroundAsync()
    {
        await backgroundPresenter.LoadBackground("Image/BG_MainMenu", FromAsset: true);
        UiLoadingPresenter.HideLoading();
    }

    private async UniTask HideBackgroundAsync()
    {
        await backgroundPresenter.HideBackground();
        UiLoadingPresenter.HideLoading();
    }

    private void SubcribeEvents()
    {
        GameMode.ToPlayState += ToPlayState;
        UIHeaderPresenter.OnShowFriend += ToFriendTab;
        uiFriendPresener.OnClose += HideFriend;

        Bottom.OnButtonPlayClicked += ToPlayTab;
        Bottom.OnButtonShopClicked += ToShopTab;
        Bottom.OnButtonQuestClicked += ToQuestTab;
        Bottom.OnButtonLeaderboardClicked += ToLeaderboardTab;
        Bottom.OnButtonCardClicked += ToCardTab;
    }

    private void UnSubcribeEvents()
    {
        GameMode.ToPlayState -= ToPlayState;
        UIHeaderPresenter.OnShowFriend -= ToFriendTab;
        uiFriendPresener.OnClose -= HideFriend;

        Bottom.OnButtonPlayClicked -= ToPlayTab;
        Bottom.OnButtonShopClicked -= ToShopTab;
        Bottom.OnButtonQuestClicked -= ToQuestTab;
        Bottom.OnButtonLeaderboardClicked -= ToLeaderboardTab;
        Bottom.OnButtonCardClicked -= ToCardTab;
    }

    private void ToPlayState()
    {
        UIHeaderPresenter.HideHeader();
        Bottom.HideAsync().Forget();
        this.Machine.ChangeState<GamePokerState>();
    }

    private void ToPlayTab()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        ToPlayTabAsync().AttachExternalCancellation(cts.Token).Forget();
    }
    private void ToShopTab()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        ToShopTabAsync().AttachExternalCancellation(cts.Token).Forget();
    }
    private void ToQuestTab()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        ToQuestTabAsync().AttachExternalCancellation(cts.Token).Forget();
    }
    private void ToLeaderboardTab()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        ToLeaderboardTabAsync().AttachExternalCancellation(cts.Token).Forget();
    }
    private void ToCardTab()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        ToCardTabAsync().AttachExternalCancellation(cts.Token).Forget();
    }
    private void ToFriendTab()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        ToFriendTabAsync().AttachExternalCancellation(cts.Token).Forget();
    }
    private void HideFriend()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        HideFriendAsync().AttachExternalCancellation(cts.Token).Forget();
    }

    private async UniTask OnStartState()
    {
        tab = TAB.PLAY;
        await UniTask.WhenAll(uiMainMenuPresenter.ShowMainMenuAsync(), UIHeaderPresenter.ShowHeaderAsync(), Bottom.ShowAsync()).AttachExternalCancellation(cts.Token);
    }

    private async UniTask ToPlayTabAsync()
    {
        await HideCurrentTab();
        tab = TAB.PLAY;
        await UniTask.WhenAll(uiMainMenuPresenter.ShowAsync(), UIHeaderPresenter.ShowHeaderAsync());
    }

    private async UniTask ToShopTabAsync()
    {
        await HideCurrentTab();
        tab = TAB.SHOP;
        await UniTask.WhenAll(uiShopPresenter.ShowAsync(), UIHeaderPresenter.ShowHeaderAsync());
    }

    private async UniTask ToQuestTabAsync()
    {
        await HideCurrentTab();
        tab = TAB.QUEST;
        await UniTask.WhenAll(uiQuestPresenter.ShowAsync(), UIHeaderPresenter.ShowHeaderAsync());
    }

    private async UniTask ToLeaderboardTabAsync()
    {
        await HideCurrentTab();
        tab = TAB.LEADERBOARD;
        await UniTask.WhenAll(uiLeaderboardPresenter.ShowAsync(), UIHeaderPresenter.HideHeaderAsync());
    }

    private async UniTask ToCardTabAsync()
    {
        await HideCurrentTab();
        tab = TAB.CARD;
        
    }

    private async UniTask ToFriendTabAsync()
    {
        await HideCurrentTab();
        await UniTask.WhenAll(uiFriendPresener.ShowAsync(), UIHeaderPresenter.HideHeaderAsync());
    }

    private async UniTask HideCurrentTab()
    {
        switch (tab)
        {
            case TAB.PLAY:
                await uiMainMenuPresenter.HideAsync();
                break;
            case TAB.SHOP:
                await uiShopPresenter.HideAsync();
                break;
            case TAB.QUEST:
                await uiQuestPresenter.HideAsync();
                break;
            case TAB.LEADERBOARD:
                await uiLeaderboardPresenter.HideAsync();
                break;
            case TAB.CARD:
                
                break;
            case TAB.FRIEND:
                await uiFriendPresener.HideAsync().AttachExternalCancellation(cts.Token);
                break;
        }
    }

    private async UniTask ShowCurrentTab()
    {
        switch (tab)
        {
            case TAB.PLAY:
                await ToPlayTabAsync();
                break;
            case TAB.SHOP:
                await ToShopTabAsync();
                break;
            case TAB.QUEST:
                await ToQuestTabAsync();
                break;
            case TAB.LEADERBOARD:
                await ToLeaderboardTabAsync();
                break;
            case TAB.CARD:

                break;
            case TAB.FRIEND:
                break;
        }
    }

    private async UniTask HideFriendAsync()
    {
        await uiFriendPresener.HideAsync().AttachExternalCancellation(cts.Token);
        await ShowCurrentTab();
    }
}
