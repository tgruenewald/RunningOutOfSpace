using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public int time = 60;
    public int wood;
    public int Max_Hay = 100;
    public int Hayamount;
    public int TimeScale = 5;

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
    IEnumerator TimePass()
    {
        
        yield return new WaitForSeconds(TimeScale);
        time--;
        StartCoroutine(TimePass());
    }
}
