// Kenneth Gower
// GSP 497
// 1/20/2015

/* This script will start a timer at the beginning of the game. As the game
   progresses, the timer will increase and the screen will grow dimmer. This
   is a very basic first attempt. */
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{

    float timer;
    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log("Timer = " + timer);

        if (timer > 10.0f && timer < 20.0f)
        {
            Debug.Log("Darker...");
        }

        else if (timer > 20.0f && timer < 30.0f)
        {
            Debug.Log("Darker still...");
        }

        else if (timer > 30.0f)
        {
            Debug.Log("Black. Game over.");

            Time.timeScale = 0;
        }
    }
}
