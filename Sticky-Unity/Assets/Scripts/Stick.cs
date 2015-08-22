using UnityEngine;
using System.Collections;

public class Stick : MonoBehaviour {

	public Control.Kind type;

	[Range(-90f,90f)]
	public float minRotation = 0f;
	[Range(-90f,90f)]
	public float maxRotation = 90f;

	public float anglesPerSecond = 50f;

	public Rigidbody2D rb;

	public bool invert = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Control.GetState(type) == (invert? -1 : 1)) {
			if (rb.rotation < maxRotation) {
				float currentRotation = invert? 180 - rb.rotation : rb.rotation;
				float wantedRotation = Mathf.Min(currentRotation + anglesPerSecond*Time.deltaTime, maxRotation);
				if (invert) {
					wantedRotation = 180 - wantedRotation;
				}
				rb.MoveRotation(wantedRotation);
			}
		}
		else if (Control.GetState(type) == (invert? 1 : -1)) {
			if (rb.rotation > minRotation) {
				float currentRotation = invert? 180 - rb.rotation : rb.rotation;
				float wantedRotation = Mathf.Max(currentRotation - anglesPerSecond*Time.deltaTime, minRotation);
				if (invert) {
					wantedRotation = 180 - wantedRotation;
				}
				rb.MoveRotation(wantedRotation);
			}
		}
	}

	void OnDrawGizmos() {
		float length = GetComponent<BoxCollider2D>().size.x - 0.5f;

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, length);

		Gizmos.color = Color.red;
		Vector3 delta = Vector3.zero;

		delta.x = length * Mathf.Cos(Mathf.Deg2Rad*minRotation);
		delta.y = length * Mathf.Sin(Mathf.Deg2Rad*minRotation);
		Gizmos.DrawLine(transform.position, transform.position + delta);

		delta.x = length * Mathf.Cos(Mathf.Deg2Rad*maxRotation);
		delta.y = length * Mathf.Sin(Mathf.Deg2Rad*maxRotation);
		Gizmos.DrawLine(transform.position, transform.position + delta);

	}
}
