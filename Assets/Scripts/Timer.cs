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

    float timer; // variable for our timer.
    GameObject _light; // variable to access the point light in the test scene. 

   
    // Use this for initialization
    void Start()
    {
        timer = 0.0f;

        _light = GameObject.FindGameObjectWithTag("MainLight");
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10.0f && timer < 20.0f)
        {
            Debug.Log("Darker...");

            // After ten seconds have passed change the color of the renderer's ambient light settings and
            // the point light. This will make the scene darker.
            RenderSettings.ambientLight = new Color(0.18f, 0.18f, 0.18f, 1.0f);
            _light.light.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            
        }

        else if (timer > 20.0f && timer < 30.0f)
        {
            Debug.Log("Darker still...");

            // After twenty seconds make the renderer's abient light and the point light even darker.
            RenderSettings.ambientLight = new Color(0.08f, 0.08f, 0.08f, 1.0f);
            _light.light.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
        }

        else if (timer > 30.0f)
        {
            Debug.Log("Black. Game over.");

            // After thirty seconds turn everything black, and stop the game.
            RenderSettings.ambientLight = Color.black;
            _light.light.color = Color.black;
            Time.timeScale = 0;
        }
    }
}
