using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UIFriend : PopupEntity<UIFriend.Entity>
{
    public class Entity {
        public ButtonEntity btnClose;

        public UIFriendListInfoItem.Entity ListFriend;
    }

    public UIButtonComponent btnClose;

    public UIFriendListInfoItem ListFriend;
    public ReloadScrollRectComponent ScrollRect;

    CancellationTokenSource cts;

    private void OnDestroy()
    {
        cts.SafeCancelAndDispose();
        cts = default;
    }

    protected override void OnSetEntity()
    {
        btnClose.SetEntity(entity.btnClose);

        ListFriend.SetEntity(entity.ListFriend);
        ScrollRect.SetPool(ListFriend);
    }

    public void SetListFriend(UIFriendInfoItem.Entity[] entities)
    {
        entity.ListFriend.entities = entities;
        ListFriend.SetEntity(entity.ListFriend);
    }

    public void Reload()
    {
        ScrollRect.Reload();
    }

}
