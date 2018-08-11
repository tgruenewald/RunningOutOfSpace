using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
	public List<Cell> neighbours = new List<Cell>();
	public Cell up;
	public Cell down;
	public Cell forward;
	public Cell reverse;

	public float x;
	public float y;

	// Use this for initialization
	void Start () {
		 FindNeighbours();
	}

	public Cell getForward() {
		return this.forward;
	}
	public Cell getReverse() {
		return this.reverse;
	}

	public Cell getUp() {
		return this.up;
	}	
	public Cell getDown() {
		return this.down;
	}	

	void FindNeighbours(){
        Cell[] tiles = FindObjectsOfType<Cell> ();
        foreach (Cell tile in tiles) {
            if (tile.gameObject.GetInstanceID() != gameObject.GetInstanceID()) {
                if (GetComponent<CircleCollider2D>().bounds.Intersects (tile.gameObject.GetComponent<Collider2D> ().bounds)) {
					tile.x = tile.gameObject.transform.position.x - gameObject.transform.position.x;
					tile.y = tile.gameObject.transform.position.y - gameObject.transform.position.y;
			
					if (tile.x > 0 && tile.y == 0) {
						this.forward = tile;
					}			
					if (tile.x < 0 && tile.y == 0) {
						this.reverse = tile;
					}			
					if (tile.y > 0 && tile.x == 0) {
						this.up = tile;
					}
					if (tile.y < 0 && tile.x == 0) {
						this.down = tile;	
					}					
					// need to be careful for numbers that are very small (because of misaligned tiles)
					if (tile.x == 0 || tile.y == 0) {
						// Debug.Log ("[" + gameObject.name + "] found a neighbour: " + tile.gameObject.name + ":  x = " + tile.x + ", y = " +  tile.y);

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
