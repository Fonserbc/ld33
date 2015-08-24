using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {

	const float motorSpeed = 45f;

	public HingeJoint2D joint;

	JointMotor2D motor;

	// Use this for initialization
	void Start () {
		joint.connectedAnchor = transform.position;
		motor = joint.motor;
	}

	void Update() {
		if (Control.GetState(Control.Kind.Blue) != 0) {
			motor.motorSpeed = motorSpeed;
			joint.motor = motor;
		}
		else if (Control.GetState(Control.Kind.Red) != 0) {
			motor.motorSpeed = -motorSpeed;
			joint.motor = motor;
		}
		else {
			motor.motorSpeed = 0f;
			joint.motor = motor;
		}
	}
}
