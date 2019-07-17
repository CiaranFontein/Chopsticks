using UnityEngine;
using System.Collections;

public class RandomSprite : MonoBehaviour {

	public Sprite[] sprites;

	// Use this for initialization
	void Start () {
		print (sprites.Length);
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, sprites.Length - 1)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
