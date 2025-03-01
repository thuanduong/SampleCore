using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Core.Model;

public class InitialState : InjectedBHState, IDisposable
{
    private UIHeaderPresenter uiHeaderPresenter;
    private StartUpStatePresenter StartUpStatePresenter => Container.Inject<StartUpStatePresenter>();
    public override void Enter()
    {
        OnEnterStateAsync().Forget();
    }

    private async UniTaskVoid OnEnterStateAsync()
    {
        //Presenter
        this.Container.Bind(new AudioPresenter(Container));
        this.Container.Bind(new LoadingPresenter());
        this.Container.Bind(new LoadingMainMenuPresenter());
        this.Container.Bind(new BackgroundPresenter(Container));
        
        uiHeaderPresenter = new UIHeaderPresenter(Container);
        this.Container.Bind(uiHeaderPresenter);
        this.Container.Bind(new UIBottomMenuPresenter(Container));
        this.Container.Bind(new ShopPresenter(Container));
        this.Container.Bind(new QuestPresenter(Container));
        this.Container.Bind(new FriendPresenter(Container));
        this.Container.Bind(new LeaderboardPresenter(Container));
        this.Container.Bind(new GameModePresenter(Container));


        //Service
        this.Container.Bind(PingService.Instantiate(Container));
        this.Container.Bind(LoginService.Instantiate(Container));
        this.Container.Bind(StoreService.Instantiate(Container));
        this.Container.Bind(QuestService.Instantiate(Container));
        this.Container.Bind(FriendService.Instantiate(Container));
        this.Container.Bind(LeaderboardService.Instantiate(Container));
        this.Container.Bind(UserService.Instantiate(Container));
        this.Container.Bind(GameService.Instantiate(Container));

        //Data
        MasterData.Instance.RegistModelData<MasterShopData>();
        MasterData.Instance.RegistModelData<MasterQuestChallenge>();
        MasterData.Instance.RegistModelData<MasterQuestReward>();
        MasterData.Instance.RegistModelData<MasterSeason>();
        MasterData.Instance.RegistModelData<MasterGameMode>();


        UserData.Instance.RegistModelData<UserProfileModel>();
        UserData.Instance.Insert(new UserProfileModel());

        UserData.Instance.RegistModelData<UserShopData>();
        UserData.Instance.RegistModelData<UserQuest>();
        UserData.Instance.RegistModelData<UserQuestChallenge>();
        UserData.Instance.RegistModelData<UserQuestReward>();
        UserData.Instance.RegistModelData<UserFriend>();
        UserData.Instance.RegistModelData<PlayerSeasonRank>();
        UserData.Instance.RegistModelData<UserSeasonRank>();
        UserData.Instance.RegistModelData<UserGameMode>();


        var masterErrorCodes = MasterData.Instance.GetAll<MasterErrorCode>();
        this.Container.Bind(TCPSocketClient.Initialize(new ProtobufMessageParser(), ErrorCodeConfiguration.Initialize(masterErrorCodes.ToArray())));

        uiHeaderPresenter.OnLogOut += OnLogOut;

        base.Enter();
        await UniTask.CompletedTask;
    }

    private void OnLogOut()
    {
        var audio = Container.Inject<AudioPresenter>();
        audio.StopMusic();
        StartUpStatePresenter.Reboot();
        Debug.Log("LOG OUT");
    }

    public override void AddStates()
    {
        base.AddStates();
        AddState<LoadingState>();
        AddState<LoginState>();
        AddState<LoadingMainMenuState>();
        AddState<MainMenuState>();
        AddState<ShopState>();
        AddState<GamePokerState>();
        SetInitialState<LoadingState>();
    }

    public override void Exit()
    {
        base.Exit();
        Dispose();
    }

    public void Dispose()
    {
        uiHeaderPresenter.OnLogOut -= OnLogOut;
        
        this.Container.RemoveAndDisposeIfNeed<PingService>();
        this.Container.RemoveAndDisposeIfNeed<LoginService>();
        this.Container.RemoveAndDisposeIfNeed<StoreService>();
        this.Container.RemoveAndDisposeIfNeed<QuestService>();
        this.Container.RemoveAndDisposeIfNeed<FriendService>();
        this.Container.RemoveAndDisposeIfNeed<LeaderboardService>();
        this.Container.RemoveAndDisposeIfNeed<UserService>();
        this.Container.RemoveAndDisposeIfNeed<GameService>();

        this.Container.RemoveAndDisposeIfNeed<AudioPresenter>();
        this.Container.RemoveAndDisposeIfNeed<UIHeaderPresenter>();
        this.Container.RemoveAndDisposeIfNeed<UIBottomMenuPresenter>();
        this.Container.RemoveAndDisposeIfNeed<BackgroundPresenter>();
        this.Container.RemoveAndDisposeIfNeed<LoadingPresenter>();
        this.Container.RemoveAndDisposeIfNeed<LoadingMainMenuPresenter>();
        this.Container.RemoveAndDisposeIfNeed<ShopPresenter>();
        this.Container.RemoveAndDisposeIfNeed<QuestPresenter>();
        this.Container.RemoveAndDisposeIfNeed<FriendPresenter>();
        this.Container.RemoveAndDisposeIfNeed<LeaderboardPresenter>();
        this.Container.RemoveAndDisposeIfNeed<GameModePresenter>();

        UserData.Instance.Drop<UserProfileModel>();
        UserData.Instance.Drop<UserShopData>();
        UserData.Instance.Drop<UserQuestChallenge>();
        UserData.Instance.Drop<UserQuestReward>();
        UserData.Instance.Drop<UserQuest>();
        UserData.Instance.Drop<UserFriend>();
        UserData.Instance.Drop<PlayerSeasonRank>();
        UserData.Instance.Drop<UserSeasonRank>();
        UserData.Instance.Drop<UserGameMode>();

        UserData.Instance.ClearData();

        MasterData.Instance.Drop<MasterShopData>();
        MasterData.Instance.Drop<MasterQuestChallenge>();
        MasterData.Instance.Drop<MasterQuestReward>();
        MasterData.Instance.Drop<MasterSeason>();
        MasterData.Instance.Drop<MasterGameMode>();
        MasterData.Instance.ClearData();

        uiHeaderPresenter = default;
    }
}