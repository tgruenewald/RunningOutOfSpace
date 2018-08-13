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

    public float healthTime = 60f;

    public float reproTime = 10f;

    public float thoughtTime = 2f;
	// Use this for initialization
	void Start () {
        player = GameState.GetPlayerDroplet();
        Debug.Log("Starting health reduce");
        StartCoroutine(ReduceHealth(healthTime));
        StartCoroutine(Reproduce(reproTime));

	}

    private IEnumerator ThoughtTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.GetChild(13).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }    
    private IEnumerator Reproduce(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            // player.decrCageHeart(transform);
            if (player.cageHeartCount(transform) == 3 && player.cageAnimalCount(transform) > 1) {
                Transform animal = player.getACagedAnimal(transform);
                var newAnimal = (GameObject)Instantiate(Resources.Load("prefab/" + animal.gameObject.name.Replace("(Clone)","")), GetComponent<Transform>().position, GetComponent<Transform>().rotation) ;

                player.incrCagedAnimal(transform);
                newAnimal.transform.parent = transform;
                newAnimal.transform.localPosition = new Vector3(-2f, -1f, 0f);	                
            }

        }
    }
    private IEnumerator ReduceHealth(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.Log("Reducing health");
            if (player.cageAnimalCount(transform) > 0) {
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
    }

    public void activateThought(string animalName) {

        if (animalName == "rabbit(Clone)") {
            transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().sprite =  Resources.Load<Sprite>("images/hay");    
        }
        if (animalName == "chicken(Clone)") {
            transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().sprite =  Resources.Load<Sprite>("images/hay");    
        }
        if (animalName == "fox(Clone)") {
            transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().sprite =  Resources.Load<Sprite>("images/rabbit");    
        }
        if (animalName == "raptor(Clone)") {
            transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().sprite =  Resources.Load<Sprite>("images/fox");    
        }
        if (animalName == "unicorn(Clone)") {
            transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().sprite =  Resources.Load<Sprite>("images/raptor");    
        }
    

        transform.GetChild(13).GetChild(0).transform.localScale = new Vector2(0.5f, 0.5f); 
        transform.GetChild(13).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(13).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

        StartCoroutine(ThoughtTime(thoughtTime));

    }
    void Awake()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
    
}
