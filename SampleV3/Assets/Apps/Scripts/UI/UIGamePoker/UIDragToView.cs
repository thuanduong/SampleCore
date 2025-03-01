using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class UIDragToView : PopupEntity<UIDragToView.Entity>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public class Entity 
    {
        public RenderTexture rawImg;
        public float Duration;
    }

    public Image BG;
    public Image Mask;
    public RawImage rawImg;
    public Button btnClose;

    public float Offset = 100;
    public float Threshold = 95.0f;

    bool isTouching, isTouchable;
    Vector3 touchStart, touchCurrent;
    float timeCount = 0;
    float Pointer = 0;

    public System.Action<float> OnDragToView = ActionUtility.EmptyAction<float>.Instance;
    public System.Action OnClose = ActionUtility.EmptyAction.Instance;

    Sequence sq;
    CancellationTokenSource cts;
    Coroutine coroutine = default;

    void Start()
    {
        btnClose.onClick.AddListener(() => OnClose());
    }

    protected override void OnSetEntity()
    {
        rawImg.texture = entity.rawImg;
        BG.fillAmount = 0;
        Mask.fillAmount = 0;

        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        cts.SafeCancelAndDispose();
        cts = default;
    }

    public void ResetDrag()
    {
        Pointer = 0;
        if (coroutine != default)
            StopCoroutine(coroutine);
        coroutine = default;
    }

    public async UniTask ShowAsync()
    {
        if (sq != default)
            sq.Kill();
        sq = DOTween.Sequence();
        await sq.Append(BG.DOFillAmount(1.0f, 0.25f))
            .Join(Mask.DOFillAmount(1.0f, 0.25f)).WithCancellation(cts.Token);
        isTouchable = true;
        isTouching = false;

        coroutine = StartCoroutine(DragToViewPoint(entity.Duration));
    }

    public async UniTask HideAsync()
    {
        if (sq != default)
            sq.Kill(); 
        if (coroutine != default)
            StopCoroutine(coroutine);
        coroutine = default;

        isTouchable = false;
        isTouching = false;
        sq = DOTween.Sequence();
        await sq.Append(BG.DOFillAmount(0.0f, 0.25f))
            .Join(Mask.DOFillAmount(0.0f, 0.25f)).WithCancellation(cts.Token);
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTouchable)
        {
            touchStart = eventData.position;
            isTouching = true;
            timeCount = 0.0f;
        }
        else
            isTouching = false;
    }

    // Drag the selected item.
    public void OnDrag(PointerEventData data)
    {
        if (isTouching)
        {
            if (data.dragging)
            {
                // Object is being dragged.
                timeCount += Time.deltaTime;
                if (timeCount > 0.25f)
                {
                    timeCount = 0.0f;
                    touchCurrent = data.position;
                    Vector3 direc = touchStart - touchCurrent;
                    Pointer = direc.y / Offset;
                }
            }
        }
        else if (isTouchable)
        {
            touchStart = data.position;
            isTouching = true;
            timeCount = 0.0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        touchCurrent = eventData.position;
        isTouching = false;
    }

    IEnumerator DragToViewPoint(float time)
    {
        float mm = 0;
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            mm = Mathf.Lerp(mm, Pointer, Time.deltaTime * 5.0f);
            OnDragToView(mm);
            if (mm >= Threshold)
            {
                if (t < time - 2.0f)
                    t = time - 2.0f;
            }
            yield return null;
        }
        coroutine = default;
        OnClose();
    }
}

