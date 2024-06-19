using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData {

    public UserData(string username,int totalcount,int wincount)
    {
        Username = username;
        TotalCount = totalcount;
        WinCount = wincount;
    }

    public UserData(int id, string username, int totalcount, int wincount)
    {
        Id = id;
        Username = username;
        TotalCount = totalcount;
        WinCount = wincount;
    }

    public UserData(int id, string username, int totalcount, int wincount,string task)
    {
        Id = id;
        Username = username;
        TotalCount = totalcount;
        WinCount = wincount;
        Task = task;
    }
    public int Id { get; set; }
    public string Username { get; set; }
    public int TotalCount { get; set; }
    public int WinCount { get; set; }
    public string Task { get; set; }
}
