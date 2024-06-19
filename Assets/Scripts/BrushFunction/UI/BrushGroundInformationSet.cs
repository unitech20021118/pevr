using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BrushGroundInformationSet : MonoBehaviour 
{
	private BrushedGround selectGround;

	public InputField IF_HeightAboveGround;

	public Button Btn_Delete;
	public Button Btn_ConfirmDel;
	// Use this for initialization
	void Start ()
	{
		IF_HeightAboveGround.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_HeightAboveGround); });
		Btn_ConfirmDel.onClick.AddListener(DeleteObj);
		Btn_Delete.onClick.AddListener(DeleteObj);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Init(BrushedGround ground)
	{

		selectGround = ground;

		IF_HeightAboveGround.text = ground.HeightAboveGround.ToString("G");

	}

	public void OnInputFieldEndEdit(InputField inputField)
	{
		if (selectGround != null)
		{
			if (inputField == IF_HeightAboveGround)
			{
				selectGround.HeightAboveGround = float.Parse(inputField.text);
			}

		}
	}
	/// <summary>
	/// 删除物体
	/// </summary>
	public void DeleteObj()
	{
		//如果是建筑楼层上的地板/墙壁
		if (selectGround.GetComponentInParent<BrushedFloor>())
		{
			BrushedFloor floor = selectGround.GetComponentInParent<BrushedFloor>();
            if (selectGround.Ceiling)
            {
				floor.ceiling = null;
            }
            else
            {
				floor.ground = null;
			}
		}
		else
		{
			//todo
			//BrushManager.Instance.BrushedWallList.Remove(selectGround);
		}

		Btn_Delete.gameObject.SetActive(false);
		
		Destroy(selectGround.gameObject);
		CancelDel();
		gameObject.SetActive(false);
	}

	public void ConfirmDel()
	{
		//Btn_Delete.gameObject.SetActive(true);
		//Btn_ConfirmDel.onClick.RemoveAllListeners();
		//Btn_ConfirmDel.onClick.AddListener(CancelDel);
		//Btn_ConfirmDel.GetComponentInChildren<Text>().text = "取消";
	}

	public void CancelDel()
	{
		//Btn_Delete.gameObject.SetActive(false);
		//Btn_ConfirmDel.onClick.RemoveAllListeners();
		//Btn_ConfirmDel.onClick.AddListener(ConfirmDel);
		//Btn_ConfirmDel.GetComponentInChildren<Text>().text = "删除";
	}
}
