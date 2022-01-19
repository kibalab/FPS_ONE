using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    public Text FPSText;
    public Text msText;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        msText.text = string.Format("{0:0.0} ms", msec);
        FPSText.text = string.Format("{1:0.} FPS", msec, fps);
    }
}