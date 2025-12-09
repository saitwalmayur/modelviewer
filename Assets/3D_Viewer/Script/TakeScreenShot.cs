using UnityEngine;
using UnityEditor;
namespace TechJuego.BallDestiny.Utils
{
    public class TakeScreenShot : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Window/Take Screen Shot")]
        static void TakeScreen()
        {
            string resolution = string.Empty;
            resolution = "" + Screen.width + "X" + Screen.height;
            ScreenCapture.CaptureScreenshot("ScreenShot-" + resolution + "-" + PlayerPrefs.GetInt("number", 0) + ".png");
            PlayerPrefs.SetInt("number", PlayerPrefs.GetInt("number", 0) + 1);
        }
#endif
    }
}