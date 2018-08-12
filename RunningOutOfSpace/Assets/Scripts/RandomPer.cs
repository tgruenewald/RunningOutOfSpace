using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPer : MonoBehaviour {
    public float RandomMax;
    public float UnicornPercent;
    public float RaptorPercent;
    public float BunnyPercent;
    public float FoxPercent;
    public float ChickenPercent;
    public float WoodPercent;
    public float HayPercent;
    bool Percentdontwork;
    
	// Use this for initialization
	void Start () {
		if(RandomMax > UnicornPercent + RaptorPercent + BunnyPercent + FoxPercent + ChickenPercent + WoodPercent + HayPercent)
        {
            Percentdontwork = true;
        }
        else
        {
            Percentdontwork = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject Loot()
    {
        float RanValue = Random.Range(0, RandomMax +1);
        if(Percentdontwork == true)
        {
            if (RanValue < UnicornPercent)
            {
                // return(Unicorn);
                return (null);
            }
            else if (RanValue < RaptorPercent && RanValue > UnicornPercent)
            {
                //return(Raptor);
                return (null);
            }
            else if (RanValue < BunnyPercent && RanValue > RaptorPercent)
            {
                //return(Bunny);
                return (null);
            }
            else if (RanValue < FoxPercent && RanValue > BunnyPercent)
            {
                //return(Fox);
                return (null);
            }
            else if (RanValue < ChickenPercent && RanValue > FoxPercent)
            {
                //return(Chicken);
                return (null);
            }
            else if (RanValue < HayPercent && RanValue > ChickenPercent)
            {
                //return(Hay);
                return (null);
            }
            else if (RanValue < WoodPercent && RanValue > HayPercent)
            {
                //return(Wood);
                return (null);
            }
            else
            {
                //return(nothing);
                return (null);
            }
        }
        else
        {
            return (null);
        }
    }
}
