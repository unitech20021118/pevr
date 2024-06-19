using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowButton :Action<Main> {
    public GameObject button;
    public Transform ParentTransform;
    public override void DoAction(Main m)
    {
        ParentTransform.gameObject.SetActive(true);
        if (button != null)
        {
            button.SetActive(true);
            button.GetComponent<Button>().enabled = true;
        }
        button.GetComponent<Button>().onClick.AddListener(SendEvent);
    }

    public void SendEvent()
    {
        even.DoRelateToEvents();
    }

    public void SetButton(GameObject obj)
    {
        button = obj;
    }
}
