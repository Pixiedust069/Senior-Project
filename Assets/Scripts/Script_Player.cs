// Kenneth Gower
// GSP 497
// 2/1/2015

/* This script controlls the player scoring. It handles colliding with candy and destroying the candy,
 * as well as adding to the accumulated score.*/

using UnityEngine;
using System.Collections;

public class Script_Player : MonoBehaviour 
{
    // Public AudioClips so that they can be set in the Inspector.
    public AudioClip stoneFloor;
    public AudioClip woodFloor;


    bool playingSound;  // Bool for checking whether we are currently playing a sound.
    bool playerWalking; // Bool for checking if we are walking.

    char groundType;    // Char to differentiate between the ground types (Stone and Wood).
    char currentGround; // Char to hold what we are currently walking on, this way we can check for the ground type changing mid-movement.

	// Use this for initialization
	void Start () 
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
        playingSound = false;
        playerWalking = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        // If the buttons for Horizontal or Vertical movement are being pressed then we are walking.
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            playerWalking = true;
        }
        else
        {
            playerWalking = false;
        }

        // If we are not paying a sound, and we are walking, and we are on the Stone Floor, play the StoneFloor sound
        // and set playingSound to true, also set the current ground.
        if (!playingSound && playerWalking && groundType == 'a')
        {
            currentGround = 'a';
            playingSound = true;
            playSound(stoneFloor);
        }
        // If we are not paying a sound, and we are walking, and we are on the Wood Floor, play the WoodFloor sound
        // and set playingSound to true, also set the current ground.
        else if (!playingSound && playerWalking && groundType == 'b')
        {
            currentGround = 'b';
            playingSound = true;
            playSound(woodFloor);
        }
        // If we are currently playing a sound, and we are walking, and the ground type has changed, change to the proper sound.
        // Set currentGround to the new ground type.
        else if (playingSound && playerWalking && currentGround != groundType)
        {
            currentGround = groundType;
            audio.Stop();
            if (groundType == 'a')
                playSound(stoneFloor);
            else
                playSound(woodFloor);
        }
        // If we are not moving then stop the sound effects.
        else if (!playerWalking)
        {
            playingSound = false;
            audio.Stop();
        }
    }   

    // Function for playing the sound clip. Takes in the needed sound an plays it.
    void playSound(AudioClip clip)
    {
        audio.clip = clip;
        audio.volume = 1.0f;
        audio.Play();
    }

    // This is a specific collider function to use when dealing with Character Controllers.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If the object we are colliding with is the Stone Floor set the groundType variable to 'a'.
        if (hit.gameObject.tag == "StoneFloor")
        {
            groundType = 'a';
        }

        // If the object we are colliding with is the Wood Floor set the groundType variable to 'b'.
        if (hit.gameObject.tag == "WoodFloor")
        {
            groundType = 'b';
        }
    }
}
