using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
#if UNITY_WEBGL
using NativeWebSocket;
#endif
using UnityEngine;

public class WebSocketClient : SocketClientBase, ISocketClient
{
#if UNITY_WEBGL
    private WebSocket ws;
#endif
    private CancellationTokenSource cts;
    private UniTaskCompletionSource connectTask;

    public static WebSocketClient Initialize(IMessageParser messageParser, IErrorCodeConfiguration errorCodeConfig)
    {
        var go = new GameObject("WebSocketClient");
        var webSocketClient = go.AddComponent<WebSocketClient>();
        webSocketClient.Init(messageParser, errorCodeConfig);
        return webSocketClient;
    }

    public void SubscribeMessage()
    {
#if UNITY_WEBGL
        ws.OnMessage += OnMessage;
        ws.OnError += OnError;
        ws.OnClose += OnClose;
        ws.OnOpen += OnOpen;
#endif
    }

    private void OnOpen()
    {
        Debug.Log("Open ws");
        connectTask.TrySetResult();
    }

    public void UnSubscribeMessage()
    {
#if UNITY_WEBGL
        if (ws != null)
        {
            ws.OnMessage -= OnMessage;
            ws.OnError -= OnError;
            ws.OnClose -= OnClose;
            ws.OnOpen += OnOpen;
        }
#endif
    }

#if UNITY_WEBGL
    private void OnClose(WebSocketCloseCode closeCode)
    {
        Debug.LogError("Ws close with code " + closeCode);
    }
#endif

    private void OnError(string errorMsg)
    {
        Debug.LogError("Ws Error :" + errorMsg);
    }

    public override async UniTask Send<T>(T message)
    {
#if UNITY_WEBGL
        if (ws != null && ws.State == WebSocketState.Open)
        {
            await ws.Send(messageParser.ToByteArray(message));
        }
#endif
        await UniTask.CompletedTask;
    }

    public override async UniTask Connect(string url, int port)
    {
#if UNITY_WEBGL
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        connectTask = new UniTaskCompletionSource();
        ws = new WebSocket($"{url}:{port}/ws");
        SubscribeMessage();
        _ = ws.Connect();
        await connectTask.Task.AttachExternalCancellation(cts.Token).ThrowWhenTimeOut();
#else
        await UniTask.CompletedTask;
#endif
    }

    public override async UniTask Close()
    {
#if UNITY_WEBGL
        cts.SafeCancelAndDispose();
        cts = default;
        UnSubscribeMessage();
        if (ws != null) await ws.Close();
        ws = default;
#endif
        
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
#if ENABLE_WEBGL
        if (ws?.State == WebSocketState.Open)
        {
            ws.DispatchMessageQueue();
        }
#endif
#endif
    }

    public override async void Dispose()
    {
        base.Dispose();
#if UNITY_WEBGL
        cts.SafeCancelAndDispose();
        cts = default;
        GameObject.Destroy(gameObject);
        UnSubscribeMessage();
        if (ws != null) await ws.Close();
        ws = default;
#endif
    }
}