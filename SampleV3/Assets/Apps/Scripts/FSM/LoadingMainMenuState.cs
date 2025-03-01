using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

public class LoadingMainMenuState : InjectedBState
{
    private CancellationTokenSource cts;
    BackgroundPresenter backgroundPresenter;
    private UILoadingMainMenu uiLoadingMainMenuState;

    LeaderboardPresenter leaderboard;
    LeaderboardPresenter Leaderboard => leaderboard ??= Container.Inject<LeaderboardPresenter>();

    GameModePresenter gameMode;
    GameModePresenter GameMode => gameMode ??= Container.Inject<GameModePresenter>();

    public override void Enter()
    {
        base.Enter();
        ShowLoadingThenChangeState().Forget();
    }

    private async UniTaskVoid ShowLoadingThenChangeState()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        var uiLoadingPresenter = this.Container.Inject<LoadingMainMenuPresenter>();
        backgroundPresenter = Container.Inject<BackgroundPresenter>();
        await backgroundPresenter.LoadBackground("Image/Background_Loading");
        uiLoadingPresenter.ShowLoadingAsync().Forget();
        await UniTask.WhenAll(LoadData(), UniTask.Delay(4000)).AttachExternalCancellation(cts.Token);
        uiLoadingPresenter.HideLoading();
        this.Machine.ChangeState<MainMenuState>();
       
    }

    private async UniTask LoadData()
    {
        await StoreService.GetStoreData(cts.Token);
        await Leaderboard.FetchData();
        await Leaderboard.FetchUserRankData();
        await GameMode.FetchData();
    }
   
    public override void Exit()
    {
        base.Exit();
        cts.SafeCancelAndDispose();
        cts = default;

        leaderboard = default;
    }
}