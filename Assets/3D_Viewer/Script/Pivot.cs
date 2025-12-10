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
        Vector3 facePos = center;
        switch (e)
        {
            case MinimapDir.Right:
                facePos = center + transform.right * extents.x;
                break;
            case MinimapDir.Left:
                facePos = center - transform.right * extents.x;
                break;
            case MinimapDir.Top:
                facePos = center + transform.up * extents.y;
                break;
            case MinimapDir.Bottom:
                facePos = center - transform.up * extents.y;
                break;
            case MinimapDir.Middle:
                break;
        }
        // Pick face by string
        switch (side)
        {
            case "Left":
               
                break;
            case "Right":
               
                break;
            case "Top":
             
                break;
            case "Bottom":
           
                break;
        }

        // --- Convert world position to screen ---
        Vector3 screenPoint = cam.WorldToScreenPoint(facePos);

        // Desired X / Y based on screen side
        Vector3 targetScreenPoint = screenPoint;

        if (side == "Left") targetScreenPoint.x = 0;
        if (side == "Right") targetScreenPoint.x = Screen.width;
        if (side == "Bottom") targetScreenPoint.y = 0;
        if (side == "Top") targetScreenPoint.y = Screen.height;

        // Convert back to world
        Vector3 targetWorldPoint = cam.ScreenToWorldPoint(targetScreenPoint);

        // Offset needed
        Vector3 offset = targetWorldPoint - facePos;

        // Move whole model
        transform.position += offset;
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
