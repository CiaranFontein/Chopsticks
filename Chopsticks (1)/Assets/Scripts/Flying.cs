using UnityEngine;
using System.Collections;

public class Flying : MonoBehaviour {
	
	public Vector3 acceleration;
	public Vector3 speed;

	Transform tempNewPosition;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		tempNewPosition = gameObject.transform;

		if (speed.x >= 1) {
			acceleration.x = Random.Range (-1f, -0.1f);
		}
		if (speed.x <= -1) {
			acceleration.x = Random.Range (0.1f, 1f);
		}

		if (speed.y <= -1) {
			acceleration.y = Random.Range (0.1f, 1f);
		}

		if (speed.y >= 1) {
			acceleration.y = Random.Range (-1f, -0.1f);
		}
			
		speed += acceleration * Time.deltaTime;
		tempNewPosition.position += speed * Time.deltaTime;
		gameObject.transform.position = tempNewPosition.position;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "table") {
			speed.y = 1f;
			print("collider2D with table");
		}
	}
}
