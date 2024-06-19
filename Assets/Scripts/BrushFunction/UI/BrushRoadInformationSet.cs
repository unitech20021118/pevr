using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushRoadInformationSet : MonoBehaviour {

	/// <summary>
	/// 选中的墙壁
	/// </summary>
	private BrushedRoad selectRoad;

	public InputField IF_Length;
	public InputField IF_Width;
	public InputField IF_HeightAboveGround;

	public Button Btn_Delete;
	public Button Btn_ConfirmDel;

	// Use this for initialization
	void Start()
	{
		IF_Length.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Length); });
		IF_Width.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Width); });
		
		IF_HeightAboveGround.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_HeightAboveGround); });
		Btn_ConfirmDel.onClick.AddListener(DeleteObj);
		Btn_Delete.onClick.AddListener(DeleteObj);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(BrushedRoad road)
	{

		selectRoad = road;

		IF_Length.text = road.Length.ToString("G");
		IF_Width.text = road.Width.ToString("G");
		IF_HeightAboveGround.text = road.HeightAboveGround.ToString("G");

	}

	public void OnInputFieldEndEdit(InputField inputField)
	{
		if (selectRoad != null)
		{
			if (inputField == IF_Length)
			{
				selectRoad.Length = float.Parse(inputField.text);
			}
			else if (inputField == IF_Width)
			{
				selectRoad.Width = float.Parse(inputField.text);
			}
			
			else if (inputField == IF_HeightAboveGround)
			{
				selectRoad.HeightAboveGround = float.Parse(inputField.text);
			}

		}
	}
	/// <summary>
	/// 删除物体
	/// </summary>
	public void DeleteObj()
	{
		BrushManager.Instance.BrushedRoadList.Remove(selectRoad);

		Btn_Delete.gameObject.SetActive(false);
		Destroy(selectRoad.gameObject);
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
