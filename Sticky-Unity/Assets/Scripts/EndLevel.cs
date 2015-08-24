using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	void Start() {
		Music.instance.PlaySound(Music.SoundType.Pop);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			Music.instance.PlaySound(Music.SoundType.Pop);
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
