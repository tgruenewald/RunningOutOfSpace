using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public List<Cell> neighbours = new List<Cell> ();
	public float x;
	public float y;

	// Use this for initialization
	void Start () {
		 FindNeighbours();
	}

	void FindNeighbours(){
        Cell[] tiles = FindObjectsOfType<Cell> ();
		if (gameObject.name == "cell (1)") {
        foreach (Cell tile in tiles) {
            if (tile.gameObject.GetInstanceID() != gameObject.GetInstanceID()) {
                if (GetComponent<CircleCollider2D>().bounds.Intersects (tile.gameObject.GetComponent<Collider2D> ().bounds)) {
					tile.x = tile.gameObject.transform.position.x - gameObject.transform.position.x;
					tile.y = tile.gameObject.transform.position.y - gameObject.transform.position.y;

                    Debug.Log ("[" + gameObject.name + "] found a neighbour: " + tile.gameObject.name + ":  x = " + tile.x + ", y = " +  tile.y);
                    neighbours.Add (tile);
                }
            }
        }
		}
 
    }
	// Update is called once per frame
	void Update () {
		
	}
}
