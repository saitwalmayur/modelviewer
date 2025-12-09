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
    public static EventHandler<Point> OnSelectPoint;
    public static EventHandler OnHidePoint;
    public static EventHandler OnShowPoint;
    public static EventHandler<SelectedTool> OnResetTool;

    public static EventHandler OnSelectVertexPoint;
    public static EventHandler OnSelect1TerminalPoint;
    public static EventHandler OnSelect2TerminalPoint;

}
