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

    [SerializeField] private CustomButton[] m_ViewButton;

    private void OnEnable()
    {
        m_HomeButton.onClick.RemoveAllListeners();
        m_HomeButton.onClick.AddListener(()=> 
        {
            SceneLoader.LoadScene("Home", Color.black, 2, ChangeEffect.RightToLeftFill);
        });

        m_ResetButton.onClick.RemoveAllListeners();
        m_ResetButton.onClick.AddListener(() =>
        {
            SceneLoader.RestartScene( Color.black, 2, ChangeEffect.TopToBottomFill);
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

        for (int i = 0; i < m_ViewButton.Length; i++)
        {
            int no = i;
            m_ViewButton[no].m_Button.onClick.RemoveAllListeners();
            m_ViewButton[no].m_Button.onClick.AddListener(() => { OnClickViewButton(no); });
        }
        UpdateImage();


        OnClickMenuButton(true);
        GameEvents.OnHidePoint?.Invoke(null,null);

        ///set starting details
        OnClickViewButton(0);

        GameManager.Instance.canSelectPoint = false;
    }
    void OnClickViewButton(int index)
    {
        foreach (var item in m_ViewButton)
        {
            item.m_Image.color = Color.white;
        }
        m_ViewButton[index].m_Image.color = Color.gray;

        //code for switch view
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
            UIHandler.Instance.ShowToolsPanel();
        }
        if (index == 4)
        {
            UIHandler.Instance.ShowGuidePanel();
            //guide
        }
    }
    void OnClickMenuButton()
    {
        isMenuSelected = !isMenuSelected;

        OnClickMenuButton(isMenuSelected);

    }
    void OnClickMenuButton(bool select)
    {
        isMenuSelected = select;

        if (isMenuSelected)
        {
            m_MenuButton.m_Image.color = Color.gray;
            LeanTween.value(m_GroupV.spacing, 10, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.spacing = val;
            });
            LeanTween.value(m_GroupV.padding.top, 100, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.padding.top = (int)val;
            });
            LeanTween.move(m_TopLeft, new Vector2(40, -20), 0.5f);
            LeanTween.move(m_BottomRight, new Vector2(0, 30), 0.5f);
            LeanTween.move(m_BottomLeft, new Vector2(40, 40), 0.5f);
        }
        else
        {
            m_MenuButton.m_Image.color = Color.white;
            LeanTween.value(m_GroupV.spacing, -90, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.spacing = val;
            });
            LeanTween.value(m_GroupV.padding.top, 0, 0.5f).setOnUpdate((val) =>
            {
                m_GroupV.padding.top = (int)val;
            });
            LeanTween.move(m_TopLeft, new Vector2(40, 120), 0.5f);
            LeanTween.move(m_BottomRight, new Vector2(0, -150), 0.5f);
            LeanTween.move(m_BottomLeft, new Vector2(40, -100), 0.5f);
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
