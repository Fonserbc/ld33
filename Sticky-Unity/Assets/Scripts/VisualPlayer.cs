using UnityEngine;
using System.Collections;

public class VisualPlayer : MonoBehaviour {

	Transform parent;

	// Use this for initialization
	void Start () {
		parent = transform.parent;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parent.position;
	}
}
