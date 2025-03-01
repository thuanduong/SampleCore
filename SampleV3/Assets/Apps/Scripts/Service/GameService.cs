using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class GameService : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;
    private ISocketClient client;
    private ISocketClient Client => client ??= container.Inject<ISocketClient>();

    private static GameService instance;
    public static GameService Instance => instance;

    public static GameService Instantiate(IDIContainer container)
    {
        if (instance == default)
        {
            instance = new GameService(container);
        }
        return instance;
    }

    private GameService(IDIContainer container)
    {
        this.container = container;
        cts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        DisposeUtility.SafeDispose(ref cts);
        client = default;
        instance = default;
    }

    public async UniTask<bool> Join(string gameMode, CancellationToken cancelToken)
    {
        await UniTask.CompletedTask;
        return true;
    }

    public async UniTask CancelFinding(CancellationToken cancelToken)
    {

    }
}
