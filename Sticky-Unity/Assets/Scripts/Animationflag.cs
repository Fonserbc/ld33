using UnityEngine;
using System.Collections;

public class Animationflag : MonoBehaviour {

	public bool flag;

	bool hasClicked = false;
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}

		if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) {
			if (!hasClicked) {
				hasClicked = true;
			}
			else {
				Application.LoadLevel(Application.loadedLevel + 1);
			}
		}
	}
}
