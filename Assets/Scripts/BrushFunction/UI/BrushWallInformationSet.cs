using UnityEngine;
using UnityEngine.UI;


public class BrushWallInformationSet : MonoBehaviour 
{
	/// <summary>
	/// 选中的墙壁
	/// </summary>
	private BrushedWall selectWall;

	public InputField IF_Length;
	public InputField IF_Width;
	public InputField IF_Height;
	public InputField IF_HeightAboveGround;

	public Button Btn_Delete;
	public Button Btn_ConfirmDel;
	
	// Use this for initialization
	void Start () 
	{
		IF_Length.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Length); });
		IF_Width.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Width); });
		IF_Height.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_Height); });
		IF_HeightAboveGround.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(IF_HeightAboveGround); });
		Btn_ConfirmDel.onClick.AddListener(DeleteObj);
		Btn_Delete.onClick.AddListener(DeleteObj);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Init(BrushedWall wall)
    {

		selectWall = wall;

		IF_Length.text = wall.Length.ToString("G");
		IF_Width.text = wall.Width.ToString("G");
		IF_Height.text = wall.Height.ToString("G");
		IF_HeightAboveGround.text = wall.HeightAboveGround.ToString("G");

	}

	public void OnInputFieldEndEdit(InputField inputField)
    {
        if (selectWall!=null)
        {
            if (inputField == IF_Length)
            {
				selectWall.Length = float.Parse(inputField.text);
			}
			else if (inputField == IF_Width)
			{
				selectWall.Width = float.Parse(inputField.text);
			}
			else if (inputField == IF_Height)
			{
				selectWall.Height = float.Parse(inputField.text);
            }
            else if (inputField == IF_HeightAboveGround)
            {
				selectWall.HeightAboveGround = float.Parse(inputField.text);
            }

		}
    }
	/// <summary>
	/// 删除物体
	/// </summary>
	public void DeleteObj()
    {
		//如果是建筑楼层上的墙壁
        if (selectWall.GetComponentInParent<BrushedFloor>())
        {
			BrushedFloor floor = selectWall.GetComponentInParent<BrushedFloor>();
			floor.InteriorWalls.Remove(selectWall);
            
        }
        else
        {
			BrushManager.Instance.BrushedWallList.Remove(selectWall);
        }

		Btn_Delete.gameObject.SetActive(false);
		if (selectWall.doors != null && selectWall.doors.Count > 0)
		{
			for (int i = 0; i < selectWall.doors.Count; i++)
			{
				Destroy(selectWall.doors[i].gameObject);
			}
		}
		Destroy(selectWall.gameObject);
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
