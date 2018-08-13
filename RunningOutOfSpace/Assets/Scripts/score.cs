using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class score : MonoBehaviour {

	public Text scoreText;
	// Use this for initialization
	void Start () {
		scoreText.text = "rabbit = " + GameState.rabbit + "\n" +
		 "chicken = " + GameState.chicken + "\n" +
		 "fox = " + GameState.fox + "\n" +
		 "raptor = " + GameState.raptor + "\n" +
		 "unicorn = " + GameState.unicorn;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
