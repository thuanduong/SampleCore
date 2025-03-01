using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class UIMainMenuPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private UserGameMode[] gameModes;
    private UserGameMode[] cashGames;

    private Sprite[] sprites;
        
    public UIMainMenu uiLobby;

    GameModePresenter gameMode;
    GameModePresenter GameMode => gameMode ??= Container.Inject<GameModePresenter>();

    public UIMainMenuPresenter(IDIContainer container)
    {
        Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
        UILoader.SafeRelease(ref uiLobby);
        sprites = default;
        gameModes = default;
        cashGames = default;
    }

    public async UniTask ShowMainMenuAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await FetchData();
        uiLobby ??= await UILoader.Instantiate<UIMainMenu>(token: cts.Token);
        await UniTask.Delay(100);
        uiLobby.SetEntity(new UIMainMenu.Entity()
        {
            CashGameList = new UISliderView.Entity()
            {
                entities = await LoadGameModeCash(),
                StartPageIndex = 0,
            }
        });
        await uiLobby.In();
    }

    private async UniTask TransitionToAsync(Action action)
    {
        await uiLobby.Out();
        action();
    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await uiLobby.In();
    }

    public async UniTask HideAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await uiLobby.Out();
    }

    private async UniTask FetchData()
    {
        gameModes = UserData.Instance.GetAll<UserGameMode>().ToArray();
        cashGames = gameModes.OrderBy(x => x.Mode.Order).ToArray();//gameModes.Where(x => x.Mode.GameType == TypeOfGameMode.CASH).OrderBy(x=>x.Mode.Order).ToArray();
        await UniTask.CompletedTask;
    }

    private async UniTask<UISliderItem.Entity[]> LoadGameModeCash()
    {
        var el = new List<UIGameModeCashItem.CashEntity>();
        for (int i = 0; i < cashGames.Length; i++)
        {
            var item = cashGames[i];
            var index = i;
            el.Add(new UIGameModeCashItem.CashEntity()
            {
                button = new ButtonEntity(()=>OnButtonGameModeCashClick(index).Forget(), isInteractable: item.IsUnlock),
                Icon = new UIImageComponent.Entity() { sprite = await SpriteLoader.LoadSprite("Image/GameMode/Mode_Cash_Red", token: cts.Token) },
                BuyIn = $"{item.Mode.MoneyMin}-{item.Mode.MoneyMax}",
            });
        }
        return el.ToArray();
    }

    private async UniTask OnButtonGameModeCashClick(int index)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        var item = cashGames[index];
        await GameMode.ShowPopupBuyIn(item);
        await UniTask.CompletedTask;
    }

}
