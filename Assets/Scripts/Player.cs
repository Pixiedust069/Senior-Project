using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{

    GameObject _candy; // GameObject that will let us destroy the Candy peice we collide with.

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnCollisionEnter(Collision collision)
    {
        // Check to see if we have collided with a piece of candy.
        // If we have, assign the _candy variable to that game object, then destroy _candy.
         if (collision.gameObject.tag == "Candy")
        {
            _candy = collision.gameObject;
            Debug.Log("SCORE!");
            Destroy(_candy);
        }
    }
}
