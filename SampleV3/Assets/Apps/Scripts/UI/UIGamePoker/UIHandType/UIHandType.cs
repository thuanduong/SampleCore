using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class UIHandType : UIComponent<UIHandType.Entity>
{
    public class Entity
    {
        public UIImageComponent.Entity image;
    }

    public UIImageComponent image;

    private CancellationTokenSource cts;

    public bool IsShowing { get; private set; }

    private void Start()
    {
        defaultOut();
    }

    protected override void OnSetEntity()
    {
        image.SetEntity(entity.image);
        defaultOut();
    }

    private void OnDestroy()
    {
        cts.SafeCancelAndDispose();
        cts = default;
    }

    public void SetHandType(Sprite s)
    {
        if (entity == null) return;
        if (entity.image != null)
        {
            entity.image.sprite = s;
        }
        else
        {
            entity.image = new UIImageComponent.Entity() { sprite = s };
        }
        image.SetEntity(entity.image);
    }

    public async UniTask In()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        gameObject.SetActive(true);
        IsShowing = true;
    }

    public async UniTask Out()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
        gameObject.SetActive(false);
        IsShowing = false;
    }

    private void defaultOut()
    {
        gameObject.SetActive(false);
        IsShowing = false;
    }
}
