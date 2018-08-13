using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class title : MonoBehaviour {

       void Awake()
       {
		   if (GameState.myaudioSource == null) {
			   GameState.myaudioSource = (GameObject) Instantiate(Resources.Load("prefab/MyAudioSource"));
			   DontDestroyOnLoad(GameState.myaudioSource);
		   }
       }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void tut() {
		SceneManager.LoadScene("tut");
	}	
	public void start() {
		SceneManager.LoadScene("exit_ark");
	}
	public void credits() {
		SceneManager.LoadScene("credits");
	}	

	public void title_scene() {
		SceneManager.LoadScene("title");
	}		
}
