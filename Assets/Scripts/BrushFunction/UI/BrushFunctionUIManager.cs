using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushFunctionUIManager : MonoBehaviour 
{
	public static BrushFunctionUIManager Instance;
	/// <summary>
	/// 切换绘制模式和普通模式的按钮
	/// </summary>
	public Button ChangeBrushModeButton;
	public GameObject BrushModePanel;
	public GameObject NormanModePanel;
	private Coroutine ChangeBrushModeCoroutine;

	/// <summary>
	/// 当前的界面是否是绘制模式
	/// </summary>
	private bool brushMode;

	/// <summary>
	/// 绘制墙壁
	/// </summary>
	public Button DrawWallButton;
	/// <summary>
	/// 添加结构图的按钮
	/// </summary>
	public Button AddStructureChart;
	/// <summary>
	/// 调整建筑位置的按钮
	/// </summary>
	public Button SetPos;
	/// <summary>
	/// 创建一个自定义建筑
	/// </summary>
	public Button BuildingButton;
	/// <summary>
	/// 自定义建筑的地基创建完成
	/// </summary>
	public Button FoundtationButton;
	/// <summary>
	/// 自定义建筑地基修改的按钮
	/// </summary>
	public Button FoundtationEditButton;
	/// <summary>
	/// 创建建筑外围墙壁的按钮
	/// </summary>
	public Button CreatePeripheralWallsButton;
	/// <summary>
	/// 创建建筑内部墙壁
	/// </summary>
	public Button CreateInteriorWallsButton;
	/// <summary>
	/// 绘制路面
	/// </summary>
	public Button DrawRoadButton;
	/// <summary>
	/// 显示可替换贴图的界面
	/// </summary>
	public GameObject WallTextureItemPanel;
	public GameObject DoorTextureItemPanel;
	/// <summary>
	/// item的父物体
	/// </summary>
	public Transform WallTextureItemPanelContent;
	public Transform DoorTextureItemPanelContent;
	/// <summary>
	/// 贴图的item
	/// </summary>
	private GameObject TextureItemPrefab;

	public Button CreateDoor;
	public Button CreateWindow;
	/// <summary>
	/// 自定义建筑时的提示文字
	/// </summary>
	public GameObject BrushBuildingTip;



	void Awake()
    {
		Instance = this;
		WallTexture.Init();
	}
	// Use this for initialization
	void Start () 
	{
		DrawWallButton.onClick.AddListener(OnDrawWallButtonClick);
		DrawRoadButton.onClick.AddListener(OnDrawRoadButtonClick);
		BuildingButton.onClick.AddListener(OnBuildingButtonClick);
		FoundtationButton.onClick.AddListener(OnFoundtationButtonClick);
		FoundtationEditButton.onClick.AddListener(OnDrawPeripheralWalls);
		AddStructureChart.onClick.AddListener(OnAddStructureChartButtonClick);
		SetPos.onClick.AddListener(OnSetPosClick);
		//CreatePeripheralWallsButton.onClick.AddListener(OnDrawPeripheralWalls);
		ChangeBrushModeButton.onClick.AddListener(OnChangeBrushModeButtonClick);
		TextureItemPrefab = Resources.Load<GameObject>("Brush/BrushTextureItem");
		CreateDoor.onClick.AddListener(BooleanRtManager.Instance.OnCreateDoorButtonClick);
		CreateWindow.onClick.AddListener(BooleanRtManager.Instance.OnCreateWindowButtonClick);


	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnChangeBrushModeButtonClick()
    {
        if (ChangeBrushModeCoroutine==null)
        {
			ChangeBrushModeCoroutine = StartCoroutine(DoChangeBrushMode());
		}
		
	}
	public IEnumerator DoChangeBrushMode()
    {
		float x = Manager.Instace.GetComponent<RectTransform>().sizeDelta.x / 2;
		float t = 0f;
		if (brushMode)
        {
            while (t<0.8f)
            {
				BrushModePanel.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(BrushModePanel.GetComponent<RectTransform>().transform.localPosition, new Vector3(-x - 12f, BrushModePanel.GetComponent<RectTransform>().transform.localPosition.y, BrushModePanel.GetComponent<RectTransform>().transform.localPosition.z), 0.2f);
				NormanModePanel.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(NormanModePanel.GetComponent<RectTransform>().transform.localPosition, new Vector3(-x + 10f, NormanModePanel.GetComponent<RectTransform>().transform.localPosition.y, NormanModePanel.GetComponent<RectTransform>().transform.localPosition.z), 0.2f);
				t += Time.deltaTime;
				yield return null;
			}
			brushMode = false;
        }
        else
        {
            while (t<0.8f)
            {
				NormanModePanel.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(NormanModePanel.GetComponent<RectTransform>().transform.localPosition, new Vector3(-x - 12f, NormanModePanel.GetComponent<RectTransform>().transform.localPosition.y, NormanModePanel.GetComponent<RectTransform>().transform.localPosition.z), 0.2f);
				BrushModePanel.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(BrushModePanel.GetComponent<RectTransform>().transform.localPosition, new Vector3(-x + 10f, BrushModePanel.GetComponent<RectTransform>().transform.localPosition.y, BrushModePanel.GetComponent<RectTransform>().transform.localPosition.z), 0.2f);
				t += Time.deltaTime;
				yield return null;
			}
			brushMode = true;
		}
		ChangeBrushModeCoroutine = null;
	}

	/// <summary>
	/// 绘制墙壁
	/// </summary>
	public void OnDrawWallButtonClick()
    {
		BrushManager.Instance.StartDraw();
	}
	/// <summary>
	/// 绘制路面
	/// </summary>
	public void OnDrawRoadButtonClick()
    {
		BrushManager.Instance.StartDrawRoad(false);
    }
	/// <summary>
	/// 自定义建筑
	/// </summary>
	public void OnBuildingButtonClick()
    {
		//BrushManager.Instance.CreatBudindingFoundation();
		//BrushManager.Instance.CreatBuildingPrefab();
		//FoundtationButton.gameObject.SetActive(true);
		BrushManager.Instance.CreateNewBuilding();
		FoundtationEditButton.gameObject.SetActive(true);
		AddStructureChart.gameObject.SetActive(true);
		SetPos.gameObject.SetActive(true);
	}

	public void OnSetPosClick()
    {
		//修改建筑的位置
		BrushManager.Instance.SetBuildingPosition();

	}
	/// <summary>
	/// 创建外围墙壁
	/// </summary>
	public void OnDrawPeripheralWalls()
    {
		BrushManager.Instance.CreatePeripheralWalls();
		FoundtationEditButton.gameObject.SetActive(false);
		FoundtationButton.gameObject.SetActive(true);
		SetPos.gameObject.SetActive(false);
		BrushBuildingTip.SetActive(true);
	}
	/// <summary>
	/// 地基创建完成
	/// </summary>
	public void OnFoundtationButtonClick()
    {

		BrushManager.Instance.CompleteBuildingFoundation();
		FoundtationButton.gameObject.SetActive(false);
		BrushBuildingTip.SetActive(false);
	}
	/// <summary>
	/// 修改地基按钮点击时
	/// </summary>
	public void OnFoundationEditButtonClick()
    {
		BrushManager.Instance.CreatBudindingFoundation();
		FoundtationEditButton.gameObject.SetActive(false);
		FoundtationButton.gameObject.SetActive(true);
	}
	/// <summary>
	/// 添加结构图按钮点击时
	/// </summary>
	public void OnAddStructureChartButtonClick()
    {
		AddStructureChart.gameObject.SetActive(false);
		//todo 功能
		string imgName = IOHelper.GetImageName();
		StartCoroutine(GetSpriteByPath(imgName));
	}

	IEnumerator GetSpriteByPath(string path)
    {
		string url = "file://" + path;
		WWW www = new WWW(url);
		yield return www;
		Texture2D tex2d = www.texture;
		Sprite sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector3(0.5f,0.5f,0));
		BrushManager.Instance.SetStructureChart(sprite);
	}
	public void OnWallTextureButtonClick()
    {
		DoorTextureItemPanel.SetActive(false);
		if (WallTextureItemPanelContent.childCount>0)
        {
			WallTextureItemPanel.SetActive(true);
		}
        else
        {
			StartCoroutine(OpenWallTexturePanel());
        }
    }

	public void OnDoorTextrueButtonClick()
    {
		WallTextureItemPanel.SetActive(false);
		if (DoorTextureItemPanelContent.childCount > 0)
		{
			DoorTextureItemPanel.SetActive(true);
		}
		else
		{
			StartCoroutine(OpenDoorTexturePanel());
		}
	}

	public IEnumerator OpenDoorTexturePanel()
    {
		DoorTextureItemPanel.SetActive(true);

		if (WallTexture.brushDoorTextureDatas.Count > 0)
		{
			for (int i = 0; i < WallTexture.brushDoorTextureDatas.Count; i++)
			{
				GameObject obj = Instantiate(TextureItemPrefab);
				obj.name = WallTexture.brushDoorTextureDatas[i].E_name;
				obj.transform.Find("Text").GetComponent<Text>().text = WallTexture.brushDoorTextureDatas[i].C_name;
				obj.transform.SetParent(DoorTextureItemPanelContent);
				obj.transform.localScale = Vector3.one;
				//Debug.Log(WallTexture.LoadWallTexture(WallTexture.brushWallTextureDatas[i]).mainTexture.name);
				//Texture2D texture2D = (Texture2D)WallTexture.LoadDoorTexture(WallTexture.brushDoorTextureDatas[i]).material.mainTexture;
				obj.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Materials/Textures/yulantu/" + WallTexture.brushDoorTextureDatas[i].E_name);
				int a = i;
				obj.GetComponent<Button>().onClick.AddListener(delegate { ChangeTexture(WallTexture.LoadDoorTexture(WallTexture.brushDoorTextureDatas[a])); });
				yield return null;
			}
		}
	}

	/// <summary>
	/// 打开墙壁贴图的面板
	/// </summary>
	public IEnumerator OpenWallTexturePanel()
    {
		WallTextureItemPanel.SetActive(true);
		
		if (WallTexture.brushWallTextureDatas.Count > 0)
        {
            for (int i = 0; i < WallTexture.brushWallTextureDatas.Count; i++)
            {
				GameObject obj = Instantiate(TextureItemPrefab);
				obj.name = WallTexture.brushWallTextureDatas[i].E_name;
				obj.transform.Find("Text").GetComponent<Text>().text = WallTexture.brushWallTextureDatas[i].C_name;
				obj.transform.SetParent(WallTextureItemPanelContent);
				obj.transform.localScale = Vector3.one;
				//Debug.Log(WallTexture.LoadWallTexture(WallTexture.brushWallTextureDatas[i]).mainTexture.name);
				//Texture2D texture2D = 
				//Debug.LogError("Materials/Textures/" + WallTexture.brushWallTextureDatas[i].E_name);
				//Texture2D texture2D = (Texture2D)WallTexture.LoadWallTexture(WallTexture.brushWallTextureDatas[i]).material.mainTexture;
				obj.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Materials/Textures/yulantu/" + WallTexture.brushWallTextureDatas[i].E_name);
				int a = i;
				obj.GetComponent<Button>().onClick.AddListener(delegate { ChangeTexture(WallTexture.LoadWallTexture(WallTexture.brushWallTextureDatas[a]), WallTexture.LoadWallTexture(WallTexture.brushWallTextureDatas[a],true)); });
				yield return null;
			}
        }
    }

	public void ChangeTexture(ChangeTextureMaterial material, ChangeTextureMaterial material_G = null)
    {
		BrushManager.Instance.ChangeTextureMaterial = material;
		BrushManager.Instance.ChangeTextureMarerial_G = material_G;
	}
}

public class ChangeTextureMaterial
{
	public Material material;
	public ChangeTextureMaterialType type;
	public ChangeTextureMaterial() { }
	public ChangeTextureMaterial(Material material, ChangeTextureMaterialType type)
    {
		this.material = material;
		this.type = type;
    }
}

public enum ChangeTextureMaterialType
{
	wall,
	door
}
