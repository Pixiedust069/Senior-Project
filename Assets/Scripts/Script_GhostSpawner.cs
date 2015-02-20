// Kenneth Gower
// GSP 497
// 2/20/2015

/*This script will pick a random time and place to spawn a ghost.*/
using UnityEngine;
using System.Collections;

public class Script_GhostSpawner : MonoBehaviour 
{
    public Transform ghost;     // Transform of the ghost. This must be set in the Inspector to a ghost Prefab.
    float timer;                // Timer to countdown to when we spawn a ghost.
    float ghostTimer;           // A random time for the timer to reach for the ghost to spawn.
    GameObject[] _ghostSpawns;  // An array of all the GhostSpawns in the game world. This allows us to pick a random spawn location every time we spawn a ghost.
    GameObject _ghost;          // A ghost game object.
    bool hasSpawned;            // Bool to tell if the ghost has been spawned. 
    bool timerSet;              // Bool to tell if the timer has been set.

	// Use this for initialization
	void Start () 
    {
        // Seed the random number generator.
        Random.seed = (int)System.DateTime.Now.Ticks;

        // Pick a random time for the ghost to spawn.
        ghostTimer = Random.Range(1.0f, 30.0f);

        // Put all GameObjects tagged "GhostSpawn into the array _ghostSpawns.
        _ghostSpawns = GameObject.FindGameObjectsWithTag("GhostSpawn");

        hasSpawned = false;

        print(ghostTimer);
	}
	
	// Update is called once per frame
	void Update () 
    {
        // If there is a ghost in the world it will populate _ghost.
        _ghost = GameObject.FindGameObjectWithTag("Ghost");

        // If there is no ghost in the world and the timer hasn't been set then set the timer.
        if (_ghost == null && !timerSet)
        {
            setTimer();
        }
        
        timer += Time.deltaTime;


        //spawn ghost
        if (timer > ghostTimer && !hasSpawned)
        {
            spawnGhost();                    
        }   	    
	}

    void spawnGhost()
    {
        //spawn ghost
        int i = (int)Random.Range(0.0f, 4.0f);
        Transform newGhost = (Transform)Instantiate(ghost, _ghostSpawns[i].transform.position, _ghostSpawns[i].transform.rotation);
        hasSpawned = true;
        timerSet = false;
    }

    void setTimer()
    {
        timer = 0.0f;
        ghostTimer = Random.Range(1.0f, 30.0f);
        timerSet = true;
        hasSpawned = false;
    }
}
