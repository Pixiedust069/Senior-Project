// Kenneth Gower
// GSP 497
// 2/1/2015

/* This script controlls the player scoring. It handles colliding with candy and destroying the candy,
 * as well as adding to the accumulated score.*/

using UnityEngine;
using System.Collections;

public class Script_Player : MonoBehaviour 
{

    GameObject _candy; // GameObject that will let us destroy the Candy peice we collide with.
    public int score;

	// Use this for initialization
	void Start () 
    {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnCollisionEnter(Collision collision)
    {
        // Check to see if we have collided with a piece of candy.
        // If we have, assign the _candy variable to that game object, then destroy _candy.
        // Finally award the player 10 points.
         if (collision.gameObject.tag == "Candy")
        {
            _candy = collision.gameObject;
            Destroy(_candy);

            score += 10;
        }
    }
}
