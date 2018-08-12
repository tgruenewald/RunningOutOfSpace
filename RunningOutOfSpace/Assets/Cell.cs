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

	// for random selection
    public float RandomMax;
    public float UnicornPercent;
    public float RaptorPercent;
    public float BunnyPercent;
    public float FoxPercent;
    public float ChickenPercent;
    public float WoodPercent;
    public float HayPercent;	

	// Use this for initialization
	void Start () {
		 FindNeighbours();
		if (this.gameObject.name == "loot_drop") {	
			StartCoroutine(RandomlyGetLoot(2f));
		}	 
	}

	public Cell getFacingDirection(Player.Direction direction) {
		switch(direction) {
			case Player.Direction.Down:
				return getDown();
			case Player.Direction.Up:
				return getUp();
			case Player.Direction.Right:
				return getForward();

		}
		return getReverse();
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

	bool isVerySmall(float testForZero) {
		if (testForZero > -0.1 && testForZero < 0.1) {
			return true;
		}
		return false;
	}

	void FindNeighbours(){
        Cell[] tiles = FindObjectsOfType<Cell> ();
        foreach (Cell tile in tiles) {
            if (tile.gameObject.GetInstanceID() != gameObject.GetInstanceID()) {
                if (GetComponent<CircleCollider2D>().bounds.Intersects (tile.gameObject.GetComponent<Collider2D> ().bounds)) {
					tile.x = tile.gameObject.transform.position.x - gameObject.transform.position.x;
					tile.y = tile.gameObject.transform.position.y - gameObject.transform.position.y;
			
					if (tile.x > 0 && isVerySmall(tile.y)) {
						this.forward = tile;
					}			
					if (tile.x < 0 && isVerySmall(tile.y)) {
						this.reverse = tile;
					}			
					if (tile.y > 0 && isVerySmall(tile.x)) {
						this.up = tile;
					}
					if (tile.y < 0 && isVerySmall(tile.x)) {
						this.down = tile;	
					}					
					// need to be careful for numbers that are very small (because of misaligned tiles)
					if (isVerySmall(tile.x) || isVerySmall(tile.y)) {
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

    private IEnumerator RandomlyGetLoot(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
			if (transform.GetChild(0).GetComponent<TileContent>().cellContent == null) {
				// if not, randomly find an item
				// GameObject loot = GetLoot("wood");
				 GenerateLoot();
				

			}            


        }
    }	

	GameObject GetLoot(string item) {
		var gObj = (GameObject)Instantiate(Resources.Load("prefab/" + item), GetComponent<Transform>().position, GetComponent<Transform>().rotation) ;
		transform.GetChild(0).GetComponent<TileContent>().cellContent = gObj;
		return gObj;
		// currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent.transform.position = currentCell.getFacingDirection(lastDirection).transform.position;
		// lastChild.parent = currentCell.getFacingDirection(lastDirection).transform;		
	}

	void GenerateLoot() {
		float RanValue = Random.Range(0, RandomMax +1);
			Debug.Log("Random value:  " + RanValue);
            if (RanValue <= UnicornPercent)
            {
				GetLoot("unicorn");
                // return(Unicorn);
            }
            else if (RanValue <= RaptorPercent && RanValue > UnicornPercent)
            {
				GetLoot("raptor");
                //return(Raptor);
            }
            else if (RanValue <= BunnyPercent && RanValue > RaptorPercent)
            {
				GetLoot("rabbit");
                //return(Bunny);
            }
            else if (RanValue <= FoxPercent && RanValue > BunnyPercent)
            {
				GetLoot("fox");
                //return(Fox);
            }
            else if (RanValue <= ChickenPercent && RanValue > FoxPercent)
            {
				GetLoot("chicken");
                //return(Chicken);
            }
            else if (RanValue <= WoodPercent && RanValue > ChickenPercent)
            {
				GetLoot("cage");
                //return(Hay);
            }			
            else if (RanValue <= HayPercent && RanValue > WoodPercent)
            {
				GetLoot("hay");
                //return(Hay);
            }
            else
            {
				GetLoot("wood");
                //return(nothing);
            }		
	}
}
