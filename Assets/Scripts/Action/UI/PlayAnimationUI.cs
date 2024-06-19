using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 播放动画的UI
/// </summary>
public class PlayAnimationUI : ActionUI
{
    //public GameObject target;
    //public string targetName;

    public Text targetText;

    private Animator animator;
    private Animation animation;
    List<AnimationClip> clips;
    public Dropdown dropdown;
    public Toggle toggleOnce;
    public Toggle toggleLoop;
    public Toggle toggleNormal;
    public Toggle toggleInverse;
    public Slider speedSlider;
    public InputField SpeedInputField;
    /// <summary>
    /// animation动画物体
    /// </summary>
    public GameObject AnimationGameObject;
    /// <summary>
    /// animator动画物体
    /// </summary>
    public GameObject AnimatorGameObject;
    /// <summary>
    /// animation动画中的是否循环播放选项
    /// </summary>
    public Toggle IsLoop_Animation;


    List<string> stringList = new List<string>();
    private PlayAnimation _playAnimation;
    private PlayAnimationInforma _playAnimationInforma;



    void Start()
    {
        Init();
    }

    void Init()
    {
        if (clips == null)
        {
            _playAnimation = (PlayAnimation)action;

            animator = Manager.Instace.gonggong.GetComponent<Animator>();
            if (animator != null)
            {
                clips = new List<AnimationClip>(animator.runtimeAnimatorController.animationClips);

                foreach (AnimationClip a in clips)
                {
                    if (!a.name[0].Equals('0') && !a.name.EndsWith("loop"))
                    {
                        if (stringList.Contains(a.name))
                        {
                            continue;
                        }
                        stringList.Add(a.name);
                    }
                }

                UpdateDropdownView(stringList);
            }
            else
            {
                animation = Manager.Instace.gonggong.GetComponent<Animation>();
                //如果检测到animation组件则应用animation方式播放动画
                if (animation != null)
                {
                    AnimatorGameObject.SetActive(false);
                    AnimationGameObject.SetActive(true);
                    _playAnimationInforma.isAnimation = true;
                    dropdown.gameObject.SetActive(false);
                    speedSlider.gameObject.SetActive(false);
                }//两者都不是则是目标物体上并没有动画
                else
                {
                    dropdown.captionText.text = null;
                    dropdown.options.Clear();
                    dropdown.enabled = false;
                }
            }
        }
        toggleOnce.onValueChanged.AddListener(delegate (bool isL) { Change(); });
        toggleLoop.onValueChanged.AddListener(delegate (bool isL) { Change(); });
        toggleNormal.onValueChanged.AddListener(delegate (bool isL) { Change(); });
        toggleInverse.onValueChanged.AddListener(delegate (bool isL) { Change(); });
        dropdown.onValueChanged.AddListener(delegate (int a) { Change(); });
        speedSlider.onValueChanged.AddListener(delegate (float a) { GetSpeed(); });
        SpeedInputField.onEndEdit.AddListener(delegate { OnEndEditSpeedInputField(); });
        //timeInputField.onValueChanged.AddListener(delegate(string a) { ActionTimeChanged(); });
        Change();
    }
    //void ActionTimeChanged()
    //{
    //    if (playAnimation != null)
    //    {
    //        playAnimation.duringTime = float.Parse( timeInputField.text);
    //        playAnimationInforma.durtime = playAnimation.duringTime;
    //    }
    //}

    void GetSpeed()
    {
        _playAnimation.speed = speedSlider.value;
        _playAnimationInforma.speed = _playAnimation.speed;
        SpeedInputField.text = speedSlider.value.ToString();
    }

    void OnEndEditSpeedInputField()
    {
        SpeedInputField.text = Mathf.Clamp(float.Parse(SpeedInputField.text), 0f, 5f).ToString();
        speedSlider.value = float.Parse(SpeedInputField.text);
    }
    //void Update()
    //{
    //    if (dropdown.captionText.text != "")
    //    {
    //        playAnimation.animationName = dropdown.captionText.text;

