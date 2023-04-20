using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeDestroy;
    [SerializeField] private float timeToLive = 60.0f;
    [SerializeField] private float timeRemaining;

    private void Start()
    {
        timeRemaining = timeToLive;
    }

    private void Update()
    {

        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime; 

            TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
            string timeString = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

            timeDestroy.text = timeString;
        }
        else
        {
            DestroyBase();
        }
    }

    private void DestroyBase()
    {
        print("You Loze!TREEEEERINMNNNG");
    }
}
