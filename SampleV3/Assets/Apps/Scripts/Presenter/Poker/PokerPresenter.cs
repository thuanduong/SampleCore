using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Core.Model;


public class PokerPresenter : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;

    private Scene gameSceneAsset;
    private string _gameScenePath;
    private GameObject camera;

    public PokerPresenter(IDIContainer container)
    {
        this.container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;

        if (gameSceneAsset != default)
        {
            SceneAssetLoader.UnloadAssetAtPath(_gameScenePath);
            gameSceneAsset = default;
        }
    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        await LoadScene().AttachExternalCancellation(cts.Token);
        await UniTask.CompletedTask;
    }

    public async UniTask HideAsync()
    {
        if (camera != default)
        {
            camera.SetActive(true);
        }
        await UniTask.CompletedTask;
    }

    public async UniTask LoadScene()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera != default)
        {
            camera.SetActive(false);
        }

        _gameScenePath = $"Scene/GameScene";
        gameSceneAsset = await SceneAssetLoader.LoadSceneAsync(_gameScenePath, true, activateOnLoad: true, token: cts.Token);

        var goes = GameObject.FindGameObjectsWithTag("DestroyInMain");
        foreach(var item in goes)
        {
            GameObject.Destroy(item);
        }

    }
}
