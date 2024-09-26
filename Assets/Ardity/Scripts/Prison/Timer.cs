using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

    public static Timer Instance;
    // Set the range for the random countdown time
    public float minTime = 5f;  // Minimum time for countdown
    public float maxTime = 15f; // Maximum time for countdown

    // This UnityEvent will be triggered when the countdown reaches zero
    public UnityEvent OnCountdownComplete;

    private float currentTime;
    private bool isCounting = false;

    private void Start()
    {
        //StartCountdown();
    }

    private void Awake()
    {
        Instance = this;
    }

    // Call this to start the countdown
    public void StartCountdown(float time)
    {
        // Assign a random time between minTime and maxTime
        currentTime =time;// UnityEngine.Random.Range(minTime, maxTime);
        isCounting = true;
        Debug.Log("Countdown started with time: " + currentTime + " seconds.");
    }

    private void Update()
    {
        if (isCounting)
        {
            // Reduce the current time by the time passed since the last frame
            currentTime -= Time.deltaTime;
            Debug.Log(currentTime);
            // When the timer reaches zero or less, trigger the complete event and stop the countdown
            if (currentTime <= 0f)
            {
                isCounting = false;
                currentTime = 0f;
                StartCoroutine(GameManager.Instance.CallDecision(2)); // Trigger the event
                Debug.Log("Countdown complete!");
            }
        }
    }
}
