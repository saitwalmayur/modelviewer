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

    [SerializeField] private GuidePanel m_GuidePanel;
    [SerializeField] private ModelViewer m_ModelViewer;
    [SerializeField] private ToolsPanel m_ToolsPanel;
    public Color m_SelectionColor;
    private void OnEnable()
    {
        m_GuidePanel.gameObject.SetActive(false);
        m_ToolsPanel.gameObject.SetActive(false);
        m_ModelViewer.gameObject.SetActive(true);
    }

    public void ShowGuidePanel()
    {
        m_GuidePanel.gameObject.SetActive(true);
    }

    public void ShowToolsPanel()
    {
        m_ModelViewer.gameObject.SetActive(false);
        m_GuidePanel.gameObject.SetActive(false);
        m_ToolsPanel.gameObject.SetActive(true);
    }
    public void ShowViewerPanel()
    {
        m_ModelViewer.gameObject.SetActive(true);
        m_ToolsPanel.gameObject.SetActive(false);
    }
}
