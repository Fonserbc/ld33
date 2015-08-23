using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
