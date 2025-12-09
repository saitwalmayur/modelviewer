using System;
using UnityEngine;

public class Point : MonoBehaviour
{
    public bool isSelected;
    private MeshRenderer m_MeshRenderer;
    private void OnEnable()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshRenderer.material.color = Color.red;
        GameEvents.OnHidePoint += GameEvents_OnHidePoint;
    }

    private void GameEvents_OnHidePoint(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        isSelected = true;
        GameEvents.OnSelectPoint?.Invoke(null,this);
        m_MeshRenderer.material.color = Color.green;
    }
    private void OnDisable()
    {
        GameEvents.OnHidePoint -= GameEvents_OnHidePoint;
    }
}
