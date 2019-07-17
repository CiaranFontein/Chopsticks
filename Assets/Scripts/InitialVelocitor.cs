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
		
        if(GetComponent<Rigidbody2D>() != null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        else
        {
            rb = GetComponentInChildren<Rigidbody2D>();
        }
		rb.AddForce(transform.up * thrust);
        if (GetComponent<PolygonCollider2D>() != null) { 
		    GetComponent<PolygonCollider2D> ().enabled = false;
        }
        else
        {
            GetComponentInChildren<PolygonCollider2D>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (!objectSettled) {
			if (aboveYThreshold == false && transform.position.y > lowestHeight) {
				aboveYThreshold = true;
			}

			if (aboveYThreshold && rb.velocity.y < 0) {
                if (GetComponent<PolygonCollider2D>() != null)
                {
                    GetComponent<PolygonCollider2D>().enabled = true;
                }
                else
                {
                    GetComponentInChildren<PolygonCollider2D>().enabled = true;
                }
				objectSettled = true;
			}
		}
	}
}
