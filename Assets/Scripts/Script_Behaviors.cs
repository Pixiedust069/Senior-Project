﻿// Kenneth Gower
// GSP 497
// 2/20/2015

/*Script contains all the Ghost AI behaviors.
 * Idle bobs the ghost up and down in its spawn location.
 * Search picks a random SearchTarget and moves the that location. After doing that 4 times it destroys the ghost if it hasn't found the player again and moved to the Chase or Scare state.
 * Chase seeks the player's position.
 * Scare pulses all the lights in the scene, darkens the renderer's ambient light, and lowers the player score by 1 candy value (currently 10) every second.
 * 
 * Contains several helper functions to facilitate the behaviors.
 */

using UnityEngine;
using System.Collections;

public class Script_Behaviors : MonoBehaviour 
{
    GameObject _player; // The player's game object. This is used to get the player's transform.position for seeking the player.
    GameObject _manager;// The GameManager game object. This lets us get to the player's score.
    
    float speed;        // The ghost's movement speed.
    float rotSpeed;     // The ghost's rotation speed.
    Vector3 velocity;   // The ghost's movement velocity (direction).

    public Vector3 newPos;  // Position above the ghost's current position. For idle movement.
    public Vector3 oldPos;  // Current position.
    public Vector3 tmpPos;  // Tmp variable to swap old/new positions for idle animation.
    public bool up;         // Bool to switch between floating up or down during idle movement.

    Vector3 searchPos;          // The position the ghost will move to during it's search behavior.
    public int searchCount;     // The count of how many times it's picked a serach position.

    GameObject[] _lights;       // Array to access all the point lights in the scene.

    GameObject[] _searchTargets;// Array to access all the SearchTargets in the scene.

    float lPulseColor;  // Variable to change the color of the point lights in a pulsing effect.
    float pulseTimer;   // Timer for the pulse effect.
    float candyTimer;   // Timer for the candy drop. Lets us keep this independent from the pulse effect.
    bool jumpLeft;      // Bool for checking if the ghost is jumping left or right.
    bool darken;        // Bool for checking if the pulse should be dark or bright.

    Color oldAmbientLight;  // Color variable to hold the original color of the Renderer's Ambient light. This way we can restore it later.
    Color oldPointLights;   // Color variable to hold the original color of the point lights. This way we can restore it later.

    public bool subPoints;  // Bool to tell Script_UI that points should be taken from the player.

    public AudioClip ghostSound;    // Ghost default AudioClip, needs to be set in the Inspector.
    public AudioClip dieSound;      // Ghost death AudioClip, needs to be set in the Inspector.
    float soundDelay;   // Delay between playing the sound.
    float soundTimer;   // Timer for counting down to the sound delay.


	// Use this for initialization 
	void Start () 
    {   
        Random.seed = (int)System.DateTime.Now.Ticks;

        _lights = GameObject.FindGameObjectsWithTag("MainLight");               // Fill _lights with all the point lights in the scene.
        _searchTargets = GameObject.FindGameObjectsWithTag("SearchTarget");     // Fill _searchTargets with all the SearchTargets in the scene.
        _player = GameObject.FindGameObjectWithTag("Player");                   // Get the player game object.
        _manager = GameObject.FindGameObjectWithTag("GameManager");             // Get the game manager game object for access to Script_Timer and Script_GUI.
        
        velocity = new Vector3(0, 0, 0);    // Default value for velocity.

        speed = 3.0f;       // The movement speed of the ghost.
        rotSpeed = 1.5f;    // The rotation speed of the ghost.
        up = true;          // Set up to true for start. We know at spawn the ghost will at the bottom position so should float upward.
        lPulseColor = 1.0f; // Set pulse color to start at 1. We'll start bright then work down to darkness in the pulse effect.

        //jumpLeft = true;    
        darken = true;
        subPoints = false;
        pulseTimer = 0.0f;
        candyTimer = 0.0f;

        soundTimer = 0.0f;
        soundDelay = Random.Range(5.0f, 10.0f) + soundTimer;
	}

    public void idle()
    {
        // Make the Ghost float up and down in place like it's hovering.
        velocity = newPos - this.transform.position;

        velocity.Normalize();
        velocity *= (speed * 0.5f);

        this.transform.position += (velocity * Time.deltaTime);
        // ********** //

        // If the ghost is moving up and it is within 0.5f of the y for newPos start moving back down.
        if (up && this.transform.position.y >= (newPos.y -0.5f))
        {
            up = false;
            tmpPos = newPos;
            newPos = oldPos;
        }
        // If the ghost is moving down and it is within 0.5f of the y for newPos start moving up.
        else if (!up && this.transform.position.y <= (newPos.y +0.5f))
        {
            up = true;
            newPos = tmpPos;
        }
        // ********** //
    }

    public void getIdlePositions()
    {
        // Get the idle positions for this ghost's spawn point.
        newPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        oldPos = this.transform.position;
    }

    public void pickRandomPosition()
    {
        // Pick random positions for the Search behavior.
        int i = (int)Random.Range(0, _searchTargets.Length);
        searchPos = _searchTargets[i].transform.position;
    }

