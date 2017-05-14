using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crosshair : MonoBehaviour
{
    public Texture2D CrosshairTexture;
    
    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (CrosshairTexture.width / 2);
        float yMin = (Screen.height / 2) - (CrosshairTexture.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, CrosshairTexture.width, CrosshairTexture.height), CrosshairTexture);
    }
}
