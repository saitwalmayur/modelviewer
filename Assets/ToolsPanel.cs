using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedTool
{
    None,
    Angle
}
public class ToolsPanel : MonoBehaviour
{
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private Button m_ResetButton;
    [SerializeField] private CustomButton[] m_ToolsButtons;
    public SelectedTool SelectedTool = SelectedTool.None;
    //
    [Header("----------------")]
    [SerializeField] private Button m_Rotate180Button;
    [SerializeField] private Button m_Rotate90Button;
    [SerializeField] private CustomButton m_ControlButton;
    private void OnEnable()
    {
        m_CloseButton.onClick.RemoveAllListeners();
        m_CloseButton.onClick.AddListener(OnClickCloseButton);

        m_ResetButton.onClick.RemoveAllListeners();
        m_ResetButton.onClick.AddListener(OnClickResetButton);

        m_Rotate180Button.onClick.RemoveAllListeners();
        m_Rotate180Button.onClick.AddListener(() =>
        {
            GameEvents.OnRotate180.Invoke(null, null);
        });

        m_Rotate90Button.onClick.RemoveAllListeners();
        m_Rotate90Button.onClick.AddListener(() =>
        {
            GameEvents.OnRotate90.Invoke(null, null);
        });
        m_ControlButton.m_Button.onClick.RemoveAllListeners();
        m_ControlButton.m_Button.onClick.AddListener(OnClickControlButton);

        for (int i = 0; i < m_ToolsButtons.Length; i++)
        {
            int no = i;
            m_ToolsButtons[no].m_Button.onClick.RemoveAllListeners();
            m_ToolsButtons[no].m_Button.onClick.AddListener(()=>
            {
                OnClickToolsButton(no);
            });
        }

        UpdateImage();
        GameEvents.OnSelectPoint += GameEvents_OnSelectPoint;
    }
    private void OnDisable()
    {
        GameEvents.OnSelectPoint -= GameEvents_OnSelectPoint;
    }

 
    public Sprite m_MoveSp;
    public Sprite m_RotateSp;

    void OnClickControlButton()
    {
        GameEvents.controlType = GameEvents.controlType == ControlType.Rotate ? ControlType.Move : ControlType.Rotate;
        GameEvents.OnSetControl.Invoke(null, GameEvents.controlType);
        UpdateImage();
    }
    void UpdateImage()
    {
        switch (GameEvents.controlType)
        {
            case ControlType.Move:
                m_ControlButton.m_Image.sprite = m_MoveSp;
                break;
            case ControlType.Rotate:
                m_ControlButton.m_Image.sprite = m_RotateSp;
                break;
        }
    }

    void OnClickToolsButton(int index)
    {
        if (index == 0)
        {
            SelectedTool = SelectedTool.Angle;
                OnClickAngleButton();
        }
    }
    void OnClickAngleButton()
    {
    }
    void OnClickCloseButton()
    {
        UIHandler.Instance.ShowViewerPanel();
    }

    void OnClickResetButton()
    {
        SelectedTool = SelectedTool.None;
    }

    ///Angle Calculation;
    ///
    public List<Point> selectedPoints = new List<Point>();

    private void GameEvents_OnSelectPoint(object sender, Point e)
    {
        if (!selectedPoints.Contains(e))
        {
            selectedPoints.Add(e);
            Debug.Log("Selected: " + e.name);
        }

        if (selectedPoints.Count == 3)
        {
            CalculateAngle();
            selectedPoints.Clear();
        }
    }


    void CalculateAngle()
    {
        Transform A = selectedPoints[0].transform;
        Transform B = selectedPoints[1].transform;
        Transform C = selectedPoints[2].transform;

        // Vectors
        Vector3 BA = A.position - B.position;
        Vector3 BC = C.position - B.position;

        // Angle
        float angle = Vector3.Angle(BA, BC);

        Debug.Log($"Angle at B ({B.name}) = {angle}?");
    }

}
