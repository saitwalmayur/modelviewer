using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsButton : MonoBehaviour
{
    [SerializeField] private CustomButton m_SelectVetexButtons;
    [SerializeField] private CustomButton m_SelectfirstTerminalPointButtons;
    [SerializeField] private CustomButton m_SelectsecondTerminalPointButtons;

    private void OnEnable()
    {
        m_SelectVetexButtons.m_Button.onClick.RemoveAllListeners();
        m_SelectfirstTerminalPointButtons.m_Button.onClick.RemoveAllListeners();
        m_SelectsecondTerminalPointButtons.m_Button.onClick.RemoveAllListeners();
    }
}
