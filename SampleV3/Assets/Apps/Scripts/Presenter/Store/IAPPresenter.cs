using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.Purchasing.Extension;

using Cysharp.Threading.Tasks;

public class IAPPresenter : IDisposable, IDetailedStoreListener
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    public bool UseFakeStore { get; set; }
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;
    private Action OnPurchaseCompleted;

    public IAPPresenter(IDIContainer container)
    {
        this.Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
    }

    public async UniTask InitAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
    }

    public void HandleIAPCatalogLoaded(string data)
    {
        Debug.Log($"Loaded Asset: {data}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>(data);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

        if (UseFakeStore) // Use bool in editor to control fake store behavior.
        {
            StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser; // Comment out this line if you are building the game for publishing.
            StandardPurchasingModule.Instance().useFakeStoreAlways = true; // Comment out this line if you are building the game for publishing.
        }

#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.GooglePlay)
        );
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore)
        );
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified)
        );
#endif
        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }

        Debug.Log($"Initializing Unity IAP with {builder.products.Count} products");
        UnityPurchasing.Initialize(this, builder);
    }

    public void RestorePurchase() // Use a button to restore purchase only in iOS device.
    {
#if UNITY_IOS
        ExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(OnRestore);
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error initializing IAP: {error}.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP: {error}; {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureDescription.reason} -- { failureDescription.message}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        StoreController = controller;
        ExtensionProvider = extensions;
        Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {StoreController.products.all.Length} products");
        //StoreIconProvider.Initialize(StoreController.products);
        //StoreIconProvider.OnLoadComplete += HandleAllIconsLoaded;
    }

    public void Purchase(string IdOfProduct, Action OnPurchaseCompleted)
    {
        var Product = StoreController.products.WithID(IdOfProduct);
        this.OnPurchaseCompleted = OnPurchaseCompleted;
        if (Product != default)
            StoreController.InitiatePurchase(Product);
    }
}
