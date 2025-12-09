using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIHandler();
            }
            return _instance;
        }
    }
    private static UIHandler _instance;
    public UIHandler()
    {
        _instance = this;
    }

    public GuidePanel m_GuidePanel;
    public Color m_SelectionColor;
    private void OnEnable()
    {
        m_GuidePanel.gameObject.SetActive(false);
    }
}
