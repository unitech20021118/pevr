using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerUI : ActionUI {
    public InputField playerNameField;
    public Text textPrefab;
    public Transform content;
    List<string> playerNames;
	SetPlayer setPlayer;
    SetPlayerInforma setPlayerInforma;
    

	public override Action<Main> CreateAction()
	{
        playerNames = new List<string>();
		setPlayer = new SetPlayer ();
		action = setPlayer;
        actionInforma = new SetPlayerInforma(true);
        setPlayerInforma = (SetPlayerInforma)actionInforma;
        actionInforma.name = "SetPlayer";
		setPlayer.isOnce = true;
        GetStateInfo().actionList.Add(actionInforma);
        return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma actionInforma)
	{
        playerNames = new List<string>();

        this.actionInforma = (SetPlayerInforma)actionInforma;
        setPlayerInforma = (SetPlayerInforma)actionInforma;
		setPlayer = new SetPlayer ();
		action = setPlayer;
        if (Manager.Instace.playerNames == null)
        {
			Manager.Instace.playerNames = new List<string>();
        }
        foreach (string pName in setPlayerInforma.playerNames)
        {
            playerNames.Add(pName);
            GameObject temp = Instantiate<GameObject>(textPrefab.gameObject, content);
            temp.GetComponent<Text>().text = pName;
            temp.SetActive(true);
            Manager.Instace.playerNames.Add(pName);
        }
        return base.LoadAction(actionInforma);
	}

    public void AddPlayer()
    {
        if (playerNameField.text != "")
        {
            if (!playerNames.Contains(playerNameField.text))
            {
                playerNames.Add(playerNameField.text);
                GameObject temp = Instantiate<GameObject>(textPrefab.gameObject, content);
                temp.GetComponent<Text>().text = playerNameField.text;
                temp.SetActive(true);
                if (Manager.Instace.playerNames == null)
                {
                    Manager.Instace.playerNames = new List<string>();
                }
                Manager.Instace.playerNames.Add(playerNameField.text);
                setPlayerInforma.playerNames = playerNames.ToArray();
            }
        }
    }
}
