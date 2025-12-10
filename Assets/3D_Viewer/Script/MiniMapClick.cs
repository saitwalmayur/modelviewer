using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MiniMapClick : MonoBehaviour
{
    public Button m_Top;
    public Button m_Left;
    public Button m_Mid;

    public Button m_Right;
    public Button m_Bottom;
    private void OnEnable()
    {
        m_Top?.onClick.RemoveAllListeners();
        m_Left?.onClick.RemoveAllListeners();
        m_Mid?.onClick.RemoveAllListeners();
        m_Right?.onClick.RemoveAllListeners();
        m_Bottom?.onClick.RemoveAllListeners();

        m_Top?.onClick.AddListener(() =>
        {
            GameEvents.OnSelectMiniMap?.Invoke(null,MinimapDir.Top);
        });
        m_Left?.onClick.AddListener(() =>
        {
            GameEvents.OnSelectMiniMap?.Invoke(null, MinimapDir.Left);
        });
        m_Mid?.onClick.AddListener(() =>
        {
            GameEvents.OnSelectMiniMap?.Invoke(null, MinimapDir.Middle);
        });
        m_Right?.onClick.AddListener(() =>
        {
            GameEvents.OnSelectMiniMap?.Invoke(null, MinimapDir.Right);
        });
        m_Bottom?.onClick.AddListener(() =>
        {
            GameEvents.OnSelectMiniMap?.Invoke(null, MinimapDir.Bottom);
        });
    }
}
