using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InputField操作的命令
/// </summary>
public class InputFieldCommand : BaseCommand
{

    #region 命令操作、撤销操作所涉及到的属性
    /// <summary>
    /// 目标
    /// </summary>
    private InputField _commandTarget;
    /// <summary>
    /// 目标的值
    /// </summary>
    private string _commandValue;
    #endregion

    public InputFieldCommand(InputField commandTarget,string commandValue,string commandDescribe)
    {
        _commandTarget = commandTarget;
        _commandValue = commandValue;
        CommandDescribe = commandDescribe;
    }
    /// <summary>
    /// 执行命令
    /// </summary>
    public override void ExecuteCommand()
    {
        base.ExecuteCommand();
        //_commandTarget.text = _commandValue;
    }
    /// <summary>
    /// 撤销命令
    /// </summary>
    public override void RevocationCommand()
    {
        base.RevocationCommand();
        _commandTarget.text = _commandValue;
    }
}
