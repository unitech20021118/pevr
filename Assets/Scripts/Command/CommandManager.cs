using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 命令收集者
/// </summary>
public class CommandManager 
{
    /// <summary>
    /// 命令集
    /// </summary>
    //private List<BaseCommand> CommandSet;
    private Stack<BaseCommand> CommandStack;
    public CommandManager()
    {
        //CommandSet = new List<BaseCommand>();
        CommandStack = new Stack<BaseCommand>();
    }
    /// <summary>
    /// 执行新的命令
    /// </summary>
    /// <param name="command"></param>
    public void ExecutiveCommand(BaseCommand command)
    {
        //CommandSet.Add(command);
        CommandStack.Push(command);
        command.ExecuteCommand();
        Debug.Log("执行命令：" + command.CommandDescribe);
    }
    /// <summary>
    /// 撤销上一个命令
    /// </summary>
    //public void RevocationCommand()
    //{
    //    if (CommandSet.Count > 0)
    //    {
    //        for (int i = 0; i < CommandSet.Count; i++)
    //        {
    //            Debug.LogError(CommandSet[i].CommandDescribe);
    //        }
    //        Debug.LogError(CommandSet.Count);
    //        BaseCommand command = CommandSet[CommandSet.Count-1];
    //        Debug.Log(command.CommandDescribe);
    //        CommandSet.Remove(CommandSet[CommandSet.Count-1]);

    //        command.RevocationCommand();
    //        Debug.Log("撤销命令：" + command.CommandDescribe);
    //    }
    //}



    /// <summary>
    ///撤销上一个命令
    ///</summary>
    public void RevocationCommand()
    {
        if (CommandStack.Count > 1)
        {
            CommandStack.Pop();
            BaseCommand command = CommandStack.Peek();
            Debug.Log(command.CommandDescribe);
            command.RevocationCommand();
            Debug.Log("撤销命令：" + command.CommandDescribe);
        }
    }
}
