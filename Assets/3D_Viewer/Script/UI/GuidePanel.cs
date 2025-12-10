using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class GuideData
{
    public string Detail;
    public Sprite Icon;
}
public class GuidePanel : MonoBehaviour
{
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private CustomButton[] m_GuideButtons;
    [SerializeField] private GuideData[] m_GuideDataDetails;

    [SerializeField] private Image m_SelectedImage;
    [SerializeField] private TextMeshProUGUI m_DetailText;
    private void OnEnable()
    {
        transform.localPosition = new Vector3(0,-2000,0);
        LeanTween.moveLocal(gameObject, Vector3.zero, 0.3f);
        m_CloseButton.onClick.RemoveAllListeners();
        m_CloseButton.onClick.AddListener(OnClickClose);
        for (int i = 0; i < m_GuideDataDetails.Length; i++)
        {
            int no = i;

            m_GuideButtons[i].m_Image.sprite = m_GuideDataDetails[no].Icon;
        m_GuideButtons[i].m_Button.onClick.RemoveAllListeners();
            m_GuideButtons[i].m_Button.onClick.AddListener(()=>
            {
                OnSelectGuide(no);
            });
        }

        OnSelectGuide(0);
    }
    void OnSelectGuide(int index)
    {
        m_DetailText.text = m_GuideDataDetails[index].Detail;
        m_SelectedImage.sprite = m_GuideDataDetails[index].Icon ;
}
    void OnClickClose()
    {
        LeanTween.moveLocal(gameObject,new Vector3(0,-2000,0),0.5f).setOnComplete(()=>
        {
            gameObject.SetActive(false);
        });
    }
}
