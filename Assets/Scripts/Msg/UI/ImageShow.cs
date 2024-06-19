using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageShow : UIBase {

    private void Awake()
    {
        Bind(UIEvent.SHOWIMAGE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOWIMAGE:
                Debug.Log(message);
                break;
            default:
                break;
        }
    }
}
