using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cage : MonoBehaviour {
    public int maxHealth;
    public int Health;
    public Slider healthbar;
    Slider newHealth;
    public Canvas MainUI;
    Vector3 barposition;
	// Use this for initialization
	void Start () {
        Health = maxHealth;
	}
    void Awake()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
    void Createhealthbar()
    {
        barposition = Camera.main.WorldToScreenPoint(transform.position);
        barposition = new Vector3(barposition.x,barposition.y - 5);

        newHealth = GameObject.Instantiate(healthbar, barposition, Quaternion.identity);
    }
    public void depricate()
    {

    }
}
