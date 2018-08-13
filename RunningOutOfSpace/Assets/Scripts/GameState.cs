using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class GameState
{
    private static GameObject droplet = null;
    public static GameObject introMusic = null;

	public static int gold = 0;
	 
	public static int stone = 0;

	public static int[] cards = new int[] { 2, 3, 4,10,  5, 6, 10, 7, 8,10,  9, 10 };
	public static float[] offsets = new float[] { 0f, 0f, 0f};
	public static string[] cardSpawns = new string[] {"Dealer1SpawnPoint", "PlayerHandSpawnPoint", "Dealer2SpawnPoint"};
	public static int hiddenCard = 0;
	public static GameObject hiddenCardBack;
	public static bool isPlayingBlackJack = false;

	public static int dealerTotal = 0;
	public static int playerTotal = 0;

	public static bool hitButton = false;
	public static bool stayButton = false;

	public static int rabbit = 0;
	public static int chicken = 0;
	public static int fox = 0;
	public static int raptor = 0;	
	public static int unicorn = 0;

	public static GameObject myaudioSource = null;


	public static void SetPlayerDroplet(GameObject droplet){
		GameState.droplet = droplet;
	}

	public static Player GetPlayerDroplet(){
		if(droplet == null){
			droplet = GameObject.FindGameObjectWithTag("Player");
			if(droplet == null) {
                Debug.Log("Getting player droplet but it is null");
                return null;
            }
				
		}

		return droplet.GetComponent<Player>();
	}
}