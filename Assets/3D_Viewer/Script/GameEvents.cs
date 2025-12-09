using UnityEngine;
using System;
public enum ControlType
{
    Move,
    Rotate
}
public class GameEvents
{
    public static EventHandler OnRotate90;
    public static EventHandler OnRotate180;
    public static EventHandler<ControlType> OnSetControl;
    public static ControlType controlType = ControlType.Rotate;
}
