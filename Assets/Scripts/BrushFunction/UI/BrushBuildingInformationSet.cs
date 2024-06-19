using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushBuildingInformationSet : MonoBehaviour 
{
	public InputField IF_SetFloorNum;
	public InputField IF_EditFloor;
	private BrushedBuilding SelectBuilding;

	public Button Btn_Delete;
	public Button Btn_ConfirmDel;
	// Use this for initialization
	void Start () 
	{
		Btn_ConfirmDel.onClick.AddListener(DeleteObj);
		Btn_Delete.onClick.AddListener(DeleteObj);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	/// <summary>
	/// 设置楼层数量
	/// </summary>
	public void SetFloorNum()
    {
		int floorNum = int.Parse(IF_SetFloorNum.text);
		floorNum = Mathf.Clamp(floorNum, 0, 100);
		IF_SetFloorNum.text = floorNum.ToString();
		SelectBuilding.SetFloorNum(floorNum);

	}
	public void EditFloor()
    {
		int floornum = int.Parse(IF_EditFloor.text);

		floornum = Mathf.Clamp(floornum, 0, int.Parse(IF_SetFloorNum.text));
		IF_EditFloor.text = floornum.ToString();
        if (floornum == 0)
        {
			return;
        }
		BrushManager.Instance.EditBuildingFloor(SelectBuilding, floornum);
	}

	public void Init(BrushedBuilding building)
    {
		SelectBuilding = building;
		IF_SetFloorNum.text = building.Floor.Count.ToString();
	}
	/// <summary>
	/// 删除物体
	/// </summary>
	public void DeleteObj()
	{
		BrushManager.Instance.BrushedBuildingList.Remove(SelectBuilding);
        Destroy(SelectBuilding.gameObject);
		Manager.Instace.dragUIMoveObjOnGround.gameObject.SetActive(false);
        Btn_Delete.gameObject.SetActive(false);
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
