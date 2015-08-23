using UnityEngine;
using System.Collections;

public class Piston : MonoBehaviour {

	public SliderJoint2D joint;
	public float degreesPerSecond = 40f;
	public bool invert = false;
	public Control.Kind type;

	JointMotor2D motor;

	// Use this for initialization
	void Start () {
		motor = joint.motor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Control.GetState(type) == (invert? -1 : 1)) {
			motor.motorSpeed = -degreesPerSecond;
			joint.motor = motor;
		}
		else if (Control.GetState(type) == (invert? 1 : -1)) {
			motor.motorSpeed = degreesPerSecond;
			joint.motor = motor;
		}
		else {
			motor.motorSpeed = 0f;
			joint.motor = motor;
		}
	}
}
