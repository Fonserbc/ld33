using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public static Music instance;

	[System.Serializable]
	public struct SoundHolder {
		public AudioClip[] clips;
		public float volume;
	}
	public SoundHolder[] sounds;

	public AudioSource source;

	public enum SoundType {
		Pop,
		Stick,
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
		source.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Length)], sound.volume > 0f? sound.volume : 1f);
	}
}
