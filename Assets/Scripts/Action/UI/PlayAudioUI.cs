using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayAudioUI : ActionUI
{
	public GameObject target;
	public string targetName;
	public Text targetText;
    public Text audioPath;
    public AudioClip clip;
    public AudioSource audioS;
	public Toggle loopToggle;
	public Dropdown modeSwitch;
	public Slider volumeSlider;
	string path;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override Action<Main> CreateAction()
    {
		action = new PlayAudio();
		actionInforma = new PlayAudioInforma (true);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "PlayAudio";
        return base.CreateAction();
    }

	public override Action<Main> LoadAction (ActionInforma actionInforma)
	{
		PlayAudioInforma paInforma = (PlayAudioInforma)actionInforma;

		this.actionInforma = actionInforma;

		audioPath.text = paInforma.fileName;
		gameObject.SetActive (true);
		ResLoader.resLoader.GetClip (paInforma.filePath,ref clip);
		path=paInforma.filePath;
		loopToggle.isOn = paInforma.isLoop;
		volumeSlider.value = paInforma.volume;
		modeSwitch.value = paInforma.mode;
		action = new PlayAudio();
		PlayAudio playAudio = (PlayAudio)action;
		playAudio.isLoop = paInforma.isLoop;
		playAudio.volume = paInforma.volume;
		playAudio.mode = paInforma.mode;
		return action;
	}

    public void GetAudio()
    {
		try{
			FileInfo fileInfo = new FileInfo (IOHelper.GetAudioFileName());
			audioPath.text = fileInfo.Name;

			PlayAudioInforma painforma=(PlayAudioInforma)actionInforma;
			painforma.fileName=fileInfo.Name;
			StartCoroutine(GetClip(fileInfo.FullName));
		}catch{
		}
    }

    IEnumerator GetClip(string path)
    {
        WWW www = new WWW(@"file://"+path);
        yield return www;
        clip = www.GetAudioClip(false, false);
		SetAudioClip ();

		try{
			PlayAudioInforma painforma=(PlayAudioInforma)actionInforma;
			painforma.filePath=path;
		}catch{
		}
    }

	public void SetAudioClip(){
		PlayAudio playAudio = (PlayAudio)action;
		playAudio.clip = clip;
	}

	public void SetLoop(bool loop){
		PlayAudio playAudio = (PlayAudio)action;
		try{
			PlayAudioInforma painforma=(PlayAudioInforma)actionInforma;
			painforma.isLoop=loop;
			playAudio.isLoop = loop;
		}catch{
		}
	}

	public void SetVolume(float num){
		PlayAudio playAudio = (PlayAudio)action;
		try{
			PlayAudioInforma painforma=(PlayAudioInforma)actionInforma;
			painforma.volume=num;
			playAudio.volume = num;
		}catch{
		}
	}

	public void SetMode(int id){
		PlayAudio playAudio = (PlayAudio)action;
		try{
			PlayAudioInforma painforma=(PlayAudioInforma)actionInforma;
			painforma.mode=id;
			playAudio.mode = id;
		}catch{
		}
	}

	public void UpdateInput(){
		SetVisibility setVisibility = (SetVisibility)action;

		targetText.text = targetName;
		try{
			SetVisibilityInforma setVInforma = (SetVisibilityInforma)actionInforma;

		}catch{
		}
	}

	public void SetGameObject(){
		if (item.isDragging) {
			target = item.dragedItem.GetTarget ();
			targetName = target.name;
			UpdateInput ();
		}
	}

	public void ReturnGameObject(){
		if (item.isDragging) {
			target = null;
			targetName = "";
			UpdateInput ();
		}
	}
}
