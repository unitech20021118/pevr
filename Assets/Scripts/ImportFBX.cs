//*************************王梓亦-2018-11-26*************************
//*******************************************************************
//*******************************************************************
//*******************************************************************
//*******************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using UnityEditor;
using LitJson;
//using UnityEditor;

public class ImportFBX : MonoBehaviour {

    public Button MoXing;
    public Text fbxpath;
    public InputField fbxname;
    public InputField fbxname2;
    public InputField tubiaoname;
    public InputField tubiaoname2;
    public Button tubiaobtn;
    public Text tubiaopath;
    public Text seccess;

    public Text pngpath;
    public Dropdown dropdown;
    private string oldfbxpath;
    private string newfbxpath;
    private string new3dpropath;
    private string oldpngpath;
    private string newpngpath;

    private string txtpath;
    private bool ziding;

	// Use this for initialization
	void Start () {
        string sspng = Application.streamingAssetsPath + "/ink2";
        string ssmodel = Application.streamingAssetsPath + "/NewUIPic/tubiao";
        if(!Directory.Exists(sspng))
        {
            Directory.CreateDirectory(sspng);
            seccess.text = "创建文件夹sspng";
        }
        if (!Directory.Exists(ssmodel))
        {
            Directory.CreateDirectory(ssmodel);
            seccess.text = "创建文件夹ssmodel";
        }
        //AssetDatabase.Refresh();
        //string path = Application.dataPath + "/Resources/json.txt";
        string path = Application.streamingAssetsPath + "/json.txt";
        readjson(path);       
        dropdown.captionText.text = "animal";
        MoXing.onClick.AddListener(threeDpro);
	}
	
	// Update is called once per frame
	void Update () {
        if(dropdown.captionText.text == "自定义")
        {
            ziding = true;
            tubiaoname.gameObject.SetActive(true);
            tubiaoname2.gameObject.SetActive(true);
            tubiaobtn.gameObject.SetActive(true);
            tubiaopath.gameObject.SetActive(true);
        }
        else
        {
            ziding = false;
            tubiaoname.gameObject.SetActive(false);
            tubiaoname2.gameObject.SetActive(false);
            tubiaobtn.gameObject.SetActive(false);
            tubiaopath.gameObject.SetActive(false);
        }
		
	}

