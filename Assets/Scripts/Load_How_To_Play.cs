using UnityEngine;
using System.Collections;

public class Load_How_To_Play : MonoBehaviour {

	public void LoadScene (int level)
	{
		Application.LoadLevel (level);
		}

	public void Update ()
	{
		int level;
		if (Input.GetKey ("escape")) {
			Application.LoadLevel(0);
		}
	}
}
