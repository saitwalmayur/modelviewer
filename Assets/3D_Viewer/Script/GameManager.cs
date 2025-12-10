using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    private static GameManager _instance;
    public GameManager()
    {
        _instance = this;
    }

    public bool canSelectPoint;
    public CustomCylinderBetweenTwoPoints line1;
    public CustomCylinderBetweenTwoPoints line2;

    public void ShowLine(Transform one, Transform two, Transform thee)
    {
        line1.gameObject.SetActive(true);
        line1.SetPos(one, two);

        line2.gameObject.SetActive(true);
        line2.SetPos(two, thee);
    }

    public void HideLine()
    {
        line1.gameObject.SetActive(false);
        line2.gameObject.SetActive(false);
    }
}
