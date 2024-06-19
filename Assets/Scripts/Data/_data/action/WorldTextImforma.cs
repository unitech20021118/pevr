using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldTextImforma : ActionInforma
{
    public bool close;
    /// <summary>
    /// 文本信息
    /// </summary>
    public string message;
    /// <summary>
    /// 大小
    /// </summary>
    public float size = 1f;

    public WorldTextImforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
