using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cage : MonoBehaviour {

    public int pop = 0;
    public int hearts = 0; // 0 to 3
    public enum Health {Green, Yellow, Red};
    public Health health = Health.Yellow;

    Player player;

    public float healthTime = 2f;
	// Use this for initialization
	void Start () {
        player = GameState.GetPlayerDroplet();
        Debug.Log("Starting health reduce");
        StartCoroutine(ReduceHealth(healthTime));
	}

    private IEnumerator ReduceHealth(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.Log("Reducing health");
            player.decrCageHeart(transform);
            if (player.cageHeartCount(transform) == 0) {
                // destroy all animals
                Debug.Log("Removing all animals!!!!!!!!");
                player.removeAllAnimalsFromCage(transform);
            }
            else {
                Debug.Log("Still hearts left");
            }


        }
    }
    void Awake()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
    
}
