using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	public static int stateX = 0;
	public static int stateO = 0;

	public enum Kind {
		Red,
		Blue
	};

	public static int GetState(Kind k) {
		return k == Kind.Red? stateX : stateO;
	}

	// Use this for initialization
	void Start () {
		stateX = 0;
		stateO = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)) {
			stateX = 1;
		}
		else if (Input.GetKey(KeyCode.S)) {
			stateX = -1;
		}
		else {
			stateX = 0;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.I)) {
			stateO = 1;
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.K)) {
			stateO = -1;
		}
		else {
			stateO = 0;
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
