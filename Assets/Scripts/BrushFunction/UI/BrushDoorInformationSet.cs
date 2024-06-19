using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushDoorInformationSet : MonoBehaviour {

	/// <summary>
	/// 选中的门
	/// </summary>
	private BrushedDoor selectDoor;

	public InputField IF_Length;
	public InputField IF_Width;
	public InputField IF_Height;
	public InputField IF_HeightAboveGround;
	/// <summary>
	/// 修改位置的按钮
	/// </summary>
	public Button Btn_MovePosition;

	public Button Btn_Delete;
	public Button Btn_ConfirmDel;

	// Use this for initialization
	void Start()
	{
		IF_Length.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Length); });
		IF_Width.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Width); });
		IF_Height.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Height); });
		IF_HeightAboveGround.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_HeightAboveGround); });
		Btn_MovePosition.onClick.AddListener(MoveDoorPosition);
		Btn_ConfirmDel.onClick.AddListener(DeleteObj);
		Btn_Delete.onClick.AddListener(DeleteObj);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(BrushedDoor door)
	{

		selectDoor = door;

		IF_Length.text = door.Length.ToString("G");
		IF_Width.text = door.Width.ToString("G");
		IF_Height.text = door.Height.ToString("G");
		IF_HeightAboveGround.text = door.HeightAboveGround.ToString("G");

	}

	public void OnInputFieldEndEdit(InputField inputField)
	{
		if (selectDoor != null)
		{
			if (inputField == IF_Length)
			{
				selectDoor.Length = float.Parse(inputField.text);
			}
			else if (inputField == IF_Width)
			{
				selectDoor.Width = float.Parse(inputField.text);
			}
			else if (inputField == IF_Height)
			{
				selectDoor.Height = float.Parse(inputField.text);
            }
            else if (inputField == IF_HeightAboveGround)
            {
				selectDoor.HeightAboveGround = float.Parse(inputField.text);
			}

		}
	}
	/// <summary>
	/// 移动门的位置
	/// </summary>
	public void MoveDoorPosition()
    {
		BooleanRtManager.Instance.MoveDoorPosition(selectDoor);
	}

	/// <summary>
	/// 删除物体
	/// </summary>
	public void DeleteObj()
	{
		selectDoor.wall.doors.Remove(selectDoor);
		BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(selectDoor.wall);
		Destroy(selectDoor.gameObject);
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
