using System;
using UnityEngine;
using UnityEngine.UI;

public class TransformUI : MonoBehaviour
{
	public static TransformUI Instance;

	private GameObject parentGO;

	public static bool isTrans;

	private string uiName;

	private GameObject traObj;

	public GameObject transformObj;

	private InputField positionX;

	private InputField positionY;

	private InputField positionZ;

	private InputField rotationX;

	private InputField rotationY;

	private InputField rotationZ;

	private InputField scaleX;

	private InputField scaleY;

	private InputField scaleZ;

	private Button touchDownButton;

	private Button adjustButton;

	private float valueF;

	private void Awake()
	{
		this.parentGO = base.gameObject;
		TransformUI.Instance = this.parentGO.GetComponent<TransformUI>();
		this.positionX = transform.Find("Position/XText/InputField").GetComponent<InputField>();
        this.positionY = transform.Find("Position/YText/InputField").GetComponent<InputField>();
        this.positionZ = transform.Find("Position/ZText/InputField").GetComponent<InputField>();
		this.rotationX = transform.Find("Rotation/XText/InputField").GetComponent<InputField>();
        this.rotationY = transform.Find("Rotation/YText/InputField").GetComponent<InputField>();
        this.rotationZ = transform.Find("Rotation/ZText/InputField").GetComponent<InputField>();
		this.scaleX = transform.Find("Scale/XText/InputField").GetComponent<InputField>();
		this.scaleY = transform.Find("Scale/YText/InputField").GetComponent<InputField>();
		this.scaleZ = transform.Find("Scale/ZText/InputField").GetComponent<InputField>();
        //this.touchDownButton = this.parentGO.transform.Find("TouchDownButton").GetComponent<Button>();
        //this.adjustButton = this.parentGO.transform.Find("AdjustButton").GetComponent<Button>();
        //this.traObj = GameObject.Find("transform").gameObject;
	}

