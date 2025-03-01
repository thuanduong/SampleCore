using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIToggleComponent : UIComponent<UIToggleComponent.Entity>
{
	[System.Serializable]
    public class Entity
    {
	    public bool isOn;
		public Action<bool> onActiveToggle = ActionUtility.EmptyAction<bool>.Instance;
    }

    public Toggle toggle;
    private Action<bool> onActiveToggleInternal = ActionUtility.EmptyAction<bool>.Instance;

    [SerializeField] private UnityEvent onActiveToggle;
    [SerializeField] private UnityEvent onDeActiveToggle;

    private void Awake()
    {
	    toggle.onValueChanged.AddListener(val => OnActiveToggleInternal(val));
    }

    protected override void OnSetEntity()
    {
	    toggle.isOn = this.entity.isOn;
	    onActiveToggleInternal = ActionUtility.EmptyAction<bool>.Instance;
	    onActiveToggleInternal += this.entity.onActiveToggle;
    }

    private void Reset()
    {
	    toggle = GetComponent<Toggle>();
    }

    private void OnActiveToggleInternal(bool val)
    {
        if (val) {
            onActiveToggle?.Invoke();
        }
        else
        {
            onDeActiveToggle?.Invoke();
        }
        onActiveToggleInternal(val);
    }
}	