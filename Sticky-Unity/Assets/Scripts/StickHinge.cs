﻿using UnityEngine;
using System.Collections;

public class StickHinge : MonoBehaviour {
	
	public Control.Kind type;
	
	const float degreesPerSecond = 70f;
	
	public Rigidbody2D rb;
	
	public bool invert = false;

	public HingeJoint2D joint;

	JointMotor2D motor;

	// Use this for initialization
	void Start () {
		joint.connectedAnchor = transform.position;
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
	
	void FixRotation (ref float rot) {
		if (rot < -180f) {
			rot += 360f;
		}
		else if (rot > 180f) {
			rot -= 360f;
		}
	}

	void OnDrawGizmos() {
		float length = GetComponent<BoxCollider2D>().size.x;
		
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, length);
	}
}
