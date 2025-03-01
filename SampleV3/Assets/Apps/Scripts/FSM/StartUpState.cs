using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StartUpState : InjectedBHState
{
    private bool isNeedResetState = false;
    private StartUpStatePresenter startUpStateHandler;
    private CancellationTokenSource cts;

    
    public override void Enter()
    {
        OnEnterStateAsync().Forget();
    }

    private async UniTaskVoid OnEnterStateAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        //this.Container.Bind(await MasterLoader.LoadMasterAsync<MasterLocalizeContainer>());
        Core.Model.MasterData.Instance.RegistModelData<MasterErrorCode>();
        await LoadErrorCode(cts.Token);

        startUpStateHandler = new StartUpStatePresenter();
        startUpStateHandler.OnReboot += OnReboot;
        await startUpStateHandler.ShowClientInfoAsync();
        Container.Bind(startUpStateHandler);
        

        base.Enter();
    }

    public override void AddStates()
    {
        base.AddStates();
        this.AddState<InitialState>();
        //this.AddState<LoginState>();
        this.AddState<DownloadAssetState>();
        this.SetInitialState<DownloadAssetState>();
    }

    private void OnReboot()
    {
        isNeedResetState = true;
        this.Machine.CurrentState.Exit();
    }

    public override void Exit()
    {
        try
        {
            base.Exit();
            cts.SafeCancelAndDispose();
            cts = default;

            startUpStateHandler.OnReboot -= OnReboot;
            Container.RemoveAndDisposeIfNeed<StartUpStatePresenter>();
            //Container.RemoveAndDisposeIfNeed<MasterLocalizeContainer>();
            //MasterLoader.Unload<MasterLocalizeContainer>();

            Core.Model.MasterData.Instance.Drop<MasterErrorCode>();
        }
        finally
        {
            this.Machine.RemoveAllStates();
            if (isNeedResetState)
            {
                ((MonoFSMContainer)this.Machine).Reset();
                this.Machine.Initialize();
            }
            isNeedResetState = false;
        }
    }

    private async UniTask LoadErrorCode(CancellationToken token)
    {
        
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("Data/MasterErrorCode");
        await operation.WithCancellation(token);
        if (operation.isDone && operation.asset != default)
        {
            var s = (operation.asset as TextAsset).text;
            var x = new MasterErrorCodeEmbed();
            x.ParseFromJson(s);
            foreach (var item in x.data.ToArray())
            {
                Core.Model.MasterData.Instance.Insert(item);
            }
        }
    }
}