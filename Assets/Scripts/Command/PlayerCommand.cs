using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player操作的命令
/// </summary>
public class PlayerCommand : BaseCommand
{
    #region 命令操作、撤销所涉及到的属性
    /// <summary>
    /// 目标
    /// </summary>
    private Player _commandTarget;
    /// <summary>
    /// 目标的位置
    /// </summary>
    private Vector3 _commandPosition;
    #endregion
    public PlayerCommand(Player commandTarget, Vector3 commandPosition, string commandDescribe)
    {
        
        _commandTarget = commandTarget;
        _commandPosition = commandPosition;
        CommandDescribe = commandDescribe;
    }

    public override void ExecuteCommand()
    {
        base.ExecuteCommand();
        _commandTarget.transform.position = _commandPosition;    
    }
    public override void RevocationCommand()
    {
        base.RevocationCommand();
        _commandTarget.transform.position = _commandPosition;
    }
}
