using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Core.Model;

public class LoginService : IDisposable
{
    private readonly IDIContainer container;
    private static LoginService instance;
    public static LoginService Instance => instance;

    private ISocketClient Client => client ??= container.Inject<ISocketClient>();
    private ISocketClient client;
   


    private LoginService(IDIContainer container)
    {
        this.container = container;
    }

    public void Dispose()
    {
        client = default;
        instance = default;
    }

    public static LoginService Instantiate(IDIContainer container)
    {
        if (instance == default)
        {
            instance = new LoginService(container);
        }
        return instance;
    }

}
