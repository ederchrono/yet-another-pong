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
			CheckPaddleCollision (oldPosition, newPosition, rightPaddle);
		}

		if (oldPosition.x - BALL_RADIUS >  leftPaddle.transform.position.x &&
			newPosition.x - BALL_RADIUS <= leftPaddle.transform.position.x) {
			// ball is going trough the left paddle
			CheckPaddleCollision (oldPosition, newPosition, leftPaddle);
		}

	}

	void CheckPaddleCollision(Vector3 oldPosition, Vector3 newPosition, GameObject paddle){

		//calculate Y position difference between ball and paddle
		float dY = newPosition.y - paddle.transform.position.y;

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
}
