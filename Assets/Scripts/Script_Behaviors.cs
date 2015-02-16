﻿// Kenneth Gower
// GSP 497
// 2/15/2015

using UnityEngine;
using System.Collections;

public class Script_Behaviors : MonoBehaviour 
{
    GameObject _player; // The player's game object. This is used to get the player's transform.position for seeking the player.
    
    float speed;        // The ghost's movement speed.
    float rotSpeed;     // The ghost's rotation speed.
    Vector3 velocity;   // The ghost's movement velocity (direction).

    public Vector3 newPos;  // Position above the ghost's current position. For idle movement.
    public Vector3 oldPos;  // Current position.
    public Vector3 tmpPos;  // Tmp variable to swap old/new positions for idle animation.
    public bool up;         // Bool to switch between floating up or down during idle movement.

    Vector3 searchPos;          // The position the ghost will move to during it's search behavior.
    public int searchCount;     // The count of how many times it's picked a serach position.

	// Use this for initialization
	void Start () 
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        _player = GameObject.FindGameObjectWithTag("Player");
        velocity = new Vector3(0, 0, 0);
        speed = 3.0f;
        rotSpeed = 1.5f;
        up = true;
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
        float newX = this.transform.position.x + Random.Range(2.0f, 5.0f);
        float newZ = this.transform.position.z + Random.Range(2.0f, 5.0f);

        searchPos = new Vector3(newX, this.transform.position.y, newZ); 
    }

    public void search()
    {
        // Seek to the search position
        velocity = searchPos - this.transform.position;

        velocity.Normalize();
        velocity *= speed;

        this.transform.position += (velocity * Time.deltaTime);
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
        Destroy(this.gameObject);
    }
}