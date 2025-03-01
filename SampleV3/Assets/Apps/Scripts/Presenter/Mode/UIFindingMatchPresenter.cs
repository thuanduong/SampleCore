using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core.Model;
using Cysharp.Threading.Tasks;

public class UIFindingMatchPresenter : IDisposable
{
    private UIWaiting uiWaiting;
    private CancellationTokenSource cts;
    private IDIContainer Container { get; }

    public event Action OnBack = ActionUtility.EmptyAction.Instance;
    public event Action ProceedToMatch = ActionUtility.EmptyAction.Instance;

    LoadingPresenter loadingPresenter;
    LoadingPresenter LoadingPresenter => loadingPresenter ??= Container.Inject<LoadingPresenter>();

    public UIFindingMatchPresenter(IDIContainer container)
    {
        Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
        UILoader.SafeRelease(ref uiWaiting);
    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        uiWaiting ??= await UILoader.Instantiate<UIWaiting>(canvasType: UICanvas.UICanvasType.Loading, token: cts.Token);
        uiWaiting.SetEntity(new UIWaiting.Entity()
        {
            //time = new UIComponentCountTimer.Entity() { Time = 0 },
            btnOnBack = new ButtonEntity(() => OnBack()),

        });
        await uiWaiting.In();
    }

    public async UniTask HideAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await uiWaiting.Out();
    }

    public async UniTask FindMatch()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await UniTask.Delay(5000, cancellationToken: cts.Token);
        ProceedToMatch();
    }
}
