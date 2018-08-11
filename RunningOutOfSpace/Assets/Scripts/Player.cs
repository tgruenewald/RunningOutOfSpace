using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityStandardAssets._2D;
using UnityEngine.UI;

public class Player : MonoBehaviour {
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

 //.GetComponent<Button>().interactable  = false;	

	}

	
	// Update is called once per frame
	void Update () {
		// grounded = groundCheck.IsTouchingLayers (whatIsGround);

		move = Input.GetAxis ("Horizontal");
		if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
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


     var dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
     transform.position += dir * 5.0f * Time.deltaTime;

		
		//Debug.Log ("move = " + move);
	}
	void FixedUpdate () {

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