    public void search()
    {
        // Seek to the search position
        velocity = searchPos - this.transform.position;

        velocity.Normalize();
        velocity *= speed;

        this.transform.position += (velocity * Time.deltaTime);
        // ********** //

        // Rotate to face the player
        Quaternion targetRotation = Quaternion.LookRotation(searchPos - this.transform.position);
        float rSpeed = (rotSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rSpeed);
        // ********** //
        
        // When the ghost gets to the seek position add 1 to searchCount then pick a new random position to search.
        if (this.transform.position.x >= (searchPos.x - 1.0f) && this.transform.position.x <= (searchPos.x + 1.0f) || this.transform.position.z >= (searchPos.z - 1.0f) && this.transform.position.z <= (searchPos.z + 1.0f))
        {
            searchCount++;
            pickRandomPosition();
        }
        // ********** //
    }

    public void chase()
    {
        // Follow the player
        velocity = _player.transform.position - this.transform.position;

        velocity.Normalize();
        velocity *= speed;

        this.transform.position += (velocity * Time.deltaTime);
        // ********** //

        // Rotate to face the player
        Quaternion targetRotation = Quaternion.LookRotation(_player.transform.position - this.transform.position);
        float rSpeed = (rotSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rSpeed);
        // ********** //
    }

    public void die()
    {
        // Destroy the ghost.
        audio.Stop();
        playSound(dieSound);
        Destroy(this.gameObject);        
    }

    public void scare()
    {
        // Rotate to face the player
        Quaternion targetRotation = Quaternion.LookRotation(_player.transform.position - this.transform.position);
        float rSpeed = (rotSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rSpeed);
        // ********** //

        if (pulseTimer == 0.0f)
        {
            // Make the lights in the scene 'pulse' from light to dark and back.
            if (darken)
            {
                dark();
            }
            else
            {
                bright();
            }
            // ********** //

            // Jump to the left and right in a 'freaky' manner
            //if (jumpLeft)
            //{
            //    this.transform.position = new Vector3((this.transform.position.x), this.transform.position.y, this.transform.position.z - 0.5f);
            //    jumpLeft = false;
            //}
            //else
            //{
            //    this.transform.position = new Vector3((this.transform.position.x), this.transform.position.y, this.transform.position.z + 0.5f);
            //    jumpLeft = true;
            //}
            // ********** //            
        }

        if (candyTimer == 0.0f)
        {
            // Lower the players score as if the player was dropping candy due to fright.
            if (_manager.GetComponent<Script_GUI>().score > 0)
            {
                _manager.GetComponent<Script_GUI>().score -= 10;
                subPoints = true;
                _manager.GetComponent<Script_CandySpray>().spawnCandy();
            }
        }        

        if (candyTimer > 0.5f)
        {
            subPoints = false;            
        }
        
        
        // Timer to make the pulse effect feel right. Otherwise the effect would pulse by in less than a second.
        pulseTimer += Time.deltaTime;
        
        if (pulseTimer > 0.05f)
        {
            pulseTimer = 0.0f;
        }
        // ********** //

        // Timer for the taking the candy points away from the player. 
        candyTimer += Time.deltaTime;

        if (candyTimer > 1.0f)
        {
            candyTimer = 0.0f;
            destroyCandy();
        }
        // ********** //        
    }

    public void destroyCandy()
    {
        _manager.GetComponent<Script_CandySpray>().destroyCandy();
    }

    public void dark()
    {
        // Subtract from lPulseColor then loop through every light in the scene setting the new color.
        lPulseColor -= 0.05f;
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].light.color = new Color(lPulseColor, lPulseColor, lPulseColor, lPulseColor);
        }

        // If it's gotten dark enough, switch and start to brighten the lights instead.
        if (lPulseColor < 0.05f)
        {
            darken = false;
        }
    }

    // Stop Script_Timer from resetting the point lights each update.
    public void stopTimerDim()
    {
        _manager.GetComponent<Script_Timer>().canDimLights = false;
    }

    // Set Script_Timer to resetting the point lights again.
    public void startTimerDim()
    {
        _manager.GetComponent<Script_Timer>().canDimLights = true;
    }

    public void bright()
    {
        // Add to lPulseColor then loop through every light in the scene setting the new color.
        lPulseColor += 0.05f;
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].light.color = new Color(lPulseColor, lPulseColor, lPulseColor, lPulseColor);
        }

        // If it's gotten bright enough, switch and start to darken instead.
        if (lPulseColor > 1.0f)
        {
            darken = true;
        }
        
    }

    public void saveOldLights()
    {
        oldAmbientLight = RenderSettings.ambientLight;  // Save the original AmbientLight color for later use.
        oldPointLights = _lights[0].light.color;        // Save the original Point Light color for later use. Since all the lights in our scene use the same color this one variable is enough.
    }

    public void restoreOldLights()
    {
        RenderSettings.ambientLight = oldAmbientLight;  // Set the AmbientLight back to its original color.


        // Loop through all the Point Lights and set them to their original color.
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].light.color = oldPointLights;
        }
    }

    void playSound(AudioClip clip)
    {
        soundDelay = Random.Range(5.0f, 10.0f) + soundTimer;    // Pick a random time to delay the next ghost sound play.

        // Play the sound.
        audio.clip = clip;
        audio.volume = 0.5f;
        audio.Play();
    }

    void Update()
    {
        soundTimer += Time.deltaTime;   // Start the soundTimer.
        
        // If we have reached the time of the sound delay, play the ghost sound effect.
        if ((int)soundTimer == (int)soundDelay)
        {
            playSound(ghostSound);
        }
    }
}
