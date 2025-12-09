using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public bool isSelected;
    private MeshRenderer m_MeshRenderer;
    private void OnEnable()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshRenderer.material.color = Color.red;
    }
    private void OnMouseDown()
    {
        isSelected = true;
        GameEvents.OnSelectPoint?.Invoke(null,this);
        m_MeshRenderer.material.color = Color.green;
    }
}
