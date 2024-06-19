using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayAudioInforma : ActionInforma {
	public string fileName, filePath;
	public bool isLoop;
	public float volume;
	public int mode;

	public PlayAudioInforma(bool isOnce){
		this.isOnce = isOnce;
	}
}
