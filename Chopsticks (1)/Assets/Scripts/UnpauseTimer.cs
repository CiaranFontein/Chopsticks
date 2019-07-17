using UnityEngine;
using System.Collections;

public class UnpauseTimer : MonoBehaviour {

	GameObject controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void unpauseTimer(){
		controller.GetComponent<Controller> ().unpauseTimer ();
	}
}
