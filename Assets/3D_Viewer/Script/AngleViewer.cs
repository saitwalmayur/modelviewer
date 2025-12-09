using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AngleViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_AngleText;

    public void SetAngle(string angle)
    {
        m_AngleText.text = angle;
      
    }
    void LateUpdate()
    {
        if (Camera.main == null) return;
        transform.LookAt(transform.position + Camera.main.transform.forward);
        transform.localScale = new Vector3(20, 20, 20);
        transform.transform.localPosition = new Vector3(0, 10, 0);
    }
}
