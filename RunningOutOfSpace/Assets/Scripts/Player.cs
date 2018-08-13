using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using UnityEngine.UI;

public class Player : MonoBehaviour {
			private BoxCollider2D boxCollider; 		//The BoxCollider2D component attached to this object.
		private Rigidbody2D rb2D;	
		public LayerMask blockingLayer;		
				private float inverseMoveTime;			//Used to make movement more efficient.
		public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
    public Sprite front;
	public enum Direction  {Right, Down, Up, Left};

	Player.Direction lastDirection = Direction.Right;
    public Sprite left;
    public Sprite back;
	private float move;
	public float maxSpeed = 3.0f;
	public float jumpForce = 20f;
	public bool grounded = true;
    public LayerMask whatIsGround;
	private CircleCollider2D groundCheck;
	private Animator animator;
	private bool facingRight = false;

	private bool stillMoving = false;

	
	private bool isTouchingMine = false;
	private bool isTouchingWall = false;

	private int thingsBeingHeld = 0;
	public GameObject[] inventory = new GameObject[6];

	public Vector3[] positionStack = new Vector3[]  { new Vector3(0,6f,0),
													  new Vector3(0,9.8f,0), 
													  new Vector3(0,13.5f,0), 
													  new Vector3(0,17.3f,0), 
													  new Vector3(0,20.5f,0), 
													  new Vector3(0,23.7f,0)	};

	private GameObject wall = null;
	private GameObject ladder = null;
	// Use this for initialization

	public Cell currentCell = null;
	Cell targetCell = null;
	 Vector3 pos;                                // For movement
 float speed = 2.0f;  

    public void SpawnAt(GameObject myPlayer)
    {
		//Camera.main.GetComponent<Camera2DFollow>().target = myPlayer.transform;
		Camera.main.transform.parent = myPlayer.transform;
		myPlayer.GetComponent<BoxCollider2D> ().enabled = true;

    }
	void Awake(){
		Debug.Log("Waking up");
		currentCell = GameObject.Find("starting_cell").GetComponent<Cell>();
		GameState.SetPlayerDroplet(this.gameObject);

		DontDestroyOnLoad (this.gameObject);
		GameState.hitButton = false;
		GameState.stayButton = false;
	}	
	void Start () {
		// groundCheck = GetComponent<CircleCollider2D>();
		animator = GetComponent<Animator>();
         pos = transform.position;          // Take the initial position
		 			boxCollider = GetComponent <BoxCollider2D> ();
			
			//Get a component reference to this object's Rigidbody2D
			rb2D = GetComponent <Rigidbody2D> ();
			
			//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
			inverseMoveTime = 1f / moveTime;

			currentCell = GameObject.Find("starting_cell").GetComponent<Cell>();

 //.GetComponent<Button>().interactable  = false;	

	}


             void OnEnable()
             {
              //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
                 SceneManager.sceneLoaded += OnLevelFinishedLoading;
             }
         
