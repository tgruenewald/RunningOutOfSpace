using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
	void Awake() {
		var playerDroplet = GameState.GetPlayerDroplet();
		if (playerDroplet != null)
		{
			Debug.Log("Getting existing player");
			playerDroplet.SpawnAt(playerDroplet.gameObject);

		}
		else
		{
			Debug.Log("First time creating droplet");
			var gObj = (GameObject)Instantiate(Resources.Load("prefab/noah"), GetComponent<Transform>().position, GetComponent<Transform>().rotation) ;
			Debug.Log("Droplet is " + gObj);
			gObj.GetComponent<Player>().SpawnAt(gObj);
		}		
	}
	void Start () {


	}

	public static void SwitchToLevel(GameObject playerObject)
	{
		playerObject.GetComponent<Transform>().position = GameObject.FindObjectsOfType<SpawnPoint>()[0].GetComponent<Transform>().position;
		playerObject.GetComponent<BoxCollider2D> ().enabled = false;
		GameState.SetPlayerDroplet(playerObject);
		//GameState.GetPlayerDroplet().StopAllAudio();
	}	
}
