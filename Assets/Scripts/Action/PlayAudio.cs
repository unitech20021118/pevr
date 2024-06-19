using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : Action<Main> {
	public AudioSource audioS;
	public AudioClip clip;
	public bool isLoop;
	public int mode;
	public float volume;

	public override void DoAction (Main m)
	{
		audioS = m.gameObject.GetComponent<AudioSource> ();
		if (audioS == null) {
			audioS = m.gameObject.AddComponent<AudioSource> ();
			audioS.clip = clip;
			if (!clip) {
				audioS.clip = ResLoader.resLoader.currentClip;
			}
			audioS.loop = isLoop;
			audioS.volume = volume;
			switch (mode) {
			case 0:
				if (!audioS.isPlaying) {
					audioS.Play ();
				}
				break;
			case 1:
				if (audioS.isPlaying) {
					audioS.Stop ();
				}
				break;
			}
		} else {
			audioS.clip = clip;
			audioS.loop = isLoop;
			audioS.volume = volume;
			switch (mode) {
			case 0:
				if (!audioS.isPlaying) {
					audioS.Play ();
				}
				break;
			case 1:
				if (audioS.isPlaying) {
					audioS.Stop ();
				}
				break;
			}
		}
	}
}
