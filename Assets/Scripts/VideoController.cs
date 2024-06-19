using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;

public class VideoController : MonoBehaviour {
    /// <summary>
    /// 播放视频的ugui
    /// </summary>
    public DisplayUGUI displayUGUI;
    /// <summary>
    /// 控制视频播放的mediaplayer
    /// </summary>
    public MediaPlayer mediaPlayer;
    /// <summary>
    /// 视频播放进度条
    /// </summary>
    public Slider _videoSeekSlider;
    /// <summary>
    /// 调节音量的滑动条
    /// </summary>
    public Slider _audioVolumeSlider;
    private float _setVideoSeekSliderValue;
    private float _setAudioVolumeSliderValue;
    private bool _wasPlayingOnScrub;

    void Awake()
    {
        //displayUGUI = transform.GetChild(0).GetComponent<DisplayUGUI>();
        //mediaPlayer = transform.GetChild(1).GetComponent<MediaPlayer>();
    }

    void Start()
    {
        if (mediaPlayer)
        {
            if (_audioVolumeSlider)
            {
                // Volume
                if (mediaPlayer.Control != null)
                {
                    float volume = mediaPlayer.Control.GetVolume();
                    _setAudioVolumeSliderValue = volume;
                    _audioVolumeSlider.value = volume;
                }
            }
        }
        
       
    }
    void Update()
    {
        if (mediaPlayer && mediaPlayer.Info != null && mediaPlayer.Info.GetDurationMs() > 0f)
        {
            float time = mediaPlayer.Control.GetCurrentTimeMs();
            float duration = mediaPlayer.Info.GetDurationMs();
            float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

            // Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));

            _setVideoSeekSliderValue = d;
            _videoSeekSlider.value = d;
        }
    }

    //public MediaPlayer LoadingPlayer
    //{
    //    get
    //    {
    //        return _loadingPlayer;
    //    }
    //}
    /// <summary>
    /// 关闭
    /// </summary>
    public void CloseVideo()
    {
        mediaPlayer.CloseVideo();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        mediaPlayer.Play();
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        mediaPlayer.Pause();
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        mediaPlayer.Rewind(true);
    }


    public void OnVideoSeekSlider()
    {
        if (mediaPlayer && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue)
        {
            mediaPlayer.Control.Seek(_videoSeekSlider.value * mediaPlayer.Info.GetDurationMs());
        }
    }

    public void OnVideoSliderDown()
    {
        if (mediaPlayer)
        {
            _wasPlayingOnScrub = mediaPlayer.Control.IsPlaying();
            if (_wasPlayingOnScrub)
            {
                mediaPlayer.Control.Pause();
                
            }
            OnVideoSeekSlider();
        }
    }
    public void OnVideoSliderUp()
    {
        if (mediaPlayer && _wasPlayingOnScrub)
        {
            mediaPlayer.Control.Play();
            _wasPlayingOnScrub = false;

            
        }
    }

    public void OnAudioVolumeSlider()
    {
        if (mediaPlayer && _audioVolumeSlider && _audioVolumeSlider.value != _setAudioVolumeSliderValue)
        {
            mediaPlayer.Control.SetVolume(_audioVolumeSlider.value);
        }
        //if (LoadingPlayer && _audioVolumeSlider && _audioVolumeSlider.value != _setAudioVolumeSliderValue)
        //{
        //    LoadingPlayer.Control.SetVolume(_audioVolumeSlider.value);
        //}
    }
   
}
