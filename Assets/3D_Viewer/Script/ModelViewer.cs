using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModelViewer : MonoBehaviour
{
    [SerializeField] private Button m_HomeButton;
    [SerializeField] private Button m_ResetButton;
 
    private bool isMenuSelected = false;
    public RectTransform m_TopLeft;
    public RectTransform m_BottomRight;
    public RectTransform m_BottomLeft;
    [SerializeField] private CustomButton m_MenuButton;
  
    [SerializeField] private CustomButton[] m_MenuButtons;
    public VerticalLayoutGroup m_GroupV;

    //
    [Header("----------------")]
    [SerializeField] private Button m_Rotate180Button;
    [SerializeField] private Button m_Rotate90Button;
    [SerializeField] private CustomButton m_ControlButton;

    //
    [Header("----------------")]
    [SerializeField] private CustomButton m_AssemblyButton;
    [SerializeField] private CustomButton m_ProjectButton;

    private void OnEnable()
    {
        m_HomeButton.onClick.RemoveAllListeners();
        m_HomeButton.onClick.AddListener(()=> 
        {
            SceneLoader.LoadScene("Home", Color.black, 1, ChangeEffect.RightToLeftFill);
        });

        m_ResetButton.onClick.RemoveAllListeners();
        m_ResetButton.onClick.AddListener(() =>
        {
            SceneLoader.RestartScene( Color.black, 1, ChangeEffect.TopToBottomFill);
        });

        m_Rotate180Button.onClick.RemoveAllListeners();
        m_Rotate180Button.onClick.AddListener(() =>
        {
            GameEvents.OnRotate180.Invoke(null,null);
        });

        m_Rotate90Button.onClick.RemoveAllListeners();
        m_Rotate90Button.onClick.AddListener(() =>
        {
            GameEvents.OnRotate90.Invoke(null, null);
        });

        m_ControlButton.m_Button.onClick.RemoveAllListeners();
        m_ControlButton.m_Button.onClick.AddListener( OnClickControlButton);

        m_MenuButton.m_Button.onClick.RemoveAllListeners();
        m_MenuButton.m_Button.onClick.AddListener(OnClickMenuButton);

        for (int i = 0; i < m_MenuButtons.Length; i++)
        {
            int no = i;
            m_MenuButtons[no].m_Button.onClick.RemoveAllListeners();
            m_MenuButtons[no].m_Button.onClick.AddListener(()=> { OnClicmMenuButtons(no); });
        }
        UpdateImage();

        m_TopLeft.gameObject.SetActive(isMenuSelected);
        m_BottomRight.gameObject.SetActive(isMenuSelected);
        m_BottomLeft.gameObject.SetActive(isMenuSelected);
        OnClickMenuButton();
    }

    void OnClicmMenuButtons(int index)
    {
        if(index == 0)
        {
            //control
        }
        if (index == 1)
        {
            //visaul
        }
        if (index == 2)
        {
            //part
        }
        if (index == 3)
        {
            //tools
        }
        if (index == 4)
        {
            //guide
        }
    }
    void OnClickMenuButton()
    {
        isMenuSelected = !isMenuSelected;

        m_TopLeft.gameObject.SetActive(isMenuSelected);
        m_BottomRight.gameObject.SetActive(isMenuSelected);

        if(isMenuSelected)
        {
            LeanTween.value(m_GroupV.spacing, 10, 0.5f).setOnUpdate((val) =>
            {
                  m_GroupV.spacing = val;
            });
            LeanTween.value(m_GroupV.padding.top, 100, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.padding.top = (int)val;
            });
        }
        else
        {
            LeanTween.value(m_GroupV.spacing, -90, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.spacing = val;
            });
            LeanTween.value(m_GroupV.padding.top, 0, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.padding.top = (int)val;
            });
        }
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
}
