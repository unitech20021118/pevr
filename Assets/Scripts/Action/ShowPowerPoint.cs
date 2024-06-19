using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPowerPoint : Action<Main>
{
    /// <summary>
    /// 文件夹名（也是ppt名）
    /// </summary>
    public string FolderName;
    /// <summary>
    /// 是不是自动播放
    /// </summary>
    public bool AutoPlay;
    /// <summary>
    /// 是否要循环播放
    /// </summary>
    public bool Loop;
    /// <summary>
    /// 自动播放的间隔时间
    /// </summary>
    public float Interval;

    private PowerPointColtroller powerPointColtroller;

    public override void DoAction(Main m)
    {
        if (m.transform.Find("BG/Canvas/PowerPointImage")!=null)
        {
            powerPointColtroller = m.gameObject.GetComponent<PowerPointColtroller>();
            if (powerPointColtroller==null)
            {
                powerPointColtroller = m.gameObject.AddComponent<PowerPointColtroller>();
            }
            powerPointColtroller.InitData(FolderName,AutoPlay,Loop,Interval);
        }
        base.DoAction(m);
    }
}
