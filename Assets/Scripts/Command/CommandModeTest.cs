using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandModeTest : MonoBehaviour {

    public Camera MainCamera;
    public GameObject UIPanel;
    public InputField userName;
    public InputField passWord;
    public Player player;
    private CommandManager _commandManager;
    private string userNameValue;
    private Vector3 playerPos;
    private void Awake()
    {
        //userNameValue = "";
        
        playerPos = player.transform.position;
        Init();
    }
    void Start()
    {
        _commandManager.ExecutiveCommand(new InputFieldCommand(userName, "", "用户名输入框的初始值"));
    }
    void Update()
    {
        RevocationCommand();
        PlayerMove();
    }
    private void Init() 
    {
        _commandManager = new CommandManager();
        //命令收集者：监听userName的值改变操作
        userName.onEndEdit.AddListener((string value) => 
        { 
            _commandManager.ExecutiveCommand(new InputFieldCommand(userName, value, "修改了用户名输入框的值"));
            //userNameValue = value;
        });
        passWord.onEndEdit.AddListener((string value) =>
        {
            _commandManager.ExecutiveCommand(new InputFieldCommand(passWord,value,"修改密码输入框的值为" + value));
        });
        player.MoveEvent += (Vector3 pos) =>
        {
            _commandManager.ExecutiveCommand(new PlayerCommand(player, playerPos, "角色移动"));
            playerPos = pos;
        };
    }

    private void PlayerMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.name == "Root")
            {
                player.Move(hit.point);
                //player.MovePlayer(hit.point);
            }
        }
    }

    /// <summary>
    /// 撤销操作
    /// </summary>
    private void RevocationCommand()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _commandManager.RevocationCommand();
        }
    }
}
