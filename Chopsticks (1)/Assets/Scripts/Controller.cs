using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Director;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

	private GameObject courseText;
	private GameObject timerText;
	private GameObject gameWonText;
	private GameObject helpText;
	private GameObject gameoverAnimator;
	private Animator courseTextAnimator;
	private Animator gameoverTextAnimator;
	private Animator gameWonTextAnimator;
	private Animator menuPopUpAnimator;
	private Animator helpTextAnimator;

	private AudioSource audioSource;
	public AudioClip[] sfxClips;

	public GameObject salmonMaki;
	public GameObject taco;
	public GameObject nacho;
	public GameObject chickenWing;
	public GameObject shrimp;
	public GameObject sourKey;
	public GameObject jalapeno;
	public GameObject moveableParts;
	public GameObject plate;

	public Transform foodSpawnPosition;
	public Transform plateSpawnPosition;

	static public int foodItemsLeft;
	static public float timer;

	bool controlsEnabled;
	bool helpVisible;
	public bool arcadeMode;
	public bool helpVisibleAtStart;


	int currentState;
	int currentGameMode;

	enum State {
		Stopped,
		Running,
		Paused,
		Lost,
		Won,
		NotStartedYet
	};

	enum GameMode{
		Arcade,
		FastFood,
		Practice
	};

	// Use this for initialization
	void Start() {
		//SFX
		audioSource = gameObject.GetComponent<AudioSource>();

		courseText = GameObject.FindGameObjectWithTag("courseText");
		gameoverAnimator = GameObject.FindGameObjectWithTag("gameoverText");
		timerText = GameObject.FindGameObjectWithTag("timerText");
		gameWonText = GameObject.FindGameObjectWithTag ("gameWonText");
		helpText = GameObject.FindGameObjectWithTag ("helpText");
		helpTextAnimator = helpText.GetComponent<Animator> ();
		menuPopUpAnimator = GameObject.FindGameObjectWithTag ("menu").GetComponent<Animator>();
		courseTextAnimator = courseText.GetComponent<Animator>();
		gameoverTextAnimator = gameoverAnimator.GetComponent<Animator>();

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

	// Update is called once per frame
	void Update() {

		if (currentState == (int)State.Running) {
			if (currentGameMode == (int)GameMode.Arcade) {
				CountUp ();
			} else {
				CountDown ();
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
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
		}

	}

	void helpVisibleCheck(){
		if (helpVisible) {
			helpTextAnimator.Play ("HelpPanelSlideOutLeft");
		} else {
			helpTextAnimator.Play ("HelpPanelSlideInFromLeft");
		}
	}

	void gameModeCheck(){
		switch (currentGameMode) {
		case (int)GameMode.Arcade:
			currentState = (int)State.Running;
			SpawnFood (shrimp);
			gameWonTextAnimator = gameWonText.GetComponent<Animator> ();
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

	void stateCheck(){
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

	public void goToMainMenu(){
		SceneManager.LoadScene("MainMenu");
	}

	public void restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
		
	static public void AddCountdownTime(float additionalTime) {
		timer += additionalTime;
	}

	void GameWon() {
		gameWonText.GetComponent<Text> ().text = "You won in " + timer.ToString ("F2") + " seconds!";
		gameWonTextAnimator.SetTrigger ("GameWon");
		EndGame ();
	}

	void GameOver() {
		timerText.GetComponent<Text> ().text = "Time Up!";
		gameoverTextAnimator.SetTrigger("GameOver");
		EndGame ();
	}

	void PauseGame(){
		menuPopUpAnimator.Play ("MenuPanelShow");
		currentState = (int)State.Paused;
		stateCheck ();
	}

	void UnPauseGame(){
		menuPopUpAnimator.Play ("MenuPanelHide");
	}

	public void unpauseTimer(){
		currentState = (int)State.Running;
		stateCheck ();
	}

	void EndGame() {
		menuPopUpAnimator.Play ("MenuPanelShow");
		currentState = (int)State.Stopped;
		stateCheck ();
		moveableParts.GetComponent<MovementControl> ().disableMotors ();
	}
		
	void SpawnFood(GameObject food){
		Instantiate (food, foodSpawnPosition.position, Quaternion.identity);
		foodItemsLeft++;
	}

	void CountUp() {
		timer += Time.deltaTime;
		timerText.GetComponent<Text>().text = timer.ToString ("F2");
		if (foodItemsLeft == 0) {
			GameWon ();
		}
	}

	public void foodEaten(GameObject food){
		playFoodEatenSoundEffect ();
	}

	void CountDown() {
		if (timer <= 0) {
			timer = 0.00f;
			GameOver();
		} else {
			timer -= Time.deltaTime;
			timerText.GetComponent<Text>().text = timer.ToString ("F2");
		}
	}

	private void playFoodEatenSoundEffect(){
		audioSource.PlayOneShot (sfxClips [Random.Range (0, sfxClips.Length)]);
	}
}
