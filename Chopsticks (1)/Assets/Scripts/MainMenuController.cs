using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
	
	public Transform foodSpawnPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startArcadeMode(){
		SceneManager.LoadScene ("ArcadeMode");
	}

	public void startFastFoodMode(){
		SceneManager.LoadScene ("FastFoodMode");
	}

	public void exitGame(){
		Application.Quit ();
	}

	void SpawnFood(GameObject food){
		Instantiate (food, foodSpawnPosition.position, Quaternion.identity);
	}
}
