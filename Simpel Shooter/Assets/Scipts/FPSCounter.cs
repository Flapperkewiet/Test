using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FPSCounter : MonoBehaviour 
{
    public int frameCount = 0;
    public float dt = 0.0f;
    public float fps = 0.0f;
    public float updateRate = 4.0f;  // 4 updates per sec.

    public UnityEngine.UI.Text FPSText;
    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
            FPSText.text = ((int)fps).ToString();
        }

    }
}
