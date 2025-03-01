using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public interface IClientConnection 
{
    UniTask Connect(string url, int port);
    UniTask Close();
    UniTask Send<T>(T message);
    UniTask<TResponse> Send<TRequest, TResponse>(TRequest request, float timeOut = 3.0f, CancellationToken token = default(CancellationToken), bool isHighPriority = false, bool retryWhenTimeOut = true);

    event Action OnStartRequest;
    event Action OnEndRequest;

    void Subscribe<T>(Action<T> callback);
    void UnSubscribe<T>(Action<T> callback);

    event Func<UniTask> OnReconnect;

    string Url { get; }
    int Port { get; }
}
