using UnityEngine;
using System.Collections;

public class Escape_Button : MonoBehaviour {

	public void Update ()
	{
		int level;
		if (Input.GetKey ("escape")) {
			Application.LoadLevel(5);
		}
	}
}
