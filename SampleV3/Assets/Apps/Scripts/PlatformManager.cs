using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public enum TargetFrameRate
    {
        FPS_30 = 30,
        FPS_60 = 60,
        FPS_120 = 120,
    }
    public TargetFrameRate Rate = TargetFrameRate.FPS_30;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Rate;
        AutoStart();
    }

    void AutoStart()
    {

        Destroy(this);
    }
}
