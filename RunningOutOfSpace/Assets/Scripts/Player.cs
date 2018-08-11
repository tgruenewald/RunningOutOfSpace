﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityStandardAssets._2D;
using UnityEngine.UI;

public class Player : MonoBehaviour {
			private BoxCollider2D boxCollider; 		//The BoxCollider2D component attached to this object.
		private Rigidbody2D rb2D;	
		public LayerMask blockingLayer;		
				private float inverseMoveTime;			//Used to make movement more efficient.
		public float moveTime = 0.1f;			//Time it will take object to move, in seconds.

	private float move;
	public float maxSpeed = 3.0f;
	public float jumpForce = 20f;
	public bool grounded = true;
    public LayerMask whatIsGround;
	private CircleCollider2D groundCheck;
	private Animator animator;
	private bool facingRight = false;

	
	private bool isTouchingMine = false;
	private bool isTouchingWall = false;

	private GameObject wall = null;
	private GameObject ladder = null;
	// Use this for initialization
	 Vector3 pos;                                // For movement
 float speed = 2.0f;  

    public void SpawnAt(GameObject myPlayer)
    {
		// Camera.main.GetComponent<Camera2DFollow>().target = myPlayer.transform;
		myPlayer.GetComponent<BoxCollider2D> ().enabled = true;

    }
	void Awake(){
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

 //.GetComponent<Button>().interactable  = false;	

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

		if (Input.GetKeyDown(KeyCode.E)) {
			// show context menu
		
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			// space to pick up
		
		}

			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;		//Used to store the vertical move direction.
			
			//Check if we are running either in the Unity editor or in a standalone build.
			
			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int) (Input.GetAxisRaw ("Vertical"));		

			if(horizontal != 0)
			{
				vertical = 0;
			}

			if(horizontal != 0 || vertical != 0)
			{
				RaycastHit2D hit;
			
			//If Move returns true, meaning Player was able to move into an empty space.
				Move (horizontal, vertical, out hit);
			}


    //  var dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    //  transform.position += dir * 5.0f * Time.deltaTime;

		
		//Debug.Log ("move = " + move);
	}
                       // Speed of movement
		//Move returns true if it is able to move and false if not. 
		//Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
		protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
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
		}     

    //  void FixedUpdate () {
    //      if(Input.GetKey(KeyCode.A) && transform.position == pos) {        // Left
    //          pos += Vector3.left;
    //      }
    //      if(Input.GetKey(KeyCode.D) && transform.position == pos) {        // Right
    //          pos += Vector3.right;
    //      }
    //      if(Input.GetKey(KeyCode.W) && transform.position == pos) {        // Up
    //          pos += Vector3.up;
    //      }
    //      if(Input.GetKey(KeyCode.S) && transform.position == pos) {        // Down
    //          pos += Vector3.down;
    //      }
    //      transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);    // Move there
    //  }
    void Flip()
    {
        //Debug.Log("switching...");
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }	
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "gold" ) {
			// start black jack
			Debug.Log("touch gold");
			isTouchingMine = true;
		}		
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
