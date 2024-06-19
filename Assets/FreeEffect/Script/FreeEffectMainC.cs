using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeEffectMainC : MonoBehaviour
{
    /// <summary>
    /// 选择的操作
    /// </summary>
    public int Etype;
    public float Dtime = 0;
    float _mtime;
    /// <summary>
    /// 被隐藏的粒子物体
    /// </summary>
    private List<GameObject> hideGameObjects = new List<GameObject>();

    /// <summary>
    /// 所有子物体中包含的所有粒子系统
    /// </summary>
    private ParticleSystem[] particleSystems;
    // Use this for initialization
    void Start()
    {
        Etype = 0;
        //OpenEffect();
        _mtime = 0;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Manager.Instace._Playing)
    //    {
    //        switch (Etype)
    //        {
    //            case 0:
    //                OpenEffect();//开始播放
    //                break;
    //            case 1:
    //                PauseEffect();//暂停播放
    //                break;
    //            case 2:
    //                StopEffect();//停止播放
    //                break;
    //            case 3:
    //                ContinueEffect();//继续播放
    //                break;
    //            case 4:
    //                gogoEffect();//循环播放
    //                //isOder = true;
    //                break;
    //            case 5:
    //                DeleteWffect();//删除效果
    //                break;
    //            case 6:
    //                ShowEffect();//显示效果
    //                break;
    //        }
    //        _mtime = 0;
    //    }
    //}



    public void GetParticleSystem()
    {
        particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
    }
    /// <summary>
    /// 开始播放
    /// </summary>
    public void OpenEffect()
    {
        if (particleSystems!=null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Clear();
                particleSystems[i].Play();
                var main = particleSystems[i].main;
                main.loop = true;
            }
        }
        //for (int i = 0; i < this.transform.childCount; i++)
        //{
        //    if (this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
        //    {
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
        //        //var main = this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
        //        //main.loop = true;    
        //    }
        //}
    }
    /// <summary>
    /// 循环
    /// </summary>
    public void gogoEffect()
    {
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Stop();
                particleSystems[i].Play();
                var main = particleSystems[i].main;
                main.loop = true;
            }
        }
        //for (int i = 0; i < this.transform.childCount; i++)
        //{
        //    if (this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
        //    {
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
        //        var main = this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
        //        main.loop = true;
        //    }
        //}
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void StopEffect()
    {
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Stop();
                particleSystems[i].Clear();
                var main = particleSystems[i].main;
                main.loop = false;
            }
        }
        //for (int i = 0; i < this.transform.childCount; i++)
        //{
        //    if (this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
        //    {
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
        //        //var main = this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
        //        //main.loop = false;
        //    }
        //}
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void PauseEffect()
    {
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Pause();
                var main = particleSystems[i].main;
                main.loop = false;
            }
        }
        //for (int i = 0; i < this.transform.childCount; i++)
        //{
        //    if (this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
        //    {
        //        Debug.LogError("111");
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Pause();
        //        //var main = this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
        //        //main.loop = false;
        //    }
        //}
    }
    /// <summary>
    /// 继续
    /// </summary>
    public void ContinueEffect()
    {
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Play();
                var main = particleSystems[i].main;
                main.loop = true;
            }
        }
        //for (int i = 0; i < this.transform.childCount; i++)
        //{
        //    if (this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
        //    {
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
        //        var main = this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
        //        main.loop = false;
        //    }
        //}
    }
    /// <summary>
    /// 隐藏粒子效果
    /// </summary>
    public void DeleteWffect()
    {
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Pause();
                particleSystems[i].gameObject.SetActive(false);
                hideGameObjects.Add(particleSystems[i].gameObject);
            }
        }
        //for (int i = 0; i < this.transform.childCount; i++)
        //{
        //    if (this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
        //    {
        //        this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Pause();
        //        //var main = this.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
        //        //main.loop = false;
        //        this.transform.GetChild(i).gameObject.SetActive(false);
        //        hideGameObjects.Add(transform.GetChild(i).gameObject);
        //    }
        //}
    }
    /// <summary>
    /// 显示粒子（仅用于被隐藏后的显示）
    /// </summary>
    public void ShowEffect()
    {
        if (hideGameObjects!=null&&hideGameObjects.Count>0)
        {
            for (int i = 0; i < hideGameObjects.Count; i++)
            {
                hideGameObjects[i].GetComponent<ParticleSystem>().Play();
                hideGameObjects[i].SetActive(true);
            }
        }
    }

}
