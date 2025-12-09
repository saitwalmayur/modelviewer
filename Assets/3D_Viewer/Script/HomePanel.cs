using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    [SerializeField] private Button m_ModelViewer;
    private void Start()
    {
        m_ModelViewer.onClick.RemoveAllListeners();
        m_ModelViewer.onClick.AddListener(OnClickModelViewer);
    }
    void OnClickModelViewer()
    {
        SceneLoader.LoadScene("Viewer", Color.black, 1, ChangeEffect.LeftToRightFill);
    }
}
