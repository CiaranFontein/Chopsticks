using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;
using UnityEditor;

public class Controller : MonoBehaviour
{

	private GameObject courseText;
	private GameObject timerText;
	private GameObject helpText;
	private GameObject endLevelMessage;
	private Animator courseTextAnimator;
	private Animator menuPopUpAnimator;
	private Animator helpTextAnimator;

	private AudioSource audioSource;
	public AudioClip[] sfxClips;
	public Sprite xMark;

	public GameObject salmonMaki;
	public GameObject taco;
	public GameObject nacho;
	public GameObject chickenWing;
	public GameObject shrimp;
	public GameObject sourKey;
	public GameObject jalapeno;
	public GameObject fortuneCookie;
    public GameObject chickenFeet;
	public GameObject moveableParts;
	public GameObject plate;
   

	private GameObject foodFellBounds;

	private FoodFaceCollision foodFaceCollision;

	private GameObject[] foodOnTable;
	private GameObject[] foodList;
	public GameObject foodListItem;

	public Transform foodSpawnPosition;
	public Transform plateSpawnPosition;

	static public int foodItemsLeft;
	static public float timer;

	private float currentFoodItemTimer = 0f;

	bool controlsEnabled;
	bool helpVisible;
	public bool arcadeMode;
	public bool helpVisibleAtStart;



	int currentState;
	int currentGameMode;

	int currentLevel;
	String levelName;

	enum State
	{
		Stopped,
		Running,
		Paused,
		Lost,
		Won,
		NotStartedYet}

	;

	enum GameMode
	{
		Arcade,
		FastFood,
		Practice}

	;

	// Use this for initialization
	void Start () {
		//SFX
		audioSource = gameObject.GetComponent<AudioSource> ();
		foodFaceCollision = GameObject.FindGameObjectWithTag ("FoodFaceCollision").GetComponent<FoodFaceCollision> ();
		courseText = GameObject.FindGameObjectWithTag ("courseText");
		timerText = GameObject.FindGameObjectWithTag ("timerText");
		helpText = GameObject.FindGameObjectWithTag ("helpText");
		endLevelMessage = GameObject.FindGameObjectWithTag ("endLevelMessage");
		helpTextAnimator = helpText.GetComponent<Animator> ();
		menuPopUpAnimator = GameObject.FindGameObjectWithTag ("menu").GetComponent<Animator> ();
		courseTextAnimator = courseText.GetComponent<Animator> ();
		foodFellBounds = GameObject.FindGameObjectWithTag ("FoodFellBounds");
		if (SceneManager.GetActiveScene ().name == "ArcadeMode") {
			currentGameMode = (int)GameMode.Arcade;
		}
		courseTextAnimator.SetTrigger ("CourseStart");
		if (helpVisibleAtStart) {
			helpTextAnimator.Play ("HelpPanelSlideInFromLeft");
		}
		helpVisible = !helpVisibleAtStart;//this is dumb but it works
		gameModeCheck ();//ALWAYS GAME MODE CHECK FIRST
		stateCheck ();//THEN STATE CHECK
	}
    
