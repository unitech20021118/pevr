using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

/**
 * A very simple controller for a Browser.
 * Call GoToURLInput() to go to the URL typed in urlInput
 */
[RequireComponent(typeof(Browser))]
public class LogWeb : MonoBehaviour
{

    private Browser browser;
    public GameObject target;
    public InputField urlInput;
    public string MapUrl;

    public void Start()
    {
        browser = target.transform.GetChild(0).GetComponent<Browser>();
    }

    public void GoToURLInput()
    {
        Application.OpenURL("http://192.168.1.7:8080");
        target.SetActive(true);
        //file:///E:\wangziyi/ScriptReference/Tools.html
        //http://192.168.1.4:8080
        browser.Url = MapUrl;
        //print("file:///F:/Project/OpenExeText/BrowserAssets/demo/html/index.html");
    }

    public void Close()
    {
        browser.Url = "about:blank";
        target.SetActive(false);
        //file:///E:\wangziyi/ScriptReference/Tools.html       
        //print("file:///F:/Project/OpenExeText/BrowserAssets/demo/html/index.html");
    }


}

