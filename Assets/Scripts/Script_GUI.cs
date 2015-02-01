// Kenneth Gower
// GSP 497
// 2/1/2015

/*This script handles all of the GUI details. It gets score info
  from Script_Player, it also prints a countdown timer and
  a reticle in the center of the screen.*/
using UnityEngine;
using System.Collections;

public class Script_GUI : MonoBehaviour 
{

	public Texture reticle; // Our reticle to guide the player, this way the player knows where the center of the screen is.
    GameObject _player; // GameObject that will let us get the player's score.
    float timer; // Variable for the GUI countdown timer.
    bool gameOver; // Bool so we can print the main HUD only if the game is not over.

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        timer = 30.0f; // Set the countdown timer to start at 30 seconds. This is for ease of testing, it can be increased for normal gameplay.
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // Subtract using time.deltaTime, the same concept as in the Timer.cs, but backwards since we're counting down.
    }

    void OnGUI()
    {
        if (!gameOver)
        {
            // The reticle in the center of the screen.
            if (!reticle)
            {
                Debug.Log("Assign a Texture in the inspector.");
                return;
            }

            GUI.DrawTexture(new Rect((Screen.width / 2), (Screen.height / 2), 10, 10), reticle, ScaleMode.ScaleToFit, true, 0.0f);
            // ++++++++++++++++++++ //

            // The countdown timer
            GUI.Label(new Rect(10, 10, 100, 20), "Time Left: " + timer.ToString("N0"));
            // ++++++++++++++++++++ //

            // Score counter
            GUI.Label(new Rect(10, 30, 100, 20), "Score: " + _player.GetComponent<Script_Player>().score.ToString());
            // ++++++++++++++++++++ //
        }

        // Game over text
        if (timer < 0.1)
        {
            gameOver = true;
            GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2), 100, 20), "Game Over");
            GUI.Label(new Rect((Screen.width / 2), ((Screen.height / 2) + 30), 100, 20), "Final Score: " + _player.GetComponent<Script_Player>().score.ToString());
        }
        // ++++++++++++++++++++ //
    }
}
