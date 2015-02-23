using UnityEngine;
using System.Collections;

public class Load_Game_Scene : MonoBehaviour {

	public void LoadScene (int level)
	{
		Application.LoadLevel (level);
		}
}
