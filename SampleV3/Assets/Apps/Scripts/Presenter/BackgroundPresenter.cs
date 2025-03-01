using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class BackgroundPresenter : IDisposable
{

    private readonly IDIContainer container;
    private CancellationTokenSource cts;

    UIBackground background;
    public bool FLAG_NO_CHANGE { get; set; }

    public BackgroundPresenter(IDIContainer container)
    {
        this.container = container;
    }

    public void Dispose()
    {

    }

    public async UniTask LoadBackground(string path, bool FromAsset = false)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        background ??= await UILoader.Instantiate<UIBackground>(canvasType : UICanvas.UICanvasType.BackGround, token: cts.Token);
        Sprite s = null;
        if (FromAsset)
        {
            s = await SpriteLoader.LoadSprite(path, token: cts.Token);
        }
        else
        {
            await loadBackgroundSprite(path, (i) => s = i);
        }
        var entity = new UIBackground.Entity()
        {
            sprite = s
        };
        background.SetEntity(entity);
        await background.In();
    }

    public async UniTask ChangeBackgroundImage(string newPath)
    {
        Sprite newSprite = null;
        await loadBackgroundSprite(newPath, (sprite) => newSprite = sprite);
        var entity = new UIBackground.Entity()
        {
            sprite = newSprite
        };
        background.SetEntity(entity);
    }

    private async UniTask loadBackgroundSprite(string path, Action<Sprite> callback)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<Sprite>(path);
        await resourceRequest.ToUniTask();
        Sprite loadedSprite = resourceRequest.asset as Sprite;
        callback?.Invoke(loadedSprite);
    }

    public async UniTask HideBackground()
    {
        await background.Out();
    }

   
}
