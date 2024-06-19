using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
public class LoginPanel : MonoBehaviour {
    private Button loginBtn;
    private Button registerBtn;
    //private Button createRoomBtn;
    private InputField userInput;
    private InputField passwordInput;
    private LoginRequest loginRequest;
    private RegisterRequest registerRequest;
    private CreateRoomRequest createRoomRequest;
    private bool succeed = false;
	// Use this for initialization
	void Start () {
        loginBtn = transform.Find("login").GetComponent<Button>();
        registerBtn=transform.Find("register").GetComponent<Button>();
        //createRoomBtn = transform.Find("createRoom").GetComponent<Button>();
        userInput=transform.Find("username").GetComponent<InputField>();
        passwordInput=transform.Find("password").GetComponent<InputField>();
        loginRequest=GetComponent<LoginRequest>();
        registerRequest = GetComponent<RegisterRequest>();
        //createRoomRequest=GetComponent<CreateRoomRequest>();
        loginBtn.onClick.AddListener(Login);
        registerBtn.onClick.AddListener(Register);
        //createRoomBtn.onClick.AddListener(CreateRoom);
	}

    void Update()
    {
        if (succeed)
        {
            GameObject obj = (GameObject)Resources.Load("Prefabs/CreateRoomPanel");
            GameObject createRoomPanel = Instantiate(obj);
            Destroy(this.gameObject);
        }
    }

    void Login()
    {
        loginRequest.SendRequest(userInput.text, passwordInput.text);
    }

    void Register()
    {
        registerRequest.SendRequest(userInput.text, passwordInput.text);
    }

    //void CreateRoom()
    //{
    //    createRoomRequest.SendRequest();
    //}

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            Debug.Log("登入成功");
            succeed = true;

        }
        else
        {
            Debug.Log("登入失败");
        }
    }

    
}
