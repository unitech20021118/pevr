using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushFloorInformationSet : MonoBehaviour {

	/// <summary>
	/// 正在编辑的楼层
	/// </summary>
	private BrushedFloor selectFloor;

	public Button Btn_GroundActiveTrue;
	public Button Btn_GroundActiveFalse;
	public Button Btn_GroundCreate;
	public Button Btn_CeilingActiveTrue;
	public Button Btn_CeilingActiveFalse;
	public Button Btn_CeilingCreate;



	// Use this for initialization
	void Start () 
	{
		Btn_GroundActiveTrue.onClick.AddListener(delegate { OnButtonClick(Btn_GroundActiveTrue); });
		Btn_GroundActiveFalse.onClick.AddListener(delegate { OnButtonClick(Btn_GroundActiveFalse); });
		Btn_GroundCreate.onClick.AddListener(delegate { OnButtonClick(Btn_GroundCreate); });
		Btn_CeilingActiveTrue.onClick.AddListener(delegate { OnButtonClick(Btn_CeilingActiveTrue); });
		Btn_CeilingActiveFalse.onClick.AddListener(delegate { OnButtonClick(Btn_CeilingActiveFalse); });
		Btn_CeilingCreate.onClick.AddListener(delegate { OnButtonClick(Btn_CeilingCreate); });
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void Init(BrushedFloor brushedFloor)
    {
		selectFloor = brushedFloor;
    }

	public void OnButtonClick(Button button)
    {
        if (selectFloor!=null)
        {
            if (selectFloor.ground!=null)
            {
				if (button == Btn_GroundActiveTrue)
				{
					selectFloor.ground.gameObject.SetActive(true);
                }
                else if (button == Btn_GroundActiveFalse)
                {
					selectFloor.ground.gameObject.SetActive(false);
				}
            }
            else
            {
                if (button == Btn_GroundCreate)
                {
					//todo
					Debug.LogError("11");
					BrushedBuilding brushedBuilding = selectFloor.transform.parent.GetComponent<BrushedBuilding>();
					BrushManager.Instance.AutoCreatGroundOfBuilding(brushedBuilding.Foundation, selectFloor);
                }
            }
            if (selectFloor.ceiling!=null)
            {
				if (button == Btn_CeilingActiveTrue)
				{
					selectFloor.ceiling.gameObject.SetActive(true);
				}
				else if (button == Btn_CeilingActiveFalse)
				{
					selectFloor.ceiling.gameObject.SetActive(false);
				}
            }
            else
            {
				if (button == Btn_CeilingCreate)
				{
					//todo
					BrushedBuilding brushedBuilding = selectFloor.transform.parent.GetComponent<BrushedBuilding>();
					BrushManager.Instance.AutoCreatCeilingOfBuilding(brushedBuilding.Foundation, selectFloor);
				}
			}
		}
    }
}
