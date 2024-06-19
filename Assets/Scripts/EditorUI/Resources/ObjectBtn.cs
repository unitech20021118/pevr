using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectBtn : MonoBehaviour {
    public GameObject panel;

    void Start()
    {
        panel.transform.FindChild("Close").GetComponent<Button>().onClick.AddListener(Close);
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
