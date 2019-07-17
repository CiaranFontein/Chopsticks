using UnityEngine;
using System.Collections;

public class FoodFaceCollision : MonoBehaviour {

	public Sprite fedFace;
	public Sprite hungryFace;
	public Sprite angryFace;
	public Sprite blinkFace;
	bool getAngry;

	private GameObject controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("GameController");
		InvokeRepeating("Blink", GetRandomBlinkRate(), GetRandomBlinkRate());
		getAngry = false;
	}
	
	// Update is called once per frame
	void Update() {
		if (Controller.timer <= 0) {
			getAngry = true;
		}

		if (getAngry) {
			StartCoroutine (ChangeFace (0.0f, angryFace));
			CancelInvoke();
			getAngry = false;
		}
	}

	void OnTriggerEnter2D(Collider2D food){
		if (food.tag == "food") {
			// Disable blinking while fed face is set
			CancelInvoke();
			Controller.AddCountdownTime (10f);
			Controller.foodItemsLeft--;
			controller.GetComponent<Controller> ().foodEaten (food.gameObject);
			GetComponent<SpriteRenderer> ().sprite = fedFace;
			StartCoroutine (ChangeFace (2.5f, hungryFace));
			Destroy (food.gameObject);
			InvokeRepeating("Blink", GetRandomBlinkRate(), GetRandomBlinkRate());
		}
	}

	IEnumerator ChangeFace(float time, Sprite face) {
		yield return new WaitForSeconds(time);
		GetComponent<SpriteRenderer> ().sprite = face;
	}

	void Blink(){
		StartCoroutine (ChangeFace (0.0f, blinkFace));
		StartCoroutine (ChangeFace (0.15f, hungryFace));
	}

	float GetRandomBlinkRate() {
		return Random.Range(4.0f, 10.0f);
	}

}
