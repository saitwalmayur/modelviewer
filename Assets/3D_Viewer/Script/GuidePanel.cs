using UnityEngine;
using UnityEngine.UI;
public class GuidePanel : MonoBehaviour
{
    public Button m_CloseButton;
    private void OnEnable()
    {
        transform.localPosition = new Vector3(0,-2000,0);
        LeanTween.moveLocal(gameObject, Vector3.zero, 0.5f);

        m_CloseButton.onClick.RemoveAllListeners();
        m_CloseButton.onClick.AddListener(OnClickClose);
    }
    void OnClickClose()
    {
        LeanTween.moveLocal(gameObject,new Vector3(0,-2000,0),0.5f);
    }
}
