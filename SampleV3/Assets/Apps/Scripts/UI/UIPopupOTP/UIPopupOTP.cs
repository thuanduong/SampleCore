using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPopupOTP :PopupEntity<UIPopupOTP.Entity>
{
    [System.Serializable]
    public class Entity
    {
        public string inputOtp;
        public ButtonEntity closeBtn;
        public ButtonEntity confirmBtn;
        public ButtonEntity resendOtpBtn;
    }

    public TMP_InputField inputOtp;
    public UIButtonComponent confirmBtn;
    public UIButtonComponent closeBtn;
    public UIButtonComponent resendOtpBtn;
    protected override void OnSetEntity()
    {
        inputOtp.text = this.entity.inputOtp;
        confirmBtn.SetEntity(this.entity.confirmBtn);
        closeBtn.SetEntity(this.entity.closeBtn);
        resendOtpBtn.SetEntity(this.entity.resendOtpBtn);
    }
}
