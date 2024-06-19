using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResLoader : MonoBehaviour {
	public Camera mainCam,stateCam,editorCam;
	public GameObject[] showObjs;
	public static ResLoader resLoader;
	public AudioClip currentClip;
	public Sprite currentSprite;
	public GameObject objParent;
	public static bool isBack;
	public delegate void noparam();
	public noparam NoP;

	public static string targetPath;

	// Use this for initialization
	void Start () {
		resLoader = this;
		targetPath = Application.dataPath;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public AudioClip GetClip(string path,ref AudioClip clip){
		currentClip = clip;
		StartCoroutine (GetClipRes (path));
		return currentClip;
	}

	public Sprite GetSprite(string path,ref Sprite sprite){
		currentSprite = sprite;
		//sprite=currentSprite;
		StartCoroutine (GetImgRes (path));
		return currentSprite;
	}

	IEnumerator GetClipRes(string path)
	{
		WWW www = new WWW(@"file://"+path);
		yield return www;
		currentClip = www.GetAudioClip(false, false);
	}

	IEnumerator GetImgRes(string path){
		WWW www = new WWW(@"file://"+path);
		yield return www;
		Texture2D tex2d = www.texture;
		currentSprite = Sprite.Create (tex2d, new Rect (0, 0, tex2d.width, tex2d.height), Vector3.zero);
	}

	public IEnumerator EndFrame(noparam nop)
	{
		NoP += nop;
		yield return new WaitForSeconds (1f);
		if (NoP != null) {
			NoP ();
			print ("endFrame");
			NoP -= nop;
		}
	}

	public IEnumerator GetImgRes (string path, Image img){
		WWW www = new WWW(@"file://"+path);
		yield return www;
		Texture2D tex2d = www.texture;
		currentSprite = Sprite.Create (tex2d, new Rect (0, 0, tex2d.width, tex2d.height), Vector3.zero);
		img.sprite = currentSprite;
	}

	public void GetImage(){
		FileInfo fileInfo = new FileInfo (IOHelper.GetImageName ());
		StartCoroutine (GetImgRes (fileInfo.FullName));
	}

    public IEnumerator GetVideoRes(string path, Text pathText)
    {
        WWW www = new WWW(@"file://" + path);
        while (www.isDone==false)
        {
            yield return null;
        }
        MovieTexture movietex = www.movie;
    }
}
