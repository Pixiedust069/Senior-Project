﻿// Kenneth Gower
// GSP 497
// 2/1/2015

/* This script will start a timer at the beginning of the game. As the game
   progresses, the timer will increase and the screen will grow dimmer. This
   is a very basic first attempt. */
using UnityEngine;
using System.Collections;

public class Script_Timer : MonoBehaviour
{

    float timer; // variable for our timer.
    GameObject[] _lights; // Array to access the all point lights in the test scene. 

    public bool canDimLights;

   
    // Use this for initialization
    void Start()
    {
        Screen.showCursor = false;

        timer = 0.0f;

        _lights = GameObject.FindGameObjectsWithTag("MainLight");

        canDimLights = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        
        // The countdown timer is set for 30 seconds. This is for ease of testing, it can be increased for normal gameplay.
        // I want to try and add a pulsating effect rather just a static dimming.
        if (timer > 60.0f && timer < 90.0f)
        {
            // After ten seconds have passed change the color of the renderer's ambient light settings and
            // the point light. This will make the scene darker.
            RenderSettings.ambientLight = new Color(0.18f, 0.18f, 0.18f, 1.0f);
            if (canDimLights)
            {
                for (int i = 0; i < _lights.Length; i++)
                {
                    //_lights[i].light.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                    _lights[i].light.color = new Color(0.56f, 0.30f, 0.06f, 1.0f);
                }
            }
            
        }

        else if (timer > 90.0f && timer < 120.0f)
        {
            // After twenty seconds make the renderer's abient light and the point light even darker.
            RenderSettings.ambientLight = new Color(0.08f, 0.08f, 0.08f, 1.0f);
            if (canDimLights)
            {
                for (int i = 0; i < _lights.Length; i++)
                {
                    //_lights[i].light.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
                    _lights[i].light.color = new Color(0.29f, 0.15f, 0.03f, 1.0f);
                }
            }
        }

        else if (timer > 120.0f)
        {
            // After thirty seconds turn everything black, and stop the game.
            RenderSettings.ambientLight = Color.black;
            for (int i = 0; i < _lights.Length; i++)
            {
                _lights[i].light.color = Color.black;
            }
            Time.timeScale = 0;
        }
		
			int level;
			if (Input.GetKey ("escape"))
		{
            Screen.showCursor = true;
				Application.LoadLevel(5);
		}

    }
}
