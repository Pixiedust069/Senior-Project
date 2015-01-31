using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{

    GameObject _candy;

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
        Debug.Log("Collided!");
        if (collision.gameObject.tag == "Candy")
        {
            _candy = collision.gameObject;
            Debug.Log("SCORE!");
            Destroy(_candy);
        }
    }
}
