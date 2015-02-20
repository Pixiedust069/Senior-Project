using UnityEngine;
using System.Collections;

public class Script_CandySpray : MonoBehaviour 
{
    public Transform candy;
    GameObject _player;
    GameObject newCandyObj;
    bool hasSpawned;

	// Use this for initialization
	void Start () 
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        hasSpawned = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void spawnCandy()
    {
        if (!hasSpawned)
        {
            Transform newCandy = (Transform)Instantiate(candy, (_player.gameObject.transform.position) + _player.transform.forward, _player.transform.rotation);
            newCandyObj = newCandy.gameObject;
            newCandy.transform.localScale = new Vector3(newCandy.transform.localScale.x + 5.0f, newCandy.transform.localScale.y + 5.0f, newCandy.transform.localScale.z + 5.0f);
            newCandyObj.rigidbody.AddForce(Vector3.up * 250);
            hasSpawned = true;
        }
    }

    public void destroyCandy()
    {
        Destroy(newCandyObj);
        hasSpawned = false;
    }
}
