using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class ImageListUI : MonoBehaviour {

    public GameObject item;
    GameObject prefabs;
    ShowImageUI showImageUI;
	// Use this for initialization
	void Start () {
        string[] str= Directory.GetFiles("Assets/Resources/ink2");
        foreach (string i in str)
        {
            if (Path.GetExtension(i) == ".PNG")
            {
                string name = Path.GetFileNameWithoutExtension(i);
                Sprite sprite =(Sprite) Resources.Load("ink2/" + name, typeof(Sprite));
                if (prefabs == null)
                {
                    prefabs = (GameObject)Resources.Load("Prefabs/Image");
                }
                GameObject obj = Instantiate(prefabs);
                obj.GetComponent<Image>().sprite = sprite;
                obj.transform.SetParent(item.transform);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<Button>().onClick.AddListener(delegate() { OnClick(sprite); });
            }
        }
	}

    void OnClick(Sprite obj)
    {
        showImageUI.ImageSelected.sprite = obj;
        //showImageUI.ShowImageTarget.GetComponent<Image>().sprite = obj;
        //Sprite temp= showImageUI.GetSprite();
        //temp = obj;
        showImageUI.GetSprite(obj);
        
    }

    public void SetShowImageUI(ShowImageUI showImageui)
    {
        showImageUI = showImageui;
    }

    public void SetFalse()
    {
        gameObject.SetActive(false);
    }
}
