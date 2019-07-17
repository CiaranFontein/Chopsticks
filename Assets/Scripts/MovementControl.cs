using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour {

	public HingeJoint2D indexFinger1;
	public HingeJoint2D indexFinger2;
	public HingeJoint2D thumb1;
	public HingeJoint2D thumb2;

	public float rotation = 0.01f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		// up and down keys, range [-1, 1]
		acceleration = Input.GetAxis("Vertical");

		// key up is powah * 1, key down is powah * -1, no key is powah * 0
		hingeJoint.motor.force = powah * acceleration;
	}
}
