using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

public class WorldVideoController : MonoBehaviour
{
    private MediaPlayer mediaPlayer;

    private DisplayUGUI displayUgui;
    /// <summary>
    /// 视频地址
    /// </summary>
    private string videoPath;
    /// <summary>
    /// 是否自动播放
    /// </summary>
    private bool autoPlay;
    /// <summary>
    /// 是否循环播放
    /// </summary>
    private bool loop;
    /// <summary>
    /// 视频播放进度条
    /// </summary>
    public Slider _videoSeekSlider;
    private float _setVideoSeekSliderValue;
    private float _setAudioVolumeSliderValue;
    private bool _wasPlayingOnScrub;
    /// <summary>
    /// 播放按钮
    /// </summary>
    private Button startButton;
    /// <summary>
    /// 暂停按钮
    /// </summary>
    private Button pauseButton;
    /// <summary>
    /// 停止按钮
    /// </summary>
    private Button stopButton;

    private Transform canvasTransform;

    private bool ready;
    void Awake()
    {
        if (transform.FindChild("BG/MediaPlayer")!=null)
        {
            mediaPlayer = transform.FindChild("BG/MediaPlayer").GetComponent<MediaPlayer>();
            displayUgui = transform.FindChild("BG/Canvas/AVPro Video").GetComponent<DisplayUGUI>();

            startButton = transform.FindChild("BG/Canvas/ButtonStart").GetComponent<Button>();
            pauseButton = transform.FindChild("BG/Canvas/ButtonPause").GetComponent<Button>();
            stopButton = transform.FindChild("BG/Canvas/ButtonStop").GetComponent<Button>();
            _videoSeekSlider = transform.FindChild("BG/Canvas/Slider").GetComponent<Slider>();
            canvasTransform = transform.FindChild("BG/Canvas");
            ready = true;
        }
        
    }
    // Use this for initialization
    void Start()
    {
        if (ready)
        {
            canvasTransform.gameObject.AddComponent<VRTK_UICanvas>();
            canvasTransform.GetComponent<GraphicRaycaster>().enabled = true;
            
            if (startButton != null)
            {
                startButton.onClick.AddListener(Play);
            }
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(Pause);
            }
            if (stopButton != null)
            {
                stopButton.onClick.AddListener(Stop);
            }
            if (_videoSeekSlider != null)
            {
                _videoSeekSlider.onValueChanged.AddListener(delegate (float a) { OnVideoSeekSlider(); });

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            if (mediaPlayer && mediaPlayer.Info != null && mediaPlayer.Info.GetDurationMs() > 0f)
            {
                float time = mediaPlayer.Control.GetCurrentTimeMs();
                float duration = mediaPlayer.Info.GetDurationMs();
                float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

                _setVideoSeekSliderValue = d;
                _videoSeekSlider.value = d;
            }
        }
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    public void SetValue(string videoPath,bool autoPlay,bool loop,int ajustMode)
    {
        if (ready)
        {
            this.videoPath = videoPath;
            this.autoPlay = autoPlay;
            this.loop = loop;
            canvasTransform.GetComponent<Canvas>().worldCamera = VRSwitch.isVR ? Manager.Instace.cameraEye.GetComponent<Camera>() : Manager.Instace.mainCamera;
            switch (ajustMode)
            {
                case 1:
                    displayUgui._scaleMode = ScaleMode.ScaleToFit;
                    break;
                case 2:
                    displayUgui._scaleMode = ScaleMode.StretchToFill;
                    break;
                case 3:
                    displayUgui._scaleMode = ScaleMode.ScaleAndCrop;
                    break;
                default:
                    break;
            }
            if (autoPlay)
            {
                pauseButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(false);
            }

            PlayManager();
        }
    }

    public void PlayManager()
    {
        if (mediaPlayer.Control.IsPlaying())
        {
            mediaPlayer.Control.Stop();
        }
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder, videoPath, autoPlay);
        mediaPlayer.Control.SetLooping(loop);
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        mediaPlayer.Play();
        pauseButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        mediaPlayer.Pause();
        pauseButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        mediaPlayer.Rewind(true);
        pauseButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
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
}
