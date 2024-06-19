using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoData
{
    public static bool video_mode = false;
}

public class CameraTakeVideo : MonoBehaviour {

    
    private Camera camera;

    public AVProMovieCaptureFromCamera av;

    public GameObject moden_on;
    public GameObject moden_off;

    public InputField video_name;
    public Dropdown zhen_drop;
    public Toggle audio_toggle;

    void Start () {
        
        camera = Camera.main;
    }
	
	
	void Update () {
		
	}

    public void VideoMode()
    {

        VideoData.video_mode = !VideoData.video_mode;

        if (VideoData.video_mode)
        {
            moden_on.SetActive(false);
            moden_off.SetActive(true);
        }

        else
        {
            moden_on.SetActive(true);
            moden_off.SetActive(false);
            av.StopCapture();
        }

    }

    public void NameInit(string name)
    {
        av._autoFilenamePrefix = video_name.text;
    }

    public void ZhenInit(int num)
    {
        switch (zhen_drop.value)
        {
            case 0:
                av._frameRate = AVProMovieCaptureBase.FrameRate.Fifteen;
                break;
            case 1:
                av._frameRate = AVProMovieCaptureBase.FrameRate.TwentyFive;
                break;
            case 2:
                av._frameRate = AVProMovieCaptureBase.FrameRate.Thirty;
                break;
            case 3:
                av._frameRate = AVProMovieCaptureBase.FrameRate.Fifty;
                break;
            case 4:
                av._frameRate = AVProMovieCaptureBase.FrameRate.Sixty;
                break;
        }
    }

    public void AudioInit(bool isaudio)
    {
        av._noAudio = !audio_toggle.isOn;
    }

   

    public void StartVideo()
    {
       
        if (VideoData.video_mode)
        {
           
            av.StartCapture();

        }
       
    }

    public void PauseVideo()
    {
        if (VideoData.video_mode)
        {
            av.PauseCapture();
        }
           
    }

    public void EndVideo()
    {
        if (VideoData.video_mode)
        {
            av.StopCapture();
            
        }
       


    }
}
