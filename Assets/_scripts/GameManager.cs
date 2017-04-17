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
			//print ("down is pressed!");
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

	}
}
