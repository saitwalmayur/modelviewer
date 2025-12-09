using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    [SerializeField] private Button m_ModelViewer;
    [SerializeField] private Button m_CloseViewer;
    private void Start()
    {
        m_ModelViewer.onClick.RemoveAllListeners();
        m_ModelViewer.onClick.AddListener(OnClickModelViewer);

        m_CloseViewer.onClick.RemoveAllListeners();
        m_CloseViewer.onClick.AddListener(OnClickCloseButton);
    }
    void OnClickModelViewer()
    {
        SceneLoader.LoadScene("Viewer", Color.black, 2, ChangeEffect.LeftToRightFill);
    }
    void OnClickCloseButton()
    {
        Application.Quit();
    }
}
