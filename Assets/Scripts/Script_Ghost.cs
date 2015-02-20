// Kenneth Gower
// GSP 497
// 2/15/2015
using UnityEngine;
using System.Collections;

public class Script_Ghost : MonoBehaviour 
{
    // States
    char idle = 'a';
    char search = 'b';
    char chase = 'c';
    char die = 'd';
    char scare = 'e';

    public char curState; // the current state of the ghost.
    // ********** //

    float fov;                  // The Field of View for the ghost.
    float viewLimit;            // How far the ghost can see.
    float distanceToPlayer;     // The distance from the ghost to the player.

    bool canSeePlayer;          // Bool for whether the ghost can see the player.

    GameObject _player;         // GameObject to access the player.
     
    

	// Use this for initialization
	void Start () 
    {
        this.GetComponent<Script_Behaviors>().getIdlePositions();   // Run getIdlePositions() in Script_Behaviors, this will set the positions to be used for the idle animation.
        curState = idle;    // Set the initial state to Idle.
        this.gameObject.GetComponent<Script_FSM>().setState(curState);  // Set the state in Script_FSM to the current state (Idle).   

        fov = 45.0f;
        viewLimit = 7.0f;

        canSeePlayer = false;

        _player = GameObject.FindGameObjectWithTag("Player");
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
        Debug.Log(distanceToPlayer);

        // Check if the Ghost can see the player, if it can change states to Chase.
        if ((Vector3.Angle(transform.position, _player.transform.position)) < fov && distanceToPlayer < viewLimit && transform.position.y > (_player.transform.position.y - 1.25f) && transform.position.y < (_player.transform.position.y + 1.25f)) // Detect if player is within the field of view
        {
            canSeePlayer = true;               
        }
        else
        {
            canSeePlayer = false;
        }
        // ********** //

        // If the ghost can see the player change to the Chase state.
        if (canSeePlayer)
        {
            if (distanceToPlayer > 4.0f)
            {
                Debug.Log("Entering Chase");
                curState = chase;
                this.gameObject.GetComponent<Script_FSM>().stateTransition(curState);
            }
            
            // If the ghost is close to the player change to the Scare state;
            if (distanceToPlayer < 4.0f)
            {
                Debug.Log("Entering Scare");
                curState = scare;
                this.gameObject.GetComponent<Script_FSM>().stateTransition(curState);
            }
        }
        else
        {
            // If we were in Idle, stay in Idle.
            if (curState == idle)
            {
                curState = idle;
            }
            // If we were in Chase change to Search.
            else if (curState == chase)
            {
                curState = search;
                this.gameObject.GetComponent<Script_FSM>().stateTransition(curState);
            }
            // If we were in Search, check how many times we've reached the search position. If we've reached the search position 4 times and haven't found
            // the player then change to the Die state.
            else if (curState == search)
            {
                Debug.Log("Script_Ghost value of searchCount: " + this.GetComponent<Script_Behaviors>().searchCount);
                if (this.GetComponent<Script_Behaviors>().searchCount == 4)
                {
                    curState = die;
                    this.gameObject.GetComponent<Script_FSM>().setState(curState);
                }
            }
            else if (curState == scare)
            {
                curState = search;
                this.gameObject.GetComponent<Script_FSM>().stateTransition(curState);
            }
        }
        //*******************************//

        // Execute whatever state the ghost is in.
        this.gameObject.GetComponent<Script_FSM>().executeState();
	}
}
