using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRatioFOV : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    [SerializeField] float DefaultFOV;
    [SerializeField] float DefaultRatio = 0.5625f;
    [SerializeField] float MinFOV;
    [SerializeField] float MaxFOV;
    [SerializeField] bool AutoRemove = true;

    private void Reset()
    {
        if (targetCamera == default)
            targetCamera = GetComponent<Camera>();
    }

    public void Awake()
    {
        DetechRatio();
        if (AutoRemove)
            Destroy(this);
    }

    void DetechRatio()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        if (screenRatio < DefaultRatio)
        {
            targetCamera.fieldOfView = MaxFOV;
        }
        else if (screenRatio > DefaultRatio)
        {
            targetCamera.fieldOfView = MinFOV;
        }
        else
        {
            targetCamera.fieldOfView = DefaultFOV;

        }
    }
}
