using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public int time = 5;
    public int wood;
    public int Max_Hay = 100;
    public int Hayamount;
    public int TimeScale = 1; // 5

    // Use this for initialization
    void Start () {
        StartCoroutine(TimePass());
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    void FixedUpdate()
    {
        
        if (Hayamount > Max_Hay)
        {
            Hayamount = Max_Hay;
        }
    }
    public int Maxhay()
    {
        return (Max_Hay);
    }
    public int NumHay()
    {
        return (Hayamount);
    }
    public int NumWood()
    {
        return (wood);
    }
    public int Timeleft()
    {
        return (time);
    }
    IEnumerator FortyDays()
    {
        GameObject text = GameObject.Find("FloodTime");
        time = 40;
        while (time > 0) {
            yield return new WaitForSeconds(TimeScale);
            text.GetComponent<Text>().text = "Days to survive " + time;
            time--;                   
        }

        // TODO: you made it.  Load the next screen
    }
    IEnumerator FloodTime() {
        yield return new WaitForSeconds(5f);
        GameObject.Find("close_door").transform.GetChild(0).GetComponent<TileContent>().cellContent =  GameObject.Find("starting_cell");;
        GameObject.Find("ramp").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("earth3").GetComponent<SpriteRenderer>().enabled = false;  
        StartCoroutine(FortyDays());      
    }
    IEnumerator TimePass()
    {
        GameObject text = GameObject.Find("FloodTime");
        while (time > 0) {
            yield return new WaitForSeconds(TimeScale);
            text.GetComponent<Text>().text = "Days Remaining Until Flood " + time;
            time--;            

        }
        text.GetComponent<Text>().text = "Return to ark";
        // flood starts

        // rename starting_cell
        // rename starting_cell_rain to starting_cell
        GameObject oldStart = GameObject.Find("starting_cell_rain");
        GameObject currentStart = GameObject.Find("starting_cell");
        currentStart.name = "old";
        oldStart.name = "starting_cell";
        Player player = GameState.GetPlayerDroplet();
        SpawnPoint.SwitchToLevel(player.gameObject);

        StartCoroutine(FloodTime());

        // find ramp make invinsible

        // find earth3 "    "
        // close_door delete




        // StartCoroutine(TimePass());
    }
}