    public void jj()
    {
        print(dropdown.captionText.text);
    }
    /// <summary>
    /// 寻找fbx路径
    /// </summary>
    public void FBX()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = IOHelper.OpenfbxToLoad();     
        if (!string.IsNullOrEmpty(path))
        {
            ResLoader.targetPath = path.Substring(0, path.LastIndexOf("\\"));
            
            string s = string.Empty;
            string[] sarr = path.Split('\\');
            for (int i = 0; i < sarr.Length - 1; i++)
            {
                s += sarr[i] + "/";
            }
            string name = sarr[sarr.Length - 1].Split('.')[0];

            oldfbxpath = s + sarr[sarr.Length - 1];
            fbxpath.text = s + sarr[sarr.Length - 1];
            
        }
    }
    public void threeDpro()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = IOHelper.Open3dproToLoad();
        if (!string.IsNullOrEmpty(path))
        {
            ResLoader.targetPath = path.Substring(0, path.LastIndexOf("\\"));
            string s = string.Empty;
            string[] sarr = path.Split('\\');
            for (int i = 0; i < sarr.Length - 1; i++)
            {
                s += sarr[i] + "/";
            }
            string name = sarr[sarr.Length - 1].Split('.')[0];

            oldfbxpath = s + sarr[sarr.Length - 1];
            fbxpath.text = s + sarr[sarr.Length - 1];

        }
    }
    /// <summary>
    /// 寻找预览图路径
    /// </summary>
    public void PNG()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = IOHelper.GetPNG();       
        if (!string.IsNullOrEmpty(path))
        {
            ResLoader.targetPath = path.Substring(0, path.LastIndexOf("\\"));
            string s = string.Empty;
            string[] sarr = path.Split('\\');
            for (int i = 0; i < sarr.Length - 1; i++)
            {
                s += sarr[i] + "/";
            }
            string name = sarr[sarr.Length - 1].Split('.')[0];

            oldpngpath = s + sarr[sarr.Length - 1];
            pngpath.text = oldpngpath;
            print(oldpngpath);
            
            //File.Move(oldpngpath, newpngpath);
            //txtpath = Application.dataPath + "/Resources/" + dropdown.captionText.text + ".txt";
            //JsonReadorWrite(dropdown.captionText.text, txtpath, fbxname.text, fbxname2.text);
        }
    }
    /// <summary>
    /// 获取图标路径
    /// </summary>
    public void Tubiao()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = IOHelper.GetPNG();      
        if (!string.IsNullOrEmpty(path))
        {
            ResLoader.targetPath = path.Substring(0, path.LastIndexOf("\\"));
            Debug.LogError(path.Substring(0, path.LastIndexOf("\\")));
            string s = string.Empty;
            string[] sarr = path.Split('\\');
            for (int i = 0; i < sarr.Length - 1; i++)
            {
                s += sarr[i] + "/";
            }
            string name = sarr[sarr.Length - 1].Split('.')[0];

            tubiaopath.text = s + sarr[sarr.Length - 1];
        }
    }
    /// <summary>
    /// 执行导入
    /// </summary>
    public void Move()
    {
        
        if (oldpngpath != string.Empty && newpngpath != string.Empty && fbxname.text != string.Empty && fbxname2.text != string.Empty)
        {
            Manager.Instace.DES();
            string sspng = Application.streamingAssetsPath + "/ink2/" + fbxname.text + ".png";
            string s = string.Empty;
            string[] sarr = oldfbxpath.Split('/');
            string newnamepath = sarr[sarr.Length - 1].Split('.')[1];
            string ssfbx = Application.dataPath;
            print(newnamepath);
            if (!ziding)
            {
                //txtpath = Application.streamingAssetsPath + "/" + dropdown.captionText.text + ".txt";

                if (!Directory.Exists(Application.streamingAssetsPath + "/ink2/" + dropdown.captionText.text))
                    Directory.CreateDirectory(Application.streamingAssetsPath + "/ink2/" + dropdown.captionText.text);

                sspng = Application.streamingAssetsPath + "/ink2/" + dropdown.captionText.text + "/" + fbxname.text + ".png";

                if (!Directory.Exists(ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + dropdown.captionText.text))
                    Directory.CreateDirectory(ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + dropdown.captionText.text);

                if (newnamepath == "FBX" || newnamepath == "fbx")
                    newfbxpath = ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + dropdown.captionText.text + "/" + fbxname.text + ".FBX";
                else if (newnamepath == "3dpro")
                {
                    newfbxpath = ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + dropdown.captionText.text + "/" + fbxname.text + ".3dpro";
                    new3dpropath = "AssetBundles/" + dropdown.captionText.text + "/" + fbxname.text + ".3dpro";
                    //File.Copy(oldfbxpath + ".manifest", newfbxpath + ".manifest");
                }
            }
            else
            {
                if (tubiaoname.text != string.Empty && tubiaoname2.text != string.Empty && tubiaopath.text != string.Empty)
                {

                    if (!Directory.Exists(Application.streamingAssetsPath + "/ink2/" + dropdown.captionText.text))
                        Directory.CreateDirectory(Application.streamingAssetsPath + "/ink2/" + dropdown.captionText.text);

                    if (!Directory.Exists(ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + tubiaoname.text))
                        Directory.CreateDirectory(ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + tubiaoname.text);
                    sspng = Application.streamingAssetsPath + "/ink2/" + tubiaoname.text + "/" + fbxname.text + ".png";
                    if (newnamepath == "FBX")
                        newfbxpath = ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + tubiaoname.text + "/" + fbxname.text + ".FBX";
                    else if (newnamepath == "3dpro")
                    {
                        newfbxpath = ssfbx.Substring(0, ssfbx.LastIndexOf("/")) + "/AssetBundles/" + tubiaoname.text + "/" + fbxname.text + ".3dpro";
                        new3dpropath = "AssetBundles/" + tubiaoname.text + "/" + fbxname.text + ".3dpro";
                        //File.Copy(oldfbxpath + ".manifest", newfbxpath + ".manifest");
                    }
                }
                else
                {
                    seccess.text = "导入失败，信息填写不全！";
                    return;
                }
            }
            //string sspng = Application.streamingAssetsPath + "/ink2/" + fbxname.text + ".png";
            newpngpath = sspng;
            
            
            
            try
            {
                File.Copy(oldfbxpath, newfbxpath);
                File.Copy(oldpngpath, newpngpath);
                //File.Copy(oldfbxpath, newfbxpath, true);
                //File.Copy(oldpngpath, newpngpath, true);

                    
            }
            catch(Exception e)
            {
                print(e);
                seccess.text = "文件名重复";
                return;
            }
            //AssetDatabase.Refresh();

            if (!ziding)
            {
                txtpath = Application.streamingAssetsPath + "/" + dropdown.captionText.text + ".txt";
                JsonReadorWrite(dropdown.captionText.text, txtpath, fbxname.text, fbxname2.text, newnamepath, new3dpropath);
            }
            else
            {
                if (tubiaoname.text != string.Empty && tubiaoname2.text != string.Empty && tubiaopath.text != string.Empty)
                {
                    txtpath = Application.streamingAssetsPath + "/" + tubiaoname.text + ".txt";
                    JsonReadorWriteNew(tubiaoname.text, txtpath, fbxname.text, fbxname2.text, newnamepath, new3dpropath);
                }
                else
                {
                    seccess.text = "导入失败，信息填写不全！";
                    return;
                }
            }
            // Manager.Instace.DES();
            Manager.Instace.Init();
        }
        else
        {
            seccess.text = "导入失败，信息填写不全！";
        }
            
    }
    /// <summary>
    /// 初始化界面并关闭
    /// </summary>
    public void Close()
    {
        fbxpath.text = string.Empty;
        fbxname.text = string.Empty;
        fbxname2.text = string.Empty;
        tubiaoname.text = string.Empty;
        tubiaoname2.text = string.Empty;;
        tubiaopath.text = string.Empty;
        seccess.text = string.Empty;
        pngpath.text = string.Empty;
        dropdown.captionText.text = "animal";
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 新模型类
    /// </summary>
    class js
    {
        public string name;
        public string modelpath;
        public string name2;
    }
    /// <summary>
    /// 新类别类
    /// </summary>
    public class jsonmoban
    {
        public string name;
        public string imgpath;
        public string chaniesename;
    }
    /// <summary>
    /// 读取类别
    /// </summary>
    /// <param name="path"></param>
    public void readjson(string path)
    {
        string aaa = File.ReadAllText(path, Encoding.GetEncoding("Unicode"));
        List<jsonmoban> aaalist = JsonMapper.ToObject<List<jsonmoban>>(aaa);
        dropdown.options.Clear();
        for (int i = 0; i < aaalist.Count; i++)
        {
            
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = aaalist[i].name;
            dropdown.options.Add(op);
        }
        Dropdown.OptionData op2 = new Dropdown.OptionData();
        op2.text = "自定义";
        dropdown.options.Add(op2);
    }
    /// <summary>
    /// 写入新类别
    /// </summary>
    /// <param name="path">文档路径</param>
    /// <param name="newname">英文名</param>
    /// <param name="imgpath">图标路径</param>
    /// <param name="chiname">中文名</param>
    public void Writejson(string path, string newname, string imgpath, string chiname)
    {
        string newtubiao = "NewUIPic/tubiao/" + newname + "_img";
        string aaa = File.ReadAllText(path, Encoding.GetEncoding("Unicode"));
        print(newtubiao + "***************");
        List<jsonmoban> aaalist = JsonMapper.ToObject<List<jsonmoban>>(aaa);
        jsonmoban jjj = new jsonmoban();
        jjj.name = newname;
        jjj.imgpath = newtubiao;
        jjj.chaniesename = chiname;
        aaalist.Add(jjj);
        aaa = JsonMapper.ToJson(aaalist);
        File.WriteAllText(path, aaa, Encoding.GetEncoding("Unicode"));

        string sspng = Application.streamingAssetsPath + "/NewUIPic/tubiao/" + newname + "_img.png";
        File.Copy(imgpath, sspng);

        //FileStream fso = new FileStream(imgpath,FileMode.Open);
        //FileStream fsn = new FileStream(sspng, FileMode.Create);
        //int d;
        //byte b;
        //while ((d = fso.ReadByte()) != -1)
        //{
        //    b = (byte)d;
        //    print(b);
        //    fsn.WriteByte(b);
        //}

        dropdown.options.Clear();
        for (int i = 0; i < aaalist.Count; i++)
        {           
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = aaalist[i].name;
            dropdown.options.Add(op);
        }
        //Manager.Instace.Init();
        Dropdown.OptionData op2 = new Dropdown.OptionData();
        op2.text = "自定义";
        dropdown.options.Add(op2);

    }
    /// <summary>
    /// 加入旧类别
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <param name="nameeng"></param>
    /// <param name="namecha"></param>
    /// <param name="leibie"></param>
    public void JsonReadorWrite(string name,string path, string nameeng, string namecha, string leibie, string new3dpropath)
    {
        string a = File.ReadAllText(path, Encoding.GetEncoding("Unicode"));
        //string a;
        //if (File.Exists(path))
        //{
        //    a = File.ReadAllText(path, Encoding.GetEncoding("Unicode"));
        //}
        //else
        //{
        //    TextAsset ta = Resources.Load<TextAsset>(name);
        //    a = ta.text;
        //    print(ta.text);
        //}
        JsonData charactersJd = JsonMapper.ToObject(a);
        JsonData character = charactersJd[name];
        if (leibie == "FBX")
        {
            
            //List<js> jsl = new List<js>();
            //jsl = JsonMapper.ToObject<List<js>>(character.ToJson());
            JsonData xxx = new JsonData();
            xxx["name"] = nameeng;
            xxx["modelpath"] = newfbxpath;
            xxx["name2"] = namecha;
            character.ValueAsArray().Add(xxx);
            charactersJd[name] = character;
           
        }
        else if(leibie == "3dpro")
        {
            JsonData xxx = new JsonData();
            xxx["name"] = nameeng;
            xxx["modelpath"] = new3dpropath;
            //xxx["modelpath"] = "AssetBundles/" + nameeng + ".3dpro";
            xxx["name2"] = namecha;
            character.ValueAsArray().Add(xxx);
            charactersJd[name] = character;
        }
        string ss = JsonMapper.ToJson(charactersJd);
        ss = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(ss));

        File.WriteAllText(path, ss, Encoding.GetEncoding("Unicode"));
        seccess.text = "导入成功，5s后重启程序后生效！";
    }
    /// <summary>
    /// 增加模型到新类别
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <param name="nameeng"></param>
    /// <param name="namecha"></param>
    public void JsonReadorWriteNew(string name, string path, string nameeng, string namecha, string leibie, string new3dpropath)
    {
        File.Create(path).Dispose();
        string newjs = "{\"" + name + "\":[]}";
        Debug.Log(newjs);
        JsonData charactersJd = JsonMapper.ToObject(newjs);
        JsonData character = charactersJd[name];
        if (leibie == "FBX")
        {
            //JsonData charactersJd = new JsonData();
            //JsonData character = new JsonData();
            JsonData xxx = new JsonData();
            xxx["name"] = nameeng;
            xxx["modelpath"] = newfbxpath;
            xxx["name2"] = namecha;
            character.ValueAsArray().Add(xxx);
            charactersJd[name] = character;
        }
        else if(leibie == "3dpro")
        {
            JsonData xxx = new JsonData();
            xxx["name"] = nameeng;
            xxx["modelpath"] = new3dpropath;
            //xxx["modelpath"] = "AssetBundles/" + nameeng + ".3dpro";
            xxx["name2"] = namecha;
            character.ValueAsArray().Add(xxx);
            charactersJd[name] = character;
            
        }
        string ss = JsonMapper.ToJson(charactersJd);
        ss = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(ss));
        string paths = Application.streamingAssetsPath + "/json.txt";

        Writejson(paths, tubiaoname.text, tubiaopath.text, tubiaoname2.text);
        File.WriteAllText(path, ss, Encoding.GetEncoding("Unicode"));
        //Manager.Instace.Init();
        seccess.text = "导入成功，5s后重启程序后生效！";
        //gameObject.SetActive(false);
    }
    public void Togglethis()
    {
        IEnumerable<Toggle> togglegroup = gameObject.GetComponent<ToggleGroup>().ActiveToggles();
        foreach (Toggle t in togglegroup)
        {
            if (t.isOn)
            {
                switch (t.name)
                {
                    case "Toggle":
                        {
                            MoXing.onClick.RemoveAllListeners();
                            MoXing.onClick.AddListener(threeDpro);
                            //onoff = 1;
                            break;
                        }
                    case "Toggle1":
                        {
                            MoXing.onClick.RemoveAllListeners();
                            MoXing.onClick.AddListener(FBX);
                            //onoff = 0;
                            break;
                        }
                }
            }
        }
    }
}
