using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    private Transform m_Pivot;
    private void OnEnable()
    {
        GameEvents.OnRotate180 += GameEvents_OnRotate180;
        GameEvents.OnRotate90 += GameEvents_OnRotate90;

        if(m_Pivot == null)
        {
            m_Pivot = GetComponentInChildren<PivotController>().m_Model;
        }
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
        m_Pivot.localRotation *= Quaternion.Euler(90f, 0f, 0f);
    }

    private void GameEvents_OnRotate180(object sender, EventArgs e)
    {
        m_Pivot.localRotation *= Quaternion.Euler(180f, 0f, 0f);
    }
}
