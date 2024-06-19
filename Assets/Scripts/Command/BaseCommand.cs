using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCommand : Object {

    private string _commandDesctibe;
    public string CommandDescribe
    {
        set
        {
            _commandDesctibe = value;
        }
        get
        {
            return _commandDesctibe;
        }
    }
    /// <summary>
    /// 执行命令
    /// </summary>
    public virtual void ExecuteCommand()
    {

    }
    /// <summary>
    /// 撤销命令
    /// </summary>
    public virtual void RevocationCommand()
    {

    }
}