	void Update () {
        if (currentState == (int)State.Running)
        {
            if (currentGameMode == (int)GameMode.Arcade)
            {
                CountUp();
            }
            else
            {
                CountDown();
            }
        }
        if (Input.GetKeyDown (KeyCode.Escape)) {
			if (currentState == (int)State.Running) {
				PauseGame ();
			} else if (currentState == (int)State.Paused) {
				UnPauseGame ();
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			if (currentState == (int)State.Paused) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (currentState == (int)State.Running) {
				helpVisible = !helpVisible;
				helpVisibleCheck ();
			}
		}

		if (controlsEnabled) {
			if (Input.GetKeyDown (KeyCode.Z)) {
				SpawnFood (nacho);
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				SpawnFood (taco);
			}
			if (Input.GetKeyDown (KeyCode.C)) {
				SpawnFood (chickenWing);
			}
			if (Input.GetKeyDown (KeyCode.V)) {
				SpawnFood (salmonMaki);
			}
			if (Input.GetKeyDown (KeyCode.B)) {
				SpawnFood (shrimp);
			}
			if (Input.GetKeyDown (KeyCode.N)) {
				SpawnFood (sourKey);
			}
			if (Input.GetKeyDown (KeyCode.M)) {
				SpawnFood (jalapeno);
			}
			if (Input.GetKeyDown (KeyCode.Comma)) {
				SpawnFood (fortuneCookie);
			}
            if (Input.GetKeyDown(KeyCode.Period))
            {
                SpawnFood(chickenFeet);
            }
        }

	}

	void helpVisibleCheck () {
		if (helpVisible) {
			helpTextAnimator.Play ("HelpPanelSlideOutLeft");
		} else {
			helpTextAnimator.Play ("HelpPanelSlideInFromLeft");
		}
	}

	void gameModeCheck () {
		switch (currentGameMode) {
		case (int)GameMode.Arcade:
			currentState = (int)State.Running;
			startLevel1 ();
			timer = 0.00f;
			CountUp ();
			break;
		case (int)GameMode.FastFood:
			currentState = (int)State.Running;
			timer = 60.00f;
			CountDown ();
			break;
		case (int)GameMode.Practice:
			currentState = (int)State.Running;
			break;
		}
	}

	void stateCheck () {
		switch (currentState) {
		case (int)State.Stopped:
			controlsEnabled = false;
			break;
		case (int)State.Running:
			controlsEnabled = true;
			break;
		case (int)State.Paused:
			controlsEnabled = false;
			break;
		}
	}

	public void goToMainMenu () {
		SceneManager.LoadScene ("MainMenu");
	}

	public void restart () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	static public void AddCountdownTime (float additionalTime) {
		timer += additionalTime;
	}

	void LevelComplete () {
		EndGame ();
	}

	void GameOver () {
		timerText.GetComponent<Text> ().text = "Time Up!";
		EndGame ();
	}

	void PauseGame () {
		menuPopUpAnimator.Play ("MenuPanelShow");
		currentState = (int)State.Paused;
		stateCheck ();
	}

	void UnPauseGame () {
		menuPopUpAnimator.Play ("MenuPanelHide");
	}

	public void unpauseTimer () {
		currentState = (int)State.Running;
		stateCheck ();
	}

	void EndGame () {
		menuPopUpAnimator.Play ("MenuPanelShow");
		currentState = (int)State.Stopped;
		stateCheck ();
		moveableParts.GetComponent<MovementControl> ().disableMotors ();
	}

	void SpawnFood (GameObject food) {
		GameObject tempFood = Instantiate (food, foodSpawnPosition.position, Quaternion.identity) as GameObject;
		tempFood.name = food.name;
		addToFoodList (food);//Checks every food tag and makes new list every time food is added, could be optimized later//updates the list of foods
	}

	void CountUp () {
		timer += Time.deltaTime;
		timerText.GetComponent<Text> ().text = timer.ToString ("F2");
	}

	public void foodEaten (GameObject food) {
		playFoodEatenSoundEffect ();
		greyFromFoodList (food);
		Destroy (food.gameObject);
	}

	public void foodFell (GameObject food) {
		//play smush sound effect

		xFoodThatFell (food);
		Destroy (food.gameObject);
	}

	void CountDown () {
		if (timer <= 0) {
			timer = 0.00f;
			GameOver ();
		} else {
			timer -= Time.deltaTime;
			timerText.GetComponent<Text> ().text = timer.ToString ("F2");
		}
	}

	private void playFoodEatenSoundEffect () {
		audioSource.PlayOneShot (sfxClips [UnityEngine.Random.Range (0, sfxClips.Length)]);
	}

	void addToFoodList (GameObject food) {
		foodOnTable = GameObject.FindGameObjectsWithTag ("food");

		float yPos = foodOnTable.Length * -150.0f;//set distance from "Current Plate" title to make list

		Vector3 tempPos = new Vector3 (755, yPos + 400, 0);//set the difference as a vector 3

		GameObject tempFoodListItem = Instantiate (foodListItem, tempPos, Quaternion.identity) as GameObject;
		tempFoodListItem.GetComponent<Transform> ().SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);
		tempFoodListItem.GetComponentInChildren<Text> ().text = food.name;
		foodList = GameObject.FindGameObjectsWithTag ("FoodListItem");
	}

	/**
	 * TO BE CALLED WHEN A FOOD IS EATEN
	 */
	void greyFromFoodList (GameObject food) {
		bool duplicate = false;
		String currentFoodName = food.name;
		foreach (GameObject foodName in foodList) {
			if (!duplicate && foodName.GetComponentInChildren<Text> ().text.Equals (currentFoodName) && foodName.GetComponent<JustABoolean> ().done == false) {
				foodName.transform.Find ("BackgroundPanel").GetComponent<Image> ().color = Color.grey;
				foodName.transform.Find ("Checkmark").gameObject.SetActive (true);
				foodName.transform.Find ("FoodTime").gameObject.SetActive (true);
				foodName.transform.Find ("TimePanel").gameObject.SetActive (true);
				foodName.GetComponent<JustABoolean> ().setDone (true);
				duplicate = true;
				currentFoodItemTimer = timer - currentFoodItemTimer;
				foodName.transform.Find ("FoodTime").gameObject.GetComponent<Text> ().text = currentFoodItemTimer.ToString ("F2");
				currentFoodItemTimer = timer;
			}
		}
		checkIfAllFoodsInLevelEaten ();
	}

	public void xFoodThatFell (GameObject food) {
		//get angry
		foodFaceCollision.temporaryAnger ();
		String currentFoodName = food.name;
		bool duplicate = false;
		foreach (GameObject foodName in foodList) {
			if (!duplicate && foodName.GetComponentInChildren<Text> ().text.Equals (currentFoodName) && foodName.GetComponent<JustABoolean> ().done == false) {
				foodName.transform.Find ("BackgroundPanel").GetComponent<Image> ().color = Color.grey;
				foodName.transform.Find ("Checkmark").gameObject.SetActive (true);
				foodName.transform.Find ("Checkmark").GetComponent<Image> ().sprite = xMark;
				foodName.GetComponent<JustABoolean> ().setDone (true);
				duplicate = true;
			}
		}
		checkIfAllFoodsInLevelEaten ();
	}

	void checkIfAllFoodsInLevelEaten () {
		bool isLevelComplete = true;
		foreach (GameObject foodName in foodList) {
			if (!foodName.GetComponent<JustABoolean> ().done) {
				isLevelComplete = false;
			}
		}
		if (isLevelComplete) {
			LevelComplete ();
		}
	}

	public void startLevel1 () {
		levelName = "Level 1";
		StartCoroutine (spawnLevel1Foods ());
	}

	IEnumerator spawnLevel1Foods () {
		SpawnFood (shrimp);
        yield return true;
		//yield return new WaitForSeconds (2);
		//SpawnFood (salmonMaki);
	}
}

