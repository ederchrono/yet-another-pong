using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// Fields to be filled from the Unity UI
	[SerializeField]
	private GameObject leftPaddle;
	[SerializeField]
	private GameObject rightPaddle;
	[SerializeField]
	private GameObject ball;
	[SerializeField]
	private Text leftScoreText;
	[SerializeField]
	private Text rightScoreText;

	private float paddleVelY=5;

	private float ballVelX=5;
	private float ballVelY=0;
	private float ballVelXMultiplier = 1;

	// Measurement vars
	private const float BALL_RADIUS  = 0.5f;
	private const float PADDLE_HALF_HEIGHT = 1;

	// Field vars
	private const float TOP_BOUND    = 4.5f;
	private const float BOTTOM_BOUND = -4.5f;
	private const float LEFT_BOUND   = -9;
	private const float RIGHT_BOUND  = 9;

	// Scores vars
	private int leftScore=0;
	private int rightScore=0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePlayerPaddle ();
		UpdateAIPaddle ();
		UpdateBall ();
	}

	void UpdatePlayerPaddle(){
		int paddleDirection = 0;
		if (Input.GetKey ("down")) {
			paddleDirection--;
		}
		if (Input.GetKey ("up")) {
			paddleDirection++;
		}
		Vector3 paddleMovement = new Vector3 (0, paddleDirection * paddleVelY * Time.deltaTime, 0);
		rightPaddle.transform.position += paddleMovement;

	}

	void UpdateAIPaddle(){
		Vector3 paddleMovement = new Vector3();
		// implement (stoopid & shaky) basic AI
		if (ball.transform.position.y > leftPaddle.transform.position.y) {
			// if ball is up, move up
			paddleMovement.y = paddleVelY * Time.deltaTime;

		} else {
			// else move down
			paddleMovement.y = -paddleVelY * Time.deltaTime;

		}

		// update AI paddle position
		leftPaddle.transform.position += paddleMovement;
	}

	void UpdateBall(){
		Vector3 oldPosition = ball.transform.position;

		float ballMovementX = ballVelX * Time.deltaTime * ballVelXMultiplier;
		float ballMovementY = ballVelY * Time.deltaTime;

		ball.transform.position += new Vector3 (ballMovementX, ballMovementY, 0);
		Vector3 newPosition = ball.transform.position;


		if (oldPosition.x + BALL_RADIUS <  rightPaddle.transform.position.x &&
			newPosition.x + BALL_RADIUS >= rightPaddle.transform.position.x) {
			// ball is going trough the right paddle
			CheckPaddleCollision (rightPaddle);
		}

		if (oldPosition.x - BALL_RADIUS >  leftPaddle.transform.position.x &&
			newPosition.x - BALL_RADIUS <= leftPaddle.transform.position.x) {
			// ball is going trough the left paddle
			CheckPaddleCollision (leftPaddle);
		}

		//check bounds collisions
		if (oldPosition.y < TOP_BOUND && 
			newPosition.y > TOP_BOUND) {
			ballVelY = -ballVelY;
		}
		if (oldPosition.y > BOTTOM_BOUND && 
			newPosition.y < BOTTOM_BOUND) {
			ballVelY = -ballVelY;
		}

		// check goals
		if (ball.transform.position.x > RIGHT_BOUND) {
			//left player goal
			leftScore++;
			UpdateScore ();
			ResetBall ();
		} else if (ball.transform.position.x < LEFT_BOUND) {
			//left player goal
			rightScore++;
			UpdateScore ();
			ResetBall ();
		}
	}

	void CheckPaddleCollision(GameObject paddle){

		//calculate Y position difference between ball and paddle
		float dY = ball.transform.position.y - paddle.transform.position.y;

		if ( Mathf.Abs(dY) < PADDLE_HALF_HEIGHT+BALL_RADIUS) {

			// paddle hit!
			ballVelXMultiplier += 0.1f; // increase ball velocity
			ballVelX = -ballVelX; // flip velocity direction

			/*
			reward risky players by adding more Y velocity
			when the ball hits the paddle edge than the center
			*/
			ballVelY = dY * 10; 

		}

	}

	void UpdateScore(){
		leftScoreText.text = "" + leftScore;
		rightScoreText.text = "" + rightScore;
	}

	private void ResetBall(){
		// center the ball & reset acummulated velocities
		ball.transform.position = new Vector3(0, 0, 0);
		ballVelY = 0;
		ballVelXMultiplier = 1;

		// set the ball direction to the player that scored
		ballVelX = -ballVelX;
	}
}
