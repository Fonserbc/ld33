using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

	Vector2 vel = Vector2.zero;

	// Use this for initialization
	void Start () {
		vel = new Vector2(Mathf.Sin(Time.time*3f)*2f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		vel.x = Mathf.Sin(Time.time*3f)*2f;
		vel.y += -Physics2D.gravity.y*Time.deltaTime*0.5f;
		GetComponent<Rigidbody2D>().velocity = vel;
	}
}
