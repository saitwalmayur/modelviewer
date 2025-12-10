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
        GameEvents.OnSelectMiniMap += GameEvents_OnSelectMiniMap;
        if (m_Pivot == null)
        {
            m_Pivot = GetComponentInChildren<PivotController>().m_Model;
        }

        
    }
    

    private void OnDisable()
    {
        GameEvents.OnRotate180 -= GameEvents_OnRotate180;
        GameEvents.OnRotate90 -= GameEvents_OnRotate90;
        GameEvents.OnSelectMiniMap -= GameEvents_OnSelectMiniMap;
    }
    private void OnDestroy()
    {
        GameEvents.OnRotate180 -= GameEvents_OnRotate180;
        GameEvents.OnRotate90 -= GameEvents_OnRotate90;
        GameEvents.OnSelectMiniMap -= GameEvents_OnSelectMiniMap;
    }

    private void GameEvents_OnSelectMiniMap(object sender, MinimapDir e)
    {
        BoxCollider box = GetComponentInChildren<BoxCollider>();
        if (box == null) return;

        // World center of collider
        Vector3 center = transform.TransformPoint(box.center);

        // World extents (half size)
        Vector3 extents = Vector3.Scale(box.size * 0.5f, transform.lossyScale);

        // 6 faces
        Vector3[] faceCenters =
        {
            center + transform.right  * extents.x,  // Right
            center - transform.right  * extents.x,  // Left
            center + transform.up     * extents.y,  // Top
            center - transform.up     * extents.y,  // Bottom
            center + transform.forward* extents.z,  // Front
            center - transform.forward* extents.z   // Back
        };
       
        switch (e)
        {
            case MinimapDir.Right:
                transform.position = extents + new Vector3(5, 0, 0);
                break;
            case MinimapDir.Left:
                break;
            case MinimapDir.Top:
                break;
            case MinimapDir.Bottom:
                break;
            case MinimapDir.Middle:
                break;
        }
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
