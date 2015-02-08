// Kenneth Gower
// GSP 497
// 2/8/2015

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
    bool highlight; // Bool so we can highligh candy in the OnGUI function.

    Vector3[] pts = new Vector3[8]; // Array of Vector3's to hold the vertices of the bounds.
    Rect r; // Rect for the Highlight Gui box.

    int score; // the current score.

    void Start()
    {
        timer = 300.0f; // Set the countdown timer to start at 300 seconds. This is for ease of testing, it can be changed for normal gameplay.
        gameOver = false;
        highlight = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // Subtract using time.deltaTime, the same concept as in the Timer.cs, but backwards since we're counting down.

        // From Unity3D Script Reference.
        // Casting a Ray from the center of the screen. When it hits something we check the tag, if it's tagged "Candy"
        // we put a highlight effect around it and if the player clicks on the Candy, we destroy the candy and increase the score.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Make sure that highlight is set to false when we are not looking at a piece of candy.
            highlight = false;

            print("I'm looking at " + hit.transform.tag);
            if (hit.transform.tag == "Candy")
            {
                highlight = true;
                
                // Thanks to TowerOfBricks from the Unity3D forums for this chunk of code.
                // This will get the renderer bounds of the piece of candy that we're looking at, calculate the min and max positions
                // so we can make a rect with the min and max positions of the candy to use for our highlight effect. This is slightly modified
                // from TowerOfBricks' code found at: http://answers.unity3d.com/questions/292031/how-to-display-a-rectangle-around-a-player.html
                Bounds b = hit.transform.renderer.bounds;
                Camera cam = Camera.main;
                //All 8 vertices of the bounds
                pts[0] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
                pts[1] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
                pts[2] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
                pts[3] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
                pts[4] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
                pts[5] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
                pts[6] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
                pts[7] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

                //Get them in GUI space
                for (int i=0;i<pts.Length;i++) pts[i].y = Screen.height-pts[i].y;
                //Calculate the min and max positions
                Vector3 min = pts[0];
                Vector3 max = pts[0];
                for (int i = 1; i < pts.Length; i++)
                {
                    min = Vector3.Min(min, pts[i]);
                    max = Vector3.Max(max, pts[i]);
                }

                //Construct a rect of the min and max positions and apply some margin
                r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
                float margin = 0.5f;
                r.xMin -= margin;
                r.xMax += margin;
                r.yMin -= margin;
                r.yMax += margin;
                // ++++++++++++++++++++ //

                // If the player clicks the left mouse button, destroy the GameObject that the Ray is hitting, then
                // increase the score by 10.
                if (Input.GetMouseButtonDown(0))
                {
                    Destroy(hit.transform.gameObject);
                    score += 10;
                }
            }
        }
        else
        {
            highlight = false;
            print("I'm looking at nothing!");
        }
        // ++++++++++++++++++++ //        
    }

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

            // Highlight candy when it the reticle is targeting it.
            if (highlight)
            {
                GUI.Box(r, " ");
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
    }
}
