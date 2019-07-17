using UnityEngine;
using System.Collections;

public class InitialVelocitor : MonoBehaviour {
	Rigidbody2D rb;
	public float thrust;
	bool aboveYThreshold = false;
	float lowestHeight = -9;
	bool objectSettled = false;
	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody2D> ();
		rb.AddForce(transform.up * thrust);
		gameObject.GetComponent<PolygonCollider2D> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!objectSettled) {
			if (aboveYThreshold == false && transform.position.y > lowestHeight) {
				aboveYThreshold = true;
			}

			if (aboveYThreshold && rb.velocity.y < 0) {
				gameObject.GetComponent<PolygonCollider2D> ().enabled = true;
				objectSettled = true;
			}
		}
	}
}