	private void Start()
	{
		this.positionX.onValueChanged.AddListener(delegate(string var)
		{
			if (this.positionX.text == string.Empty)
			{
				this.positionX.text = "0";
				this.transformObj.transform.position = new Vector3(0f, this.transformObj.transform.position.y, this.transformObj.transform.position.z);
			}
			else if (float.TryParse(this.positionX.text, out this.valueF))
			{
				this.transformObj.transform.position = new Vector3(this.valueF, this.transformObj.transform.position.y, this.transformObj.transform.position.z);
			}
		});
		this.positionY.onValueChanged.AddListener(delegate(string var)
		{
			if (this.positionY.text == string.Empty)
			{
				this.positionY.text = "0";
				this.transformObj.transform.position = new Vector3(this.transformObj.transform.position.x, 0f, this.transformObj.transform.position.z);
			}
			else if (float.TryParse(this.positionY.text, out this.valueF))
			{
				this.transformObj.transform.position = new Vector3(this.transformObj.transform.position.x, this.valueF, this.transformObj.transform.position.z);
			}
		});
		this.positionZ.onValueChanged.AddListener(delegate(string var)
		{
			if (this.positionZ.text == string.Empty)
			{
				this.positionZ.text = "0";
				this.transformObj.transform.position = new Vector3(this.transformObj.transform.position.x, this.transformObj.transform.position.y, 0f);
			}
			else if (float.TryParse(this.positionZ.text, out this.valueF))
			{
				this.transformObj.transform.position = new Vector3(this.transformObj.transform.position.x, this.transformObj.transform.position.y, this.valueF);
			}
		});
		this.rotationX.onValueChanged.AddListener(delegate(string var)
		{
			if (this.rotationX.text == string.Empty)
			{
				this.rotationX.text = "0";
				this.transformObj.transform.localEulerAngles = new Vector3(0f, this.transformObj.transform.localEulerAngles.y, this.transformObj.transform.localEulerAngles.z);
			}
			else if (float.TryParse(this.rotationX.text, out this.valueF))
			{
				this.transformObj.transform.localEulerAngles = new Vector3(this.valueF, this.transformObj.transform.localEulerAngles.y, this.transformObj.transform.localEulerAngles.z);
			}
		});
		this.rotationY.onValueChanged.AddListener(delegate(string var)
		{
			if (this.rotationY.text == string.Empty)
			{
				this.rotationY.text = "0";
				this.transformObj.transform.localEulerAngles = new Vector3(this.transformObj.transform.localEulerAngles.x, 0f, this.transformObj.transform.localEulerAngles.z);
			}
			else if (float.TryParse(this.rotationY.text, out this.valueF))
			{
				this.transformObj.transform.localEulerAngles = new Vector3(this.transformObj.transform.localEulerAngles.x, this.valueF, this.transformObj.transform.localEulerAngles.z);
			}
		});
		this.rotationZ.onValueChanged.AddListener(delegate(string var)
		{
			if (this.rotationZ.text == string.Empty)
			{
				this.rotationZ.text = "0";
				this.transformObj.transform.localEulerAngles = new Vector3(this.transformObj.transform.localEulerAngles.x, this.transformObj.transform.localEulerAngles.y, 0f);
			}
			else if (float.TryParse(this.rotationZ.text, out this.valueF))
			{
				this.transformObj.transform.localEulerAngles = new Vector3(this.transformObj.transform.localEulerAngles.x, this.transformObj.transform.localEulerAngles.y, this.valueF);
			}
		});
		this.scaleX.onValueChanged.AddListener(delegate(string var)
		{
			if (this.scaleX.text == string.Empty)
			{
				this.scaleX.text = "0";
				this.transformObj.transform.localScale = new Vector3(0f, this.transformObj.transform.localScale.y, this.transformObj.transform.localScale.z);
			}
			else if (float.TryParse(this.scaleX.text, out this.valueF))
			{
				this.transformObj.transform.localScale = new Vector3(this.valueF, this.transformObj.transform.localScale.y, this.transformObj.transform.localScale.z);
			}
		});
		this.scaleY.onValueChanged.AddListener(delegate(string var)
		{
			if (this.scaleY.text == string.Empty)
			{
				this.scaleY.text = "0";
				this.transformObj.transform.localScale = new Vector3(this.transformObj.transform.localScale.x, 0f, this.transformObj.transform.localScale.z);
			}
			else if (float.TryParse(this.scaleY.text, out this.valueF))
			{
				this.transformObj.transform.localScale = new Vector3(this.transformObj.transform.localScale.x, this.valueF, this.transformObj.transform.localScale.z);
			}
		});
		this.scaleZ.onValueChanged.AddListener(delegate(string var)
		{
			if (this.scaleZ.text == string.Empty)
			{
				this.scaleZ.text = "0";
				this.transformObj.transform.localScale = new Vector3(this.transformObj.transform.localScale.x, this.transformObj.transform.localScale.y, 0f);
			}
			else if (float.TryParse(this.scaleZ.text, out this.valueF))
			{
				this.transformObj.transform.localScale = new Vector3(this.transformObj.transform.localScale.x, this.transformObj.transform.localScale.y, this.valueF);
			}
		});
        //this.touchDownButton.onClick.AddListener(delegate
        //{
        //    this.transformObj.transform.localPosition = new Vector3(this.transformObj.transform.localPosition.x, 0f, this.transformObj.transform.localPosition.z);
        //    this.transformObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //    this.SetOBJ(this.transformObj, this.uiName);
        //});
        //this.adjustButton.onClick.AddListener(delegate
        //{
        //    this.transformObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //    this.SetOBJ(this.transformObj, this.uiName);
        //});
	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (this.positionX.isFocused)
            {
                this.positionY.Select();
            }
            else if (this.positionY.isFocused)
            {
                this.positionZ.Select();
            }
            else if (this.positionZ.isFocused)
            {
                this.positionX.Select();
            }
            if (this.rotationX.isFocused)
            {
                this.rotationY.Select();
            }
            else if (this.rotationY.isFocused)
            {
                this.rotationZ.Select();
            }
            else if (this.rotationZ.isFocused)
            {
                this.rotationX.Select();
            }
            if (this.scaleX.isFocused)
            {
                this.scaleY.Select();
            }
            else if (this.scaleY.isFocused)
            {
                this.scaleZ.Select();
            }
            else if (this.scaleZ.isFocused)
            {
                this.scaleX.Select();
            }
        }
		base.transform.parent.transform.SetSiblingIndex(1);
	}

	private void FixedUpdate()
	{
        //if (this.positionX.isFocused || this.positionY.isFocused || this.positionZ.isFocused || this.rotationX.isFocused || this.rotationY.isFocused || this.rotationZ.isFocused || this.scaleX.isFocused || this.scaleY.isFocused || this.scaleZ.isFocused)
        //{
        //    this.traObj.transform.position = this.transformObj.transform.position;
        //    this.traObj.transform.localEulerAngles = this.transformObj.transform.localEulerAngles;
        //    TransformUI.isTrans = true;
        //}
        //else
        //{
        //    TransformUI.isTrans = false;
        //}
        if (this.transformObj != null)
        {
            this.SetOBJ(this.transformObj, this.uiName);
        }
	}

	public void SetOBJ(GameObject go, string _name)
	{
		this.uiName = _name;
		this.transformObj = go;
		if (this.transformObj == null)
		{
			this.transformObj = new GameObject();
			this.positionX.text = "0";
			this.positionY.text = "0";
			this.positionZ.text = "0";
			this.rotationX.text = "0";
			this.rotationY.text = "0";
			this.rotationZ.text = "0";
			this.scaleX.text = "1";
			this.scaleY.text = "1";
			this.scaleZ.text = "1";
		}
		else
		{
			this.positionX.text = this.transformObj.transform.position.x.ToString();
			this.positionY.text = this.transformObj.transform.position.y.ToString();
			this.positionZ.text = this.transformObj.transform.position.z.ToString();
			if (_name != "SoundUI")
			{
				if (go.name == "初始点")
				{
					if (this.transformObj.transform.localEulerAngles.x >= 270f && this.transformObj.transform.localEulerAngles.x <= 270.03f)
					{
						this.rotationX.text = "270";
					}
					else
					{
						this.rotationX.text = this.transformObj.transform.localEulerAngles.x.ToString();
					}
				}
				this.rotationX.text = this.transformObj.transform.localEulerAngles.x.ToString();
				this.rotationY.text = this.transformObj.transform.localEulerAngles.y.ToString();
				this.rotationZ.text = this.transformObj.transform.localEulerAngles.z.ToString();
				this.rotationX.interactable = true;
				this.rotationY.interactable = true;
				this.rotationZ.interactable = true;
			}
			else
			{
				this.rotationX.text = "0";
				this.rotationY.text = "0";
				this.rotationZ.text = "0";
				this.rotationX.interactable = false;
				this.rotationY.interactable = false;
				this.rotationZ.interactable = false;
			}
			if (_name != "SoundUI" && _name != "MainLightUI")
			{
				this.scaleX.text = this.transformObj.transform.localScale.x.ToString();
				this.scaleY.text = this.transformObj.transform.localScale.y.ToString();
				this.scaleZ.text = this.transformObj.transform.localScale.z.ToString();
				this.scaleX.interactable = true;
				this.scaleY.interactable = true;
				this.scaleZ.interactable = true;
			}
			else
			{
				this.scaleX.text = "1";
				this.scaleY.text = "1";
				this.scaleZ.text = "1";
				this.scaleX.interactable = false;
				this.scaleY.interactable = false;
				this.scaleZ.interactable = false;
			}
		}
	}
}
