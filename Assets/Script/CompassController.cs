using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour {
    public Transform player;
    public Image compBg;
    public Texture blipTex;
    void OnGUI()
    {
        //GUI.DrawTexture(new Rect(800, 463, 70, 70), compBg);
        GUI.DrawTexture(CreateBlip(), blipTex);
    }

    Rect CreateBlip()
    {
        int w = Screen.width, h = Screen.height;
        float angDeg = player.eulerAngles.y - 90;
        float angRed = angDeg * Mathf.Deg2Rad;

        float blipX = 30 * Mathf.Cos(angRed);
        float blipY = 30 * Mathf.Sin(angRed);
        Transform ts = compBg .GetComponent<Transform>();
        
        blipX += w/1.535f;
        blipY += h/1.118f;

        return new Rect(blipX, blipY, 10 - 1920 / w, 10 - 1920 / w);
    }
}
 