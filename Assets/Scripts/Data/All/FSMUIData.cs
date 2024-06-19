using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUIData  {

    public List<StateUIData> allstates = new List<StateUIData>();
    public List<EventUIData> alllines = new List<EventUIData>();

}

public class StateUIData
{
    public Transform pos;
    public string stateName;
    public List<EventUIData> events;
}

public class EventUIData
{
    public Vector3 pos;
    public string eventName;
}

public class LineUIData
{
    public Vector3 OnePos;
    public Vector3 TwoPos;
}