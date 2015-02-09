// Kenneth Gower
// GSP 497
// 2/8/2015

/*This script will pick a random time and place to spawn a ghost.*/
using UnityEngine;
using System.Collections;

public class Script_GhostSpawner : MonoBehaviour 
{
    public Transform ghost;
    float timer;
    float ghostTimer;
    GameObject[] _ghostSpawns;
    bool hasSpawned;

	// Use this for initialization
	void Start () 
    {
        timer = 0.0f;
        Random.seed = (int)System.DateTime.Now.Ticks;

        ghostTimer = Random.Range(1.0f, 10.0f);
        _ghostSpawns = GameObject.FindGameObjectsWithTag("GhostSpawn");

        hasSpawned = false;

        print(ghostTimer);
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;

        if (timer > ghostTimer && !hasSpawned)
        {
            //spawn ghost
            int i = (int)Random.Range(0.0f, 4.0f);
            Debug.Log("Spawning in " + i);
            Transform newGhost = (Transform) Instantiate(ghost, _ghostSpawns[i].transform.position, _ghostSpawns[i].transform.rotation);
            hasSpawned = true;            
        }   	    
	}
}
