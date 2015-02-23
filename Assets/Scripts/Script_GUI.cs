// Kenneth Gower
// GSP 497
// 2/20/2015

/*This script handles all of the GUI details. In this script we:
 * * Cast a ray in order to highlight the pieces of candy in the game when they are under the reticle.
 * * Get the min and max bounds of the piece of candy for the highlight effect.
 * * When the player clicks on a piece of candy, increase the score and destroy the candy piece.
 * * Print a countdown timer and the current score.
 * * Display a reticle in the center of the screen.
 * * When the game is over show a game over screen with the player's score.
 */
using UnityEngine;
using System.Collections;

public class Script_GUI : MonoBehaviour 
{
    public Texture reticle; // Our reticle to guide the player, this way the player knows where the center of the screen is.
    float timer; // Variable for the GUI countdown timer.
    bool gameOver; // Bool so we can print the main HUD only if the game is not over.

    public int score; // the current score.

    GameObject[] _candies;
    Transform scaleCandy;
    Vector3 oldCandyScale;
    Color oldCandyColor;

    GameObject _ghost;

    bool addPoints;
    float pointTimer;

    void Start()
    {
        timer = 900.0f; // Set the countdown timer to start at 300 seconds. This is for ease of testing, it can be changed for normal gameplay.
        gameOver = false;
        addPoints = false;
        score = 0;       
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // Subtract using time.deltaTime, the same concept as in the Timer.cs, but backwards since we're counting down.

        _ghost = GameObject.FindGameObjectWithTag("Ghost");

        // Populate _candies with every candy object. As candies are grabbed by the player they will be removed from _candies.
        // It might be better to move this into the GetMouseButtonDown part of the code and first define _candies in Start().
        _candies = GameObject.FindGameObjectsWithTag("Candy"); 
 
        // From Unity3D Script Reference.
        // Casting a Ray from the center of the screen. When it hits something we check the tag, if it's tagged "Candy"
        // we put a highlight effect around it and if the player clicks on the Candy, we destroy the candy and increase the score.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Check to see if we were looking at a piece of candy. If we were scale the candy we've been looking back to normal.
            if (scaleCandy != null)
            {                   
                scaleCandyDown(scaleCandy);
            }
            
            if (hit.transform.tag == "Candy")
            {
                scaleCandy = hit.transform; // Set scaleCandy to the transform of the candy piece we're looking at. This way we can manipulate it outside of this if statement.
                scaleCandyUp(scaleCandy);

                // If the player clicks the left mouse button, destroy the GameObject that the Ray is hitting, then
                // increase the score by 10.
                if (Input.GetMouseButtonDown(0))
                {
                    Destroy(hit.transform.gameObject);
                    score += 10;
                    addPoints = true;
                    pointTimer = 0.0f;
                }                
            }
        }        
        // ++++++++++++++++++++ //

        // If a candy has been picked up, start the timer.
        if (addPoints)
        {
            pointTimer += Time.deltaTime;

            // If the timer is more than the limit, stop printing the new points gained.
            if (pointTimer > 0.5f)
            {
                addPoints = false;
            }
        }
        // ++++++++++++++++++++ //
    }

    // Scale the candy piece up as it's the target under the reticle.
    void scaleCandyUp(Transform candy)
    {
        scaleCandy = candy; // Set scaleCandy outside the Raycast if statement.
        oldCandyScale = candy.transform.localScale;     // Save the original candy scale.
        oldCandyColor = candy.renderer.material.color;  // Save the original candy color.
        candy.transform.localScale = new Vector3(candy.transform.localScale.x + 0.05f, 
                                                 candy.transform.localScale.y + 0.05f, 
                                                 candy.transform.localScale.z + 0.05f); // Scale the candy pice up by 0.05f.
        candy.renderer.material.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);  // Change the material color to yellow so it's mor obvious when we're targeting the candy bars.
    }
    // ++++++++++++++++++++ //

    // Restore the candy to it's normal size.
    void scaleCandyDown(Transform candy)
    {
        candy.transform.localScale = oldCandyScale;
        candy.renderer.material.color = oldCandyColor;
    }
    // ++++++++++++++++++++ //

    void OnGUI()
    {
        if (!gameOver)
        {
            // The reticle in the center of the screen.
            if (!reticle)
            {
                Debug.Log("Assign a Texture for 'reticle' in the inspector.");
                return;
            }

            GUI.DrawTexture(new Rect((Screen.width / 2), (Screen.height / 2), 10, 10), reticle, ScaleMode.ScaleToFit, true, 0.0f);
            // ++++++++++++++++++++ //

            // The countdown timer
            GUI.Label(new Rect(10, 10, 100, 20), "Time Left: " + timer.ToString("N0"));
            // ++++++++++++++++++++ //

            // Score counter
            GUI.Label(new Rect(10, 30, 100, 20), "Score: " + score.ToString());
            // ++++++++++++++++++++ //


            // Show the player they're losing points while the ghost is scaring them.
            if (_ghost != null)
            {
                if (_ghost.gameObject.GetComponent<Script_Behaviors>().subPoints)
                {
                    GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2 - 30), 100, 20), "-10");
                }
            }
            // ++++++++++++++++++++ //

            // Display the points a player gets over the reticle after picking up candy.
            if (addPoints)
            {
                GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2 - 30), 100, 20), "+10");
            }
            // ++++++++++++++++++++ //
        }

        // Game over text
        if (timer < 0.1)
        {
            gameOver = true;
            GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2), 100, 20), "Game Over");
            GUI.Label(new Rect((Screen.width / 2), ((Screen.height / 2) + 30), 100, 20), "Final Score: " + score.ToString());
        }
        // ++++++++++++++++++++ //

        // Game over win text
        if (_candies.Length == 0)
        {
            gameOver = true;
            Time.timeScale = 0;
            RenderSettings.ambientLight = Color.black;

            GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2) - 30, 500, 20), "Congratulations! You got all the candy!");
            GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2), 100, 20), "Game Over");
            GUI.Label(new Rect((Screen.width / 2), ((Screen.height / 2) + 30), 100, 20), "Final Score: " + score.ToString());
        }
    }
}
