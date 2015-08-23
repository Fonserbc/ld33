using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class StickHinge : MonoBehaviour {
	
	public Control.Kind type;
	
	public float degreesPerSecond = 40f;
	
	public Rigidbody2D rb;
	
	public bool invert = false;
	
	float startRotation = 0f;

	public HingeJoint2D joint;

	RigidbodyConstraints2D movingConstraints;
	RigidbodyConstraints2D fixedConstraints;

	JointMotor2D motor;

	// Use this for initialization
	void Start () {
		joint.connectedAnchor = transform.position;
		startRotation = rb.rotation;
		movingConstraints = fixedConstraints = rb.constraints;
		fixedConstraints |= RigidbodyConstraints2D.FreezeRotation;
		motor = joint.motor;
	}

	void Update() {
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
		float length = GetComponent<BoxCollider2D>().size.x - 0.5f;
		
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, length);
	}
}
