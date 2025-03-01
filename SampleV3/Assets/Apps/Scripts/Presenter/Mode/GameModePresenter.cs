using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;


public class GameModePresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;
    
    private UIPopupBuyIn popupBuyIn;
    private UserGameMode userMode;
    private float sf;

    public event Action ToPlayState = ActionUtility.EmptyAction.Instance;
    public event Action OnCancel = ActionUtility.EmptyAction.Instance;

    private UIFindingMatchPresenter finding;

    private UIHeaderPresenter uiHeaderPresenter;
    private UIHeaderPresenter Header => uiHeaderPresenter ??= Container.Inject<UIHeaderPresenter>();

    private UIBottomMenuPresenter bottom;
    private UIBottomMenuPresenter Bottom => bottom ??= Container.Inject<UIBottomMenuPresenter>();

    private bool isShowingHeader, isShowingBottom;

    public GameModePresenter(IDIContainer container)
    {
        this.Container = container;
        finding = new UIFindingMatchPresenter(this.Container);
        SubcribeEvents();
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;

        UnSubcribeEvents();

        UILoader.SafeRelease(ref popupBuyIn);
        userMode = default;

        finding.Dispose();
        finding = default;
        uiHeaderPresenter = default;
        bottom = default;
    }

    private void SubcribeEvents()
    {
        finding.OnBack += OnFindingBack;
        finding.ProceedToMatch += ToGamePlay;
    }

    private void UnSubcribeEvents()
    {
        finding.OnBack -= OnFindingBack;
        finding.ProceedToMatch -= ToGamePlay;
    }

    public async UniTask FetchData()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await UserService.GetGameModeData(cts.Token);
        await UserService.GetStatusGameModeData(cts.Token);
    }

    public async UniTask ShowPopupBuyIn(UserGameMode mode)
    {
        bool wait = true;
        bool kq = false;
        var userMoney = UserService.Instance.Current.Money;
        userMode = mode;

        if (kq)
        {
            kq = await GameService.Instance.Join(mode.IdOfGameMode, cts.Token);
            if (kq)
                ShowFindingMatch().Forget();
        }
    }

    private void OnProcessValueChange(float f)
    {
        if (userMode == default || popupBuyIn == default) return;
        var _f = (float)Math.Round(f, 2);
        var Mode = userMode.Mode;
        var Value = Mathf.Lerp(Mode.MoneyMin, Mode.MoneyMax, _f);
        popupBuyIn.SetBuyIn(Value);
    }

    private void OnAddMoreMoney()
    {
        if (userMode == default || popupBuyIn == default) return;
        var f = popupBuyIn.entity.Process.progress;
        f = Mathf.Clamp01(f + sf);
        popupBuyIn.Process.SetPercent(f);
    }

    private void OnSubMoreMoney()
    {
        if (userMode == default || popupBuyIn == default) return;
        var f = popupBuyIn.entity.Process.progress;
        f = Mathf.Clamp01(f - sf);
        popupBuyIn.Process.SetPercent(f);
    }
        
    private async UniTask ShowFindingMatch()
    {
        await HandleHeaderAndBottomIfNeeded(false);
        await finding.ShowAsync().AttachExternalCancellation(cts.Token);
        await finding.FindMatch().AttachExternalCancellation(cts.Token);
    }

    private void OnFindingBack()
    {
        OnFindingBackAsync().Forget();
    }

    private async UniTask OnFindingBackAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await GameService.Instance.CancelFinding(cts.Token);
        await finding.HideAsync().AttachExternalCancellation(cts.Token);
        await HandleHeaderAndBottomIfNeeded(true);
        OnCancel();
    }

    private void ToGamePlay()
    {
        finding.HideAsync().Forget();
        ToPlayState();
    }

    private async UniTask HandleHeaderAndBottomIfNeeded(bool active)
    {
        if (active)
        {
            if (isShowingHeader && isShowingBottom)
                await UniTask.WhenAll(Header.ShowHeaderAsync(), Bottom.ShowAsync()).AttachExternalCancellation(cts.Token);
            else if (isShowingHeader)
                await Header.ShowHeaderAsync();
            else if (isShowingBottom)
                await Bottom.ShowAsync();
        }
        else
        {
            isShowingHeader = Header.IsShow;
            isShowingBottom = Bottom.IsShow;
            await UniTask.WhenAll(Header.HideHeaderAsync(), Bottom.HideAsync());
        }
    }
}
