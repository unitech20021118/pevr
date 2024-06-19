using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenemax : MonoBehaviour
{
    public GameObject cam1;
    public GameObject canvas1;
    public GameObject cam2;
    public GameObject canvas2;
    bool _maxYN;
    Image _image;
	// Use this for initialization
	void Start () {
        cam1.SetActive (true);
        cam2.SetActive (true);
        _maxYN = false;
        _image = this.GetComponentInChildren<Image>();
        _image.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void sceneMax()
    {
        if (!_maxYN)
        {
            cam1.SetActive(false);
            canvas1.SetActive(false);
            cam2.SetActive(false);
            canvas2.SetActive(false);
            _maxYN = true;
            _image.color = new Color(1f, 0.7f, 0, 1);
            return;
        }
        if (_maxYN)
        {
            cam1.SetActive(true);
            canvas1.SetActive(true);
            cam2.SetActive(true);
            canvas2.SetActive(true);
            _maxYN = false;
            _image.color = Color.white;
            return;
        }
    }
}
