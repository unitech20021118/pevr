using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

using System.Net.Sockets;
using System;
using Common;
public class Loader : MonoBehaviour {
	static Loader instance;
	public static Loader Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.Find("CreateRoomPanel(Clone)").GetComponent<Loader>();
			}
			return instance;
		}
	}
	private GameObject btnPrefab;
	private GameObject btnPrefab2;
	private GameObject btnPrefab3;
	public Transform content;
	public Transform contentChoosed;
	public Button ChooseScene;
	public Button loadScene;
	bool isChooseScene = false;
	bool isChoose = false;
	public GameObject pic;
	private const string IP = "192.168.1.4";
	private const int PORT = 6699;
	string taskname = null;
	Socket clientSocket;
	Message message = new Message();
	private ListRoomRequest listRoomRequest;
	private JoinRoomRequest joinRoomRequest;
	private StartGameRequest startGameRequest;
	public GameObject taskStartPanel;
	public Text Username;
	private string roomOwner;
	public Transform roomcontent;
	public Button startBtn;

    public GameObject panel_Loading;
    public GameObject topui;

    //edit by 吕存全
    private bool isVR;
    //--------------
    public bool BuildVR;

	void Awake(){
        if (isVR)
        {
            VRSwitch.Instance.SetVRState(true);
        }
		btnPrefab = (GameObject)Resources.Load("Prefabs/_scenePrefab");
		btnPrefab2 = (GameObject)Resources.Load("Prefabs/_scenePrefabChoose");
		btnPrefab3 = (GameObject)Resources.Load("Prefabs/usersInfo");
		//taskStartPanel = transform.Find("Image").gameObject;
		listRoomRequest=GetComponent<ListRoomRequest>();
		joinRoomRequest = GetComponent<JoinRoomRequest>();
		startGameRequest=GetComponent<StartGameRequest>();
	}

    void Start()
    {
        //VRSwitch.SetVR (true);

        //Manager.Instace.SetMainActive();
        //string path = "SaveFile/saved.txt";
        //string filename = PlayerPrefs.GetString("filename");
        //string path = "SaveFile/" + filename;

        // edit by kuai
        if (BuildVR)
        {
            VRSwitch.Instance.SetVRState(true);
        }
        StartCoroutine(DoWaitToSetMainActive());


    }

    IEnumerator DoWaitToSetMainActive()
    {
        panel_Loading.SetActive(true);
        yield return null;
        if (!SceneCtrl.isAction)
        {
            LoadManager.Instance.IsPublish = true;
            BinaryFormatter bf = new BinaryFormatter();

            DirectoryInfo folder = new DirectoryInfo("SaveFile");
            FileInfo[] files = folder.GetFiles("*.pevrsf");
			//Debug.LogError(folder);
            string path = files[0].FullName;
            //string path = Application.dataPath + "/SaveFile/automatic.pevrsf";
            FileStream file = File.Open(path, FileMode.Open);
            Manager.Instace.allDataInformation = (AllDataInformation)bf.Deserialize(file);
            file.Close();
            StartCoroutine(Manager.Instace.InitialAll());

            while (Manager.Instace.ISOpen == false)
            {
                yield return null;
            }
            Manager.Instace.SetMainActive();
            yield return new WaitForSeconds(0.2f);
            panel_Loading.SetActive(false);
        }
    }

    void BeginGame()
	{
		startGameRequest.SendRequest();
	}

	void CreateTask(string name)
	{
		taskname = name;
		isChoose = true;
		PlayerPrefs.SetString("filename", name);
	}

	void  SetPath(string path,string username)
	{
		PlayerPrefs.SetString("filename", path);
		isChooseScene = true;
		roomOwner = username;
	}

	void Load()
	{
		if (isChooseScene)
		{
			//taskStartPanel.SetActive(true);
			//Username.text = roomOwner;
			//GameObject obj = Instantiate(btnPrefab2);
			//obj.transform.SetParent(roomcontent);
			//obj.transform.localScale = Vector3.one;
			//obj.transform.FindChild("Task").GetComponent<Text>().text = Manager.Instace.playerMng.UserData.Username;
			//obj.transform.FindChild("User").GetComponent<Text>().text = "房客";
			joinRoomRequest.SendRequest(roomOwner);
		}
	}

	//创建任务房间
	void Create(string btnname,string username)
	{
		if (isChoose&&taskname!=null)
		{

			CreateRoomBtn(btnname,username);
			Manager.Instace.GetComponent<ChooseSceneRequest>().SendRequest(taskname);
			taskStartPanel.SetActive(true);
			Username.text = username;

		}
	}


	public void CreateRoomBtn(string btnname, string username)
	{
		GameObject obj = Instantiate(btnPrefab2);
		obj.transform.SetParent(contentChoosed);
		obj.transform.localScale = Vector3.one;
		//obj.GetComponentInChildren<Text>().text = btnname;
		obj.transform.FindChild("Task").GetComponent<Text>().text = btnname;
		obj.transform.FindChild("User").GetComponent<Text>().text = username;
		obj.GetComponent<Button>().onClick.AddListener(delegate() { SetPath(btnname,username); });
	}


	public void UpdateRoom(List<UserData> users)
	{
		foreach (Transform t in roomcontent)
		{
			Destroy(t.gameObject);
		}
		foreach (UserData u in users)
		{
			GameObject obj = Instantiate(btnPrefab3);
			obj.transform.SetParent(roomcontent);
			obj.transform.localScale = Vector3.one;
			obj.transform.FindChild("User").GetComponent<Text>().text = u.Username;
		}

	}
}
