using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase {

    public static UIManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }
}