    //        playAnimationInforma.animationName = playAnimation.animationName;
    //    }

    //}

    void Change()
    {

        if (toggleOnce.isOn && toggleNormal.isOn)//单次播放
        {
            _playAnimation.animationName = dropdown.captionText.text;
        }
        else if (toggleLoop.isOn && toggleNormal.isOn)//循环播放
        {
            _playAnimation.animationName = dropdown.captionText.text + "loop";
        }
        else if (toggleOnce.isOn && toggleInverse.isOn)//倒播单次
        {
            _playAnimation.animationName = "0" + dropdown.captionText.text;
        }
        else//倒播循环
        {
            _playAnimation.animationName = "0" + dropdown.captionText.text + "loop";
        }
        _playAnimationInforma.animationName = _playAnimation.animationName;
    }

    void UpdateDropdownView(List<string> clips)
    {
        dropdown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < clips.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = clips[i];
            dropdown.options.Add(tempData);

        }

        if (_playAnimationInforma.animationName == null)
        {
            dropdown.captionText.text = stringList[0];
        }
        else
        {
            _playAnimation.animationName = _playAnimationInforma.animationName;

            if (_playAnimationInforma.animationName.StartsWith("0") && _playAnimationInforma.animationName.EndsWith("loop"))
            {
                toggleInverse.isOn = true;
                toggleLoop.isOn = true;
                toggleNormal.isOn = false;
                toggleOnce.isOn = false;

            }
            else if (_playAnimationInforma.animationName.StartsWith("0") && !_playAnimationInforma.animationName.EndsWith("loop"))
            {
                toggleInverse.isOn = true;
                toggleLoop.isOn = false;
                toggleNormal.isOn = false;
                toggleOnce.isOn = true;
            }
            else if (!_playAnimationInforma.animationName.StartsWith("0") && !_playAnimationInforma.animationName.EndsWith("loop"))
            {
                toggleNormal.isOn = true;
                toggleLoop.isOn = false;
                toggleInverse.isOn = false;
                toggleOnce.isOn = true;
            }
            else
            {
                toggleNormal.isOn = true;
                toggleLoop.isOn = true;
                toggleInverse.isOn = false;
                toggleOnce.isOn = false;
            }
            string s = null;
            if (_playAnimationInforma.animationName.StartsWith("0"))
            {
                s = _playAnimationInforma.animationName.Replace("0", "");
                if (s.EndsWith("loop"))
                {
                    s = s.Replace("loop", "");
                }
                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    if (dropdown.options[i].text == s)
                    {
                        dropdown.value = i;
                    }
                }
                if (dropdown.captionText.text != s)
                {
                    dropdown.captionText.text = s;
                }

            }
            else
            {
                if (_playAnimationInforma.animationName.EndsWith("loop"))
                {
                    s = _playAnimationInforma.animationName.Replace("loop", "");
                    for (int i = 0; i < dropdown.options.Count; i++)
                    {
                        if (dropdown.options[i].text == s)
                        {
                            dropdown.value = i;
                        }
                    }
                    if (dropdown.captionText.text != s)
                    {
                        dropdown.captionText.text = s;
                    }
                }
                else
                {
                    for (int i = 0; i < dropdown.options.Count; i++)
                    {
                        if (dropdown.options[i].text == _playAnimationInforma.animationName)
                        {
                            dropdown.value = i;
                        }
                    }
                    if (dropdown.captionText.text != _playAnimationInforma.animationName)
                    {
                        dropdown.captionText.text = _playAnimationInforma.animationName;
                    }
                    //dropdown.captionText.text = playAnimationInforma.animationName;
                }
            }
        }
    }

    public override Action<Main> CreateAction()
    {
        //_playAnimation = new PlayAnimation();

        //actionInforma = new PlayAnimationInforma(true);
        //_playAnimationInforma = (PlayAnimationInforma)actionInforma;

        //GetStateInfo().actionList.Add(actionInforma);
        //actionInforma.name = "PlayAnimation";
        //action = _playAnimation;
        //GetSpeed();
        //Init();

        action = new PlayAnimation();
        actionInforma = new PlayAnimationInforma(true);
        _playAnimation = (PlayAnimation)action;
        _playAnimationInforma = (PlayAnimationInforma)actionInforma;


        GetStateInfo().actionList.Add(actionInforma);
        
        actionInforma.name = "PlayAnimation";
        GetSpeed();
        Init();
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma a)
    {
        _playAnimationInforma = (PlayAnimationInforma)a;
        actionInforma = a;
        //action = new PlayAnimation(_playAnimationInforma.animationName);
        action = new PlayAnimation();
        _playAnimation = (PlayAnimation)action;

        if (_playAnimationInforma.isAnimation == false)
        {
            _playAnimation.speed = _playAnimationInforma.speed;
            speedSlider.value = _playAnimation.speed;
            SpeedInputField.text = _playAnimationInforma.speed.ToString();

            animator = Manager.Instace.gonggong.GetComponent<Animator>();

            clips = new List<AnimationClip>(animator.runtimeAnimatorController.animationClips);
            foreach (AnimationClip c in clips)
            {
                if (c.name == _playAnimationInforma.animationName)
                {
                    _playAnimation.clip = c;
                }
                if (!c.name[0].Equals('0') && !c.name.EndsWith("loop"))
                {
                    if (stringList.Contains(c.name))
                    {
                        continue;
                    }
                    stringList.Add(c.name);
                }
            }
            UpdateDropdownView(stringList);
        }
        else
        {
            _playAnimation.isLoop = _playAnimationInforma.isloop;
            IsLoop_Animation.isOn = _playAnimationInforma.isloop;
        }


        //targetText.text = playAnimationInforma.targetName;
        //targetName = playAnimationInforma.targetName;

        //playAnimation.duringTime = playAnimationInforma.durtime;
        //timeInputField.text = playAnimation.duringTime.ToString();
        //ResLoader.resLoader.StartCoroutine (ResLoader.resLoader.EndFrame (() => {
        //    target = GameObject.Find ("Parent/" + targetName);
        //    UpdateTarget ();
        //}));
        return action;
    }

    //public void SetGameObject()
    //{
    //    if (item.isDragging)
    //    {
    //        target = item.dragedItem.GetTarget();
    //        targetName = target.name;
    //        UpdateTarget();
    //    }
    //}

    //public void ReturnGameObject()
    //{
    //    if (item.isDragging)
    //    {
    //        target = null;
    //        targetName = "";
    //        UpdateTarget();
    //    }
    //}

    public void UpdateTarget()
    {
        try
        {
            clips.Clear();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        _playAnimation = (PlayAnimation)action;
        //playAnimation.target = target;
        //playAnimation.targetName = targetName;
        //targetText.text = targetName;
        PlayAnimationInforma changeColorInforma = (PlayAnimationInforma)actionInforma;
        //changeColorInforma.targetName = targetName;
        //try{
        //    //animator=target.GetComponent<Animator>();
        //}catch{
        //}
        if (animator != null)
        {
            dropdown.enabled = true;
            clips = new List<AnimationClip>(animator.runtimeAnimatorController.animationClips);

            //print (clips.Count);
            UpdateDropdownView(stringList);
        }
        else
        {
            //print (clips.Count);
            dropdown.captionText.text = null;
            dropdown.options.Clear();
            dropdown.enabled = false;
        }
    }
    /// <summary>
    /// animation动画模式下更换单次循环播放动画
    /// </summary>
    public void AniLoopChanged()
    {
        if (IsLoop_Animation.isOn)
        {
            _playAnimationInforma.isloop = true;
            _playAnimation.isLoop = true;
        }
        else
        {
            _playAnimationInforma.isloop = false;
            _playAnimation.isLoop = false;
        }
    }
}
