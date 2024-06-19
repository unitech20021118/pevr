using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SourceListBtn : UIBase {

    [SerializeField]
    private Button btn;
	
	void Start () {
        btn.onClick.AddListener(Click);
	}

    void Click()
    {
        Dispatch(AreaCode.UI, UIEvent.SHOWIMAGE, "showImage");
    }
}
