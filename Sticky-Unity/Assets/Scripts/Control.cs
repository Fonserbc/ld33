using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	public static int stateX = 0;
	public static int stateO = 0;

	public enum Kind {
		X,
		O
	};

	public static int GetState(Kind k) {
		return k == Kind.X? stateX : stateO;
	}

	// Use this for initialization
	void Start () {
		stateX = 0;
		stateO = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.I)) {
			stateX = 1;
		}
		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K)) {
			stateX = -1;
		}
		else {
			stateX = 0;
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
			stateO = 1;
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			stateO = -1;
		}
		else {
			stateO = 0;
		}
	}
}
