using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCenter : MonoBehaviour {

    public static MsgCenter Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.AddComponent<UIManager>();
    }

    public void Dispatch(int areaCode, int eventCode, object message)
    {
        switch (areaCode)
        {
            case AreaCode.UI:
                UIManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.MODEL:
                break;
            default:
                break;
        }
    }
}
