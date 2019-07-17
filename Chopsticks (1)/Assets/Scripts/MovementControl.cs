using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour {

	public GameObject topstick;
	public GameObject wrist;
	public GameObject forearm;
	public GameObject shoulder;

	public float topstickAcceleration;
	public float topstickPower = 15f;

	public float wristAcceleration;
	public float wristPower = 15f;

	public float forearmAcceleration;
	public float forearmPower = 15f;

	public float shoulderAcceleration;
	public float shoulderPower = 15f;

	private JointMotor2D topstickMotor;
	private JointMotor2D wristMotor;
	private JointMotor2D forearmMotor;
	private JointMotor2D shoulderMotor;

	// Use this for initialization
	void Start () {
		topstick.GetComponent<HingeJoint2D>().motor = topstickMotor;
		wrist.GetComponent<HingeJoint2D>().motor = wristMotor;
		forearm.GetComponent<HingeJoint2D>().motor = forearmMotor;
		shoulder.GetComponent<HingeJoint2D> ().motor = shoulderMotor;

		topstickMotor.maxMotorTorque = 1000f;
		wristMotor.maxMotorTorque = 1000f;
		forearmMotor.maxMotorTorque = 1000f;
		shoulderMotor.maxMotorTorque = 1000f;
	}

	// Update is called once per frame
	void Update () {
		// up and down keys, range [-1, 1]
		topstickAcceleration = Input.GetAxis("Hand");

		// key up is powah * 1, key down is powah * -1, no key is powah * 0
		topstickMotor.motorSpeed = topstickPower * topstickAcceleration;
		topstick.GetComponent<HingeJoint2D> ().motor = topstickMotor;

		wristAcceleration = Input.GetAxis ("Wrist");
		wristMotor.motorSpeed = wristPower * wristAcceleration;
		wrist.GetComponent<HingeJoint2D> ().motor = wristMotor;

		forearmAcceleration = Input.GetAxis ("Elbow");
		forearmMotor.motorSpeed = forearmPower * forearmAcceleration;
		forearm.GetComponent<HingeJoint2D> ().motor = forearmMotor;

		shoulderAcceleration = Input.GetAxis ("Shoulder");
		shoulderMotor.motorSpeed = shoulderPower * shoulderAcceleration;
		shoulder.GetComponent<HingeJoint2D> ().motor = shoulderMotor;
	}

	public void disableMotors(){
		topstickMotor.maxMotorTorque = 0;
		wristMotor.maxMotorTorque = 0;
		forearmMotor.maxMotorTorque = 0;
		shoulderMotor.maxMotorTorque = 0;
	}
}
