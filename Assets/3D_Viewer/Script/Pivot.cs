using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnRotate180 += GameEvents_OnRotate180;
        GameEvents.OnRotate90 += GameEvents_OnRotate90;
    }

   

    private void OnDisable()
    {
        GameEvents.OnRotate180 -= GameEvents_OnRotate180;
        GameEvents.OnRotate90 -= GameEvents_OnRotate90;
    }
    private void OnDestroy()
    {
        GameEvents.OnRotate180 -= GameEvents_OnRotate180;
        GameEvents.OnRotate90 -= GameEvents_OnRotate90;
    }


    private void GameEvents_OnRotate90(object sender, EventArgs e)
    {
        transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
    }

    private void GameEvents_OnRotate180(object sender, EventArgs e)
    {
        transform.localRotation *= Quaternion.Euler(180f, 0f, 0f);
    }
}
