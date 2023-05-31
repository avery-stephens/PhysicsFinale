using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    DateTime startTime;
    public TimeSpan timeElapsed { get; private set; }

    public void StartTime()
    {
        startTime = DateTime.Now;
    }

    public void GetTime()
    {
        this.timeElapsed = DateTime.Now - startTime;
    }
}
