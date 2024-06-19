using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Video : MonoBehaviour {
    GameObject thisobj;
    public MovieTexture movie;
    AudioSource audio;

    public CheckLicense CheckLicense;
	// Use this for initialization
	void Start () {
        thisobj = this.gameObject;
        thisobj.GetComponent<Image>().material.mainTexture = movie;
        audio = thisobj.GetComponent<AudioSource>();
        movie.Stop();
        movie.Play();
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!movie.isPlaying)
        {
            audio.Stop();
            thisobj.GetComponent<Image>().enabled = false;
            //SceneManager.LoadScene(1);
            CheckLicense.CheckActivation();
        }
	}
}

