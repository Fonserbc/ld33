using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	const float SPEED = 5f;

	Transform player;
	Vector3 aux;

	// Use this for initialization
	void Start () {
		player = transform.parent;

		transform.parent = null;
		aux = player.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		aux = Vector3.Lerp(transform.position, player.position, Time.fixedDeltaTime*SPEED);
		aux.z = -10f;

		transform.position = aux;
	}
}
