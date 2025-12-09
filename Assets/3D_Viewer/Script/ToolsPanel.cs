using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public SelectedTool m_SelectedTool = SelectedTool.None;
    //
    [Header("----------------")]
    [SerializeField] private Button m_Rotate180Button;
    [SerializeField] private Button m_Rotate90Button;
    [SerializeField] private CustomButton m_ControlButton;

    [SerializeField] private CustomButton m_SelectVetexButtons;
    [SerializeField] private CustomButton m_SelectfirstTerminalPointButtons;
    [SerializeField] private CustomButton m_SelectsecondTerminalPointButtons;
    [SerializeField] private GameObject m_MinimapCamera;

    [SerializeField] private TextMeshProUGUI m_AngleIntruction;
    [SerializeField] private AngleViewer m_AngleViewer;
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

        m_MinimapCamera.gameObject.SetActive(false);
        m_AngleViewer.gameObject.SetActive(false);
        UpdateImage();
        GameEvents.OnSelectPoint += GameEvents_OnSelectPoint;
        GameEvents.OnResetTool += GameEvents_OnResetTool;
        GameEvents.OnSelectVertexPoint += GameEvents_OnSelectVertexPoint;
        GameEvents.OnSelect1TerminalPoint += GameEvents_OnSelect1TerminalPoint;
        GameEvents.OnSelect2TerminalPoint += GameEvents_OnSelect2TerminalPoint;
        m_SelectVetexButtons.gameObject.SetActive(false);
        m_SelectfirstTerminalPointButtons.gameObject.SetActive(false);
        m_SelectsecondTerminalPointButtons.gameObject.SetActive(false);

        m_SelectVetexButtons.m_Button.onClick.RemoveAllListeners();
        m_SelectfirstTerminalPointButtons.m_Button.onClick.RemoveAllListeners();
        m_SelectsecondTerminalPointButtons.m_Button.onClick.RemoveAllListeners();

        m_SelectVetexButtons.m_Button.onClick.AddListener(OnClickSelectVetexButton);
        m_SelectfirstTerminalPointButtons.m_Button.onClick.AddListener(OnClickSelectfirstTerminalPointButton);
        m_SelectsecondTerminalPointButtons.m_Button.onClick.AddListener(OnClickSelectsecondTerminalPointButton);

        m_AngleIntruction.gameObject.SetActive(false);

        foreach (var item in m_ToolsButtons)
        {
            item.m_Image.color = Color.white;
        }
        m_SelectedTool = SelectedTool.None;
        selectedPoints.Clear();
    }
    [SerializeField ]private LineRenderer m_AngleLine;
    private void GameEvents_OnSelect2TerminalPoint(object sender, EventArgs e)
    {
        
    }
    private void GameEvents_OnSelect1TerminalPoint(object sender, EventArgs e)
    {
        m_SelectfirstTerminalPointButtons.gameObject.SetActive(false);
        m_SelectsecondTerminalPointButtons.gameObject.SetActive(true);
        LeanTween.scale(m_SelectsecondTerminalPointButtons.gameObject, Vector3.one * 1.2f, 0.3f).setLoopPingPong();
    }
    void OnClickSelectVetexButton()
    {
        m_AngleIntruction.gameObject.SetActive(true);
        m_AngleIntruction.text = "Please select vertex point";
        LeanTween.cancel(m_SelectVetexButtons.gameObject);
        m_SelectVetexButtons.transform.localScale = Vector3.one;
        GameEvents.OnShowPoint?.Invoke(null,null);
        GameManager.Instance.canSelectPoint = true;
    }
    void OnClickSelectfirstTerminalPointButton()
    {
        m_AngleIntruction.gameObject.SetActive(false);
        m_AngleIntruction.text = "Please select first terminal point";
        LeanTween.cancel(m_SelectfirstTerminalPointButtons.gameObject);
        m_SelectfirstTerminalPointButtons.transform.localScale = Vector3.one;
        GameManager.Instance.canSelectPoint = true;
    }
    void OnClickSelectsecondTerminalPointButton()
    {
        m_AngleIntruction.gameObject.SetActive(false);
        m_AngleIntruction.text = "Please select second terminal point";
        LeanTween.cancel(m_SelectsecondTerminalPointButtons.gameObject);
        m_SelectsecondTerminalPointButtons.transform.localScale = Vector3.one;
        
        GameManager.Instance.canSelectPoint = true;
    }
    private void OnDisable()
    {
        GameEvents.OnSelectPoint -= GameEvents_OnSelectPoint;
        GameEvents.OnResetTool -= GameEvents_OnResetTool;
        GameEvents.OnSelectVertexPoint -= GameEvents_OnSelectVertexPoint;
        GameEvents.OnSelect1TerminalPoint -= GameEvents_OnSelect1TerminalPoint;
        GameEvents.OnSelect2TerminalPoint -= GameEvents_OnSelect2TerminalPoint;
    }

    private void GameEvents_OnSelectVertexPoint(object sender, EventArgs e)
    {
        m_AngleIntruction.gameObject.SetActive(false);
        m_SelectfirstTerminalPointButtons.gameObject.SetActive(true);
        LeanTween.scale(m_SelectfirstTerminalPointButtons.gameObject, Vector3.one * 1.2f, 0.3f).setLoopPingPong();
    }

    private void GameEvents_OnResetTool(object sender, SelectedTool e)
    {
        switch (m_SelectedTool)
        {
            case SelectedTool.None:
                break;
            case SelectedTool.Angle:
                ResetAngle();
                break;
        }
    }

    void ResetAngle()
    {

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
        foreach (var item in m_ToolsButtons)
        {
            item.m_Image.color = Color.white;
        }
        m_ToolsButtons[index].m_Image.color = Color.gray;
        if (index == 0)
        {
            m_SelectedTool = SelectedTool.Angle;
            OnClickAngleButton();
        }
    }
    void OnClickAngleButton()
    {
        m_MinimapCamera.gameObject.SetActive(true);
        m_SelectVetexButtons.gameObject.SetActive(true);
        LeanTween.scale(m_SelectVetexButtons.gameObject, Vector3.one * 1.2f, 0.3f).setLoopPingPong();
    }
    void OnClickCloseButton()
    {
        UIHandler.Instance.ShowViewerPanel();
    }

    void OnClickResetButton()
    {
        selectedPoints.Clear();
        GameEvents.OnResetTool?.Invoke(null, SelectedTool.Angle);
        GameEvents.OnHidePoint.Invoke(null,null);
        pos.Clear();
        m_AngleLine.positionCount = 0;
        m_SelectfirstTerminalPointButtons.gameObject.SetActive(false);
        m_SelectsecondTerminalPointButtons.gameObject.SetActive(false);
        m_AngleViewer.gameObject.SetActive(false);
        GameManager.Instance.canSelectPoint = false;
        OnClickAngleButton();
    }

    ///Angle Calculation;
    ///
    public List<Point> selectedPoints = new List<Point>();

    private void GameEvents_OnSelectPoint(object sender, Point e)
    {
        if (!selectedPoints.Contains(e))
        {
            selectedPoints.Add(e);
            if(selectedPoints.Count == 1)
            {
                GameEvents.OnSelectVertexPoint?.Invoke(null,null);
            }
            if (selectedPoints.Count == 2)
            {
                GameEvents.OnSelect1TerminalPoint?.Invoke(null, null);
            }
            if (selectedPoints.Count == 3)
            {
                GameEvents.OnSelect2TerminalPoint?.Invoke(null, null);
            }
            Debug.Log("Selected: " + e.name);
            GameManager.Instance.canSelectPoint = false ;
        }

        if (selectedPoints.Count == 3)
        {
            CalculateAngle();
           // selectedPoints.Clear();
        }
    }
    private List<Transform> pos = new List<Transform>();
    void CalculateAngle()
    {
        m_AngleLine.positionCount = 3;
        m_AngleLine.useWorldSpace = true;
        Transform A = selectedPoints[0].transform;
        pos.Add(A);
        Transform B = selectedPoints[1].transform;
        pos.Add(B);
        Transform C = selectedPoints[2].transform;
        pos.Add(C);
        // Vectors
        Vector3 BA = A.position - B.position;
        Vector3 BC = C.position - B.position;

 
        // Angle
        float angle = Vector3.Angle(BA, BC);

        Debug.Log($"Angle at B ({B.name}) = {angle}?");
        m_AngleViewer.gameObject.SetActive(true);
        m_AngleViewer.SetAngle(angle.ToString("F2") + "\u00B0");
        m_AngleViewer.transform.SetParent(B);
    }

    private void Update()
    {
        if(pos.Count > 0)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                m_AngleLine.SetPosition(i, pos[i].position);
            }
        }
    }
}
