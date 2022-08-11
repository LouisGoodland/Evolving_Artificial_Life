using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    bool isTiming;
    float currentTime;
    public Text currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        resetTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTiming)
        {
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            if(currentTimeText != null)
            {
                currentTimeText.text = "Time taken: " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
            }
            
        }
    }

    public void resetTime()
    {
        currentTime = 0;
        isTiming = true;
        if(currentTimeText != null)
        {
            currentTimeText.text = "Time taken: 0:0:0";
        }
    }

    public float getTime()
    {
        return currentTime;
    }

    public void startTiming()
    {
        isTiming = true;
    }
}
