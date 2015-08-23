using UnityEngine;
using System.Collections;

public class Animationflag : MonoBehaviour {

	public bool flag;
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
