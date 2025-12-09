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
    public MeshLineBetweenTwoPoints line1;
    public MeshLineBetweenTwoPoints line2;

    public void SetFirstLine(Transform one, Transform two)
    {
        line1.SetPos(one, two);
    }
    public void SetSecondLine(Transform one, Transform two)
    {
        line2.SetPos(one, two);
    }
}
