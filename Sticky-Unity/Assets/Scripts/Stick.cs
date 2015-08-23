using UnityEngine;
using System.Collections;

public class Stick : MonoBehaviour {

	public Control.Kind type;

	[Range(-180f,180f)]
	public float minRotation = 0f;
	[Range(-180f,180f)]
	public float maxRotation = 90f;

	public float degreesPerSecond = 40f;

	public Rigidbody2D rb;

	public bool invert = false;

	float startRotation = 0f;

	// Use this for initialization
	void Start () {
		startRotation = GetComponent<Rigidbody2D>().rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Control.GetState(type) == (invert? -1 : 1)) {
			float currentRotation = rb.rotation - startRotation;
			if (currentRotation < maxRotation) {
				FixRotation(ref currentRotation);
				float wantedRotation = Mathf.Min(currentRotation + degreesPerSecond*Time.deltaTime, maxRotation);
				rb.MoveRotation(wantedRotation + startRotation);
			}
		}
		else if (Control.GetState(type) == (invert? 1 : -1)) {
			float currentRotation = rb.rotation - startRotation;
			if (currentRotation > minRotation) {
				FixRotation(ref currentRotation);
				float wantedRotation = Mathf.Max(currentRotation - degreesPerSecond*Time.deltaTime, minRotation);
				rb.MoveRotation(wantedRotation + startRotation);
			}
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

		Gizmos.color = Color.red;
		Vector3 delta = Vector3.zero;

		float baseRot = 0f;
		if (Application.isPlaying) {
			baseRot = startRotation;
		}
		else {
			baseRot = GetComponent<Rigidbody2D>().rotation;
		}

		delta.x = length * Mathf.Cos(Mathf.Deg2Rad*(minRotation + baseRot));
		delta.y = length * Mathf.Sin(Mathf.Deg2Rad*(minRotation + baseRot));
		Gizmos.DrawLine(transform.position, transform.position + delta);

		delta.x = length * Mathf.Cos(Mathf.Deg2Rad*(maxRotation + baseRot));
		delta.y = length * Mathf.Sin(Mathf.Deg2Rad*(maxRotation + baseRot));
		Gizmos.DrawLine(transform.position, transform.position + delta);

	}
}
