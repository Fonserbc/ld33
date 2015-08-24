using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public static Music instance;

	[System.Serializable]
	public struct SoundHolder {
		public AudioClip clip;
		public float volume;
	}
	public SoundHolder[] sounds;

	public AudioSource source;

	public enum SoundType {
		Length
	};

	void Start() {
		instance = this;

		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.transform.position;
	}

	public void PlaySound(SoundType which) {
		SoundHolder sound = sounds[(int)which];
		source.PlayOneShot(sound.clip, sound.volume > 0f? sound.volume : 1f);
	}
}
