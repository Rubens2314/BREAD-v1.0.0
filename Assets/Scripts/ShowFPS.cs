using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    public TextMeshProUGUI FpsText;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time>=pollingTime)
        {
            int frameRate=Mathf.RoundToInt(frameCount/time);
            FpsText.text=frameRate.ToString()+"FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