             void OnDisable()
             {
             //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
                 SceneManager.sceneLoaded -= OnLevelFinishedLoading;
             }	
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("Level Loaded");
		Debug.Log(scene.name);
		Debug.Log(mode);
		//Camera.main.GetComponent<Camera2DFollow>().target = gameObject.transform;
		currentCell = GameObject.Find("starting_cell").GetComponent<Cell>();
	}

	void LoadScene(string SceneToLoad)
	{
		Scene scene = SceneManager.GetSceneByName(SceneToLoad);
		if ((scene != null) && (!scene.isLoaded))
		{
			SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Additive);
		}
	}	
	
	// Update is called once per frame
	void Update () {
		// grounded = groundCheck.IsTouchingLayers (whatIsGround);

		// move = Input.GetAxis ("Horizontal");
		// if (move > 0 && !facingRight)
        // {
        //     Flip();
        // }
        // else if (move < 0 && facingRight)
        // {
        //     Flip();
        // }

		// if (move != 0) {
		// 	animator.SetBool("isMoving", true);
		// }
		// else {
		// 	animator.SetBool("isMoving", false);
		// }
			if (currentCell != null && currentCell.name.StartsWith("exit")) {
				string scene = currentCell.name;
				SpawnPoint.SwitchToLevel (this.gameObject);
				GameState.SetPlayerDroplet(this.gameObject);
				currentCell = null;
				LoadScene(scene);
				//SceneManager.LoadScene(scene);
			}

			if (currentCell != null && !stillMoving) {
				int horizontal = 0;  	//Used to store the horizontal move direction.
				int vertical = 0;		//Used to store the vertical move direction.
				
				//Check if we are running either in the Unity editor or in a standalone build.
				
				//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
				horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
				
				//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
				vertical = (int) (Input.GetAxisRaw ("Vertical"));	
				// Debug.Log("H: " + horizontal + ", V: " + vertical);	

				if(horizontal != 0)
				{
					vertical = 0;
				}

				if(horizontal != 0 || vertical != 0)
				{
					if (horizontal > 0) { 
						// move forward
						lastDirection = Direction.Right;
						targetCell = currentCell.getForward();


					}
					if (horizontal < 0) {
						// move backward
						lastDirection = Direction.Left;
						targetCell = currentCell.getReverse();
					}

					if (vertical > 0) { 
						// move up
						lastDirection = Direction.Up;
						targetCell = currentCell.getUp();					
					}

					if (vertical < 0) {
						// move down
						lastDirection = Direction.Down;
						targetCell = currentCell.getDown();					
					}
				
				//If Move returns true, meaning Player was able to move into an empty space.
					if (targetCell != null && targetCell.transform.GetChild(0).GetComponent<TileContent>().cellContent == null) {
						stillMoving = true;
						StartCoroutine (SmoothMovement (targetCell.transform.GetChild(0).transform.position));
						// Move (targetCell.transform.GetChild(0).transform.position.x, targetCell.transform.GetChild(0).transform.position.y, out hit);
					}

					// search for cage
					if (currentCell != null && currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent != null ) {
						Transform cellCage = getCage(targetCell.transform);
						Transform animal = getACagedAnimal(cellCage);
						if (animal != null) {
							cellCage.gameObject.GetComponent<Cage>().activateThought(animal.gameObject.name);
						}
					}

				}
				switch( horizontal)
				{
					case -1:
					this.GetComponent<SpriteRenderer>().sprite = left;
					this.GetComponent<SpriteRenderer>().flipX = false;
					break;
					case 1:
					this.GetComponent<SpriteRenderer>().sprite = left;
					this.GetComponent<SpriteRenderer>().flipX = true;
					break;
				}
				switch(vertical)
				{
					case 1:
					this.GetComponent<SpriteRenderer>().sprite = back;
					break;
					case -1:
					this.GetComponent<SpriteRenderer>().sprite = front;
					break;
				}
			}
		if (Input.GetKeyDown(KeyCode.Q)) {
			Debug.Log("Starting destroy all");
			destroyAllAnimalsInCage(lastDirection);
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			// manually reduce hearts
			Transform cage = findCage(currentCell.getFacingDirection(lastDirection).transform);
			Debug.Log("Cage is " + cage.transform.parent.gameObject.name);
			decrCageHeart(cage);

		}
		if (Input.GetKeyDown(KeyCode.F)) {
			//Debug.Log("Target cell name: " + targetCell.transform.GetChild(0).GetComponent<TileContent>().cellContent.GetComponent<SpriteRenderer>().sprite.name);
			// get one animal from cage
			removeAnimalFromCage(lastDirection);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			// pick up something

			if (targetCell != null && thingsBeingHeld < 6 && currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent != null) {
							// Transform[] items = GetComponentsInChildren<Transform>();
				// pick up animal
				currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent.transform.parent = gameObject.transform;
				currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent.transform.localPosition = positionStack[thingsBeingHeld];			
				currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent = null;
				thingsBeingHeld++;
			}

		
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			// drop something
			Debug.Log("Dropping");
			if (targetCell != null &&  thingsBeingHeld > 0 && currentCell != null && currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent == null) {
				Stack<Transform> stack = new Stack<Transform>();
				// Transform[] items = GetComponentsInChildren<Transform>();	
				foreach( Transform tr in transform) {
					Debug.Log("Name:  " + tr.name);
					stack.Push(tr);
				}
				Transform lastChild = stack.Pop();

				currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent = lastChild.gameObject;
				currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent.transform.position = currentCell.getFacingDirection(lastDirection).transform.position;
				lastChild.parent = currentCell.getFacingDirection(lastDirection).transform;
				thingsBeingHeld--;
			}
			else {
				// for all other drops
				
				
				string targetItem = GetCellName(currentCell.getFacingDirection(lastDirection));
				string currentItem = NameOfTopOfStack();

				Debug.Log("Placing down something else:  [" +currentItem +  "] --> [" + targetItem + "]");
				// wood + wood to make cage
				if (currentItem != null && currentItem == "wood") {
					if (targetItem != null) {
						if (targetItem == "wood") {
							// then make a cage
							Destroy(removeTopItem().gameObject);
							var gObj = (GameObject)Instantiate(Resources.Load("prefab/" + "cage"), GetComponent<Transform>().position, GetComponent<Transform>().rotation) ;

							placeItemInCell(lastDirection, gObj.transform);
						}
					}
				}


				// animals in cages
				if (currentItem != null && (currentItem == "hay" || currentItem == "rabbit" || currentItem == "chicken" || currentItem == "fox" || currentItem == "raptor" || currentItem == "unicorn" )) {
					if (targetItem != null) {
						if (targetItem == "good_cage") {
							//placeItemInCell(lastDirection, removeTopItem());
							addAnimalToCage(lastDirection);	

							// parent the animal to the cage 					
						}						
					}
				}

	

			}
			

		
		}			

    //  var dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    //  transform.position += dir * 5.0f * Time.deltaTime;

		
		//Debug.Log ("move = " + move);
	}
                       // Speed of movement
		//Move returns true if it is able to move and false if not. 
		//Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
		Transform getCage(Transform cellTransform) {
			Transform cellCage = null;
			foreach (Transform child in cellTransform) {
				if (child.gameObject.name == "cage(Clone)") {
					cellCage = child;
					break;
				}
			}
			return cellCage;
		}
		
		protected bool Move (float xDir, float yDir, out RaycastHit2D hit)
		{
			//Store start position to move from, based on objects current transform position.
			Vector2 start = transform.position;
			
			// Calculate end position based on the direction parameters passed in when calling Move.
			Vector2 end = start + new Vector2 (xDir, yDir);
			
			//Disable the boxCollider so that linecast doesn't hit this object's own collider.
			boxCollider.enabled = false;
			
			//Cast a line from start point to end point checking collision on blockingLayer.
			hit = Physics2D.Linecast (start, end, blockingLayer);
			
			//Re-enable boxCollider after linecast
			boxCollider.enabled = true;
			
			//Check if anything was hit
			if(hit.transform == null)
			{
				//If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
				StartCoroutine (SmoothMovement (end));
				
				//Return true to say that Move was successful
				return true;
			}
	
			//If something was hit, return false, Move was unsuccesful.
			return false;
		}
		
		
		//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
		protected IEnumerator SmoothMovement (Vector3 end)
		{
			//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
			//Square magnitude is used instead of magnitude because it's computationally cheaper.
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			
			//While that distance is greater than a very small amount (Epsilon, almost zero):
			while(sqrRemainingDistance > float.Epsilon)
			{
				//Find a new position proportionally closer to the end, based on the moveTime
				Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
				
				//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
				rb2D.MovePosition (newPostion);
				
				//Recalculate the remaining distance after moving.
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				
				//Return and loop until sqrRemainingDistance is close enough to zero to end the function
				yield return null;
			}
			currentCell = targetCell;
			stillMoving = false;
		}     

	void placeItemInCell(Direction dir, Transform item) {
		// remove any items already there.
		currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent = null;
        foreach (Transform child in currentCell.getFacingDirection(lastDirection).transform) {
			if (child.gameObject.name != "noah_marker")
				GameObject.Destroy(child.gameObject);
        }

		currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent = item.gameObject;
		currentCell.getFacingDirection(lastDirection).transform.GetChild(0).GetComponent<TileContent>().cellContent.transform.position = currentCell.getFacingDirection(lastDirection).transform.position;
		item.parent = currentCell.getFacingDirection(lastDirection).transform;

	}

	string getCagedAnimalName(Transform cell) {
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" ))
				return currentItem;
		}
		return null; 
	}

	public void removeAnimalsFromCells(Transform cell) {
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {
				if (animal != null) {
					animal.parent = null;
					Destroy(animal.gameObject);
				}
			}
		}		
	}

	public Transform getACagedAnimal(Transform cell) {
		if (cell != null) {
			foreach (Transform animal in cell.transform) {
				string currentItem =  animal.gameObject.name;
				if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {
					return animal;
				}
			}
		}
		return null;		
	}

	void disableSpriteByName(Transform cell, string name) {
		Debug.Log("dis - finding sprite:  " + name);
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if (currentItem == name) {
				animal.GetComponent<SpriteRenderer>().enabled = false;
			}
		}		
	}	
	void enableSpriteByName(Transform cell, string name) {
		Debug.Log("en - finding sprite:  " + name);
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if (currentItem == name) {
				animal.GetComponent<SpriteRenderer>().enabled = true;
			}
		}		
	}

	bool decrCagedAnimal(Transform cell) {
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {

			}
			else {
				if (currentItem.StartsWith("num_")) {
					if (animal.GetComponent<SpriteRenderer>().enabled) {
						// then this is the last one, so find next number
						string n = currentItem.Replace("num_", "");
						if (int.Parse(n) == 0) {
							return false;
						}
						animal.GetComponent<SpriteRenderer>().enabled = false;
						int next = int.Parse(n) - 1;
						enableSpriteByName(cell, "num_" + next);
						return true;
					}
				}
			}
			
		}
		return true;
	}	

	public bool incrCagedAnimal(Transform cell) {
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {

			}
			else {
				if (currentItem.StartsWith("num_")) {
					if (animal.GetComponent<SpriteRenderer>().enabled) {
						// then this is the last one, so find next number
						string n = currentItem.Replace("num_", "");
						if (int.Parse(n) >= 5) {
							return false;
						}
						animal.GetComponent<SpriteRenderer>().enabled = false;
						int next = int.Parse(n) + 1;
						enableSpriteByName(cell, "num_" + next);
						return true;
					}
				}
			}
			
		}
		return true;
	}


	void incrCageHeart(Transform cell) {
		int heartMax = 0;
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {

			}
			else {
				Debug.Log("incrCageHeart : currentItem " + currentItem);
				if (currentItem.StartsWith("heartfull_")) {

					if (animal.GetComponent<SpriteRenderer>().enabled) {
						// then this is the last one, so find next number
						string n = currentItem.Replace("heartfull_", "");
						Debug.Log("finding heart " + n);
						if (int.Parse(n) > heartMax) {
							heartMax = int.Parse(n);
						}
					}
				}
			}
			
		}

		int next = heartMax + 1;
		enableSpriteByName(cell, "heartfull_" + next);
		disableSpriteByName(cell, "heartempty_" +  next);		

	}

	public int cageAnimalCount(Transform cell) {
		int count = 0;
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {
				count++;
			}
		}		
		return count;
	}

	public int cageHeartCount(Transform cell) {
		int count = 0;
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {

			}
			else {
				if (currentItem.StartsWith("heartfull_")) {
					if (animal.GetComponent<SpriteRenderer>().enabled) {		
						++count;
					}			
				}
			}
		}		
		return count;
	}

	public void decrCageHeart(Transform cell) {
	
		Debug.Log("Removing a heart");
		int heartMax = 0;
		foreach (Transform animal in cell.transform) {
			string currentItem =  animal.gameObject.name;
			if ((currentItem == "rabbit(Clone)" || currentItem == "chicken(Clone)" || currentItem == "fox(Clone)" || currentItem == "raptor(Clone)" || currentItem == "unicorn(Clone)" )) {

			}
			else {
				if (currentItem.StartsWith("heartfull_")) {

					if (animal.GetComponent<SpriteRenderer>().enabled) {
						// then this is the last one, so find next number
						string n = currentItem.Replace("heartfull_", "");
						if (int.Parse(n) > heartMax) {
							heartMax = int.Parse(n);
						}
					}
				}
			}
			
		}
		if (heartMax > 0) {
			int next = heartMax;
			enableSpriteByName(cell, "heartempty_" + next);
			disableSpriteByName(cell, "heartfull_" +  next);	
		}		

	}	


	void emptyCage(Transform cell) {
		
	}	

	Transform findCage(Transform cell) {
		foreach (Transform child in cell) {
			if (child.gameObject.name == "cage(Clone)") {
				return child;
			}

		}
		return null;
	}

	public void removeAllAnimalsFromCage(Transform cage) {
		int counter = 10;
		Debug.Log("removeAllAnimalsFromCage:  entering while loop");
		while (decrCagedAnimal(cage)) {
			Debug.Log("in loop");
			Transform animal = getACagedAnimal(cage);
			Debug.Log("animal:  " + animal);
			if (animal != null) {
				animal.parent = null;
				Destroy(animal.gameObject);
			}

			counter--;
			if (counter <= 0) {
				Debug.Log("baling on counter");
				break;
			}

		}
	}

	void destroyAllAnimalsInCage(Direction dir) {
		Debug.Log("destroy all animals");
		Transform cage = findCage(currentCell.getFacingDirection(lastDirection).transform);
		if (cage != null) {
			removeAllAnimalsFromCage(cage);

		}
		Debug.Log("exiting destroy all");
	}

	void removeAnimalFromCage(Direction dir) {
		// use Q for fetch single animal
		if (thingsBeingHeld < 6) {
			Transform cage = findCage(currentCell.getFacingDirection(lastDirection).transform);
			if (cage != null) {
				
				string existingAnimal = getCagedAnimalName(cage);
				Debug.Log("Animal to be removed:  " + existingAnimal);
				if (decrCagedAnimal(cage)) {
					Transform animal = getACagedAnimal(cage);

					animal.parent = gameObject.transform;
					animal.localPosition = positionStack[thingsBeingHeld]; 
					thingsBeingHeld++;					
				}


			}
		}
	}

	void addAnimalToCage(Direction dir) {
		Transform cage = findCage(currentCell.getFacingDirection(lastDirection).transform);
		if (cage != null) {
			
			string existingAnimal = getCagedAnimalName(cage);
			Debug.Log("Existing animal:  " + existingAnimal);
			string topAnimal = NameOfTopOfStack() + "(Clone)";

			// adding scenario
			if (existingAnimal == null || topAnimal == existingAnimal) {
				if (incrCagedAnimal(cage)) {
					// then there is enough room
					Transform animal =  removeTopItem();
					animal.parent = cage;
					animal.transform.localPosition = new Vector3(-2f, -1f, 0f);					
				}
			}
			else if (topAnimal != existingAnimal) {
				if (topAnimal == "hay(Clone)") {
					if ((existingAnimal == "rabbit(Clone)") || (existingAnimal == "chicken(Clone)")) {
						Transform animal =  removeTopItem();
						Destroy(animal.gameObject);
						incrCageHeart(cage);						
					}					
				}
				// feeding scenario
				if ((topAnimal == "rabbit(Clone)") && (existingAnimal != "chicken(Clone)")) {
					// then feed the rabbit to it
						Transform animal =  removeTopItem();
						Destroy(animal.gameObject);
						incrCageHeart(cage);					
				}
				if ((topAnimal == "chicken(Clone)") && (existingAnimal != "rabbit(Clone)")) {
					// then feed the chicken to it
					if (existingAnimal == "raptor(Clone)" || existingAnimal == "fox(Clone)") {
						// destroyAllAnimalsInCage(dir);
						Debug.Log("Feeding chicken to raptor");
						Transform animal =  removeTopItem();
						Destroy(animal.gameObject);
						incrCageHeart(cage);

						// incrCagedAnimal(cage);
						// animal.parent = cage;
						// animal.transform.localPosition = new Vector3(-2f, -1f, 0f);	
					}					
				}
				if (topAnimal == "fox(Clone)") {
					// then feed the fox to it
					if (existingAnimal == "rabbit(Clone)" || existingAnimal == "chicken(Clone)") {
						destroyAllAnimalsInCage(dir);
						Transform animal =  removeTopItem();
						incrCagedAnimal(cage);
						animal.parent = cage;
						animal.transform.localPosition = new Vector3(-2f, -1f, 0f);	
					}
				}
				if (topAnimal == "raptor(Clone)") {
					// then feed the raptor to it
					Debug.Log("Feeding raptor to rabbits");
					if (existingAnimal == "rabbit(Clone)" || existingAnimal == "chicken(Clone)" || existingAnimal == "fox(Clone)") {
						destroyAllAnimalsInCage(dir);
						Transform animal =  removeTopItem();
						incrCagedAnimal(cage);
						animal.parent = cage;
						animal.transform.localPosition = new Vector3(-2f, -1f, 0f);	
					}
				}
				if (topAnimal == "unicorn(Clone)") {
					// then feed the raptor to it
					if (existingAnimal == "raptor(Clone)") {
						destroyAllAnimalsInCage(dir);
						Transform animal =  removeTopItem();
						incrCagedAnimal(cage);
						animal.parent = cage;
						animal.transform.localPosition = new Vector3(-2f, -1f, 0f);	
					}
				}				
																
			}

		}
	}
	Transform removeTopItem() {
		if (thingsBeingHeld > 0) {
			Stack<Transform> stack = new Stack<Transform>();
			// Transform[] items = GetComponentsInChildren<Transform>();	
			foreach( Transform tr in transform) {
				stack.Push(tr);
			}
			thingsBeingHeld--;
			Transform lastChild = stack.Pop();	
			return lastChild;	
		}
		else {
			return null;
		}
	}
	string NameOfTopOfStack() {
		Stack<Transform> stack = new Stack<Transform>();
		// Transform[] items = GetComponentsInChildren<Transform>();	
		foreach( Transform tr in transform) {
			Debug.Log("Name:  " + tr.name);
			stack.Push(tr);
		}
		Transform lastChild = stack.Peek();	
		if (lastChild != null) {
			return lastChild.gameObject.name.Replace("(Clone)", "");
		}
		else {
			return null;
		}
			
	}
	string GetCellName(Cell cell) {
		if (cell != null && cell.transform.GetChild(0).GetComponent<TileContent>().cellContent != null)
			return cell.transform.GetChild(0).GetComponent<TileContent>().cellContent.GetComponent<SpriteRenderer>().sprite.name;
		else
			return null;
	}
    void Flip()
    {
        //Debug.Log("switching...");
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }	
	void OnTriggerEnter2D(Collider2D coll){
		// if (coll.gameObject.tag == "cell" ) {
		// 	// start black jack
		// 	Debug.Log("touch cell:  " + coll.gameObject.name);
		// 	currentCell = coll.gameObject.GetComponent<Cell>();

		// }		
		if (coll.gameObject.tag == "stand" ) {
			// start black jack
			Debug.Log("Entering stand");
			if (GameState.gold > 0 && !GameState.isPlayingBlackJack) {


				// Debug.Log("Start the deal");
				// GameState.isPlayingBlackJack = true;
				// GameObject dealerSpawn = GameObject.Find("Dealer1SpawnPoint");
				// dealerSpawn.GetComponent<BlackJack>().deal();
			}
		}
		if (coll.gameObject.tag == "minotaur" ) {
			SpawnPoint.SwitchToLevel (this.gameObject);
			Debug.Log("Loading scene2");
			SceneManager.LoadScene("scene1");
		}			
		if (coll.gameObject.tag == "ladder" ) {
			SpawnPoint.SwitchToLevel (this.gameObject);
			Debug.Log("Loading scene2");
			SceneManager.LoadScene("scene2");
		}	
		if (coll.gameObject.tag == "broken_wall" ) {
			isTouchingWall = true;
			wall = coll.gameObject;

		}

	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "wall" ) {
			isTouchingWall = false;
		}		
		if (coll.gameObject.tag == "stand" ) {
			// isTouchingStand = false;
		}		
		if (coll.gameObject.tag == "gold" ) {
			// start black jack
			Debug.Log("not touch gold");
			isTouchingMine = false;
		}			
	}
}
