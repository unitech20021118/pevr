using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G_TranformUI : MonoBehaviour {

    public static G_TranformUI Instance;
    private InputField positionX;
    private InputField positionY;
    private InputField positionZ;
    private Button positionCopy;
    private Button positionPaste;
    //private Vector3 positionVector3;
    private CopyTransform positionCopyTransform;
    //private bool positionVector3Value;

    private InputField rotationX;
    private InputField rotationY;
    private InputField rotationZ;
    private Button rotationCopy;
    private Button rotationPaste;
    //private Vector3 rotationVector3;
    private CopyTransform rotationCopyTransform;
    //private bool rotationVector3Value;

    private InputField scaleX;
    private InputField scaleY;
    private InputField scaleZ;
    private Button scaleCopy;
    private Button scalePaste;
    //private Vector3 scaleVector3;
    private CopyTransform scaleCopyTransform;
    //private bool scaleVector3Value;

    public GameObject moveTarget;
    private float value;

    void Awake()
    {
        Instance = gameObject.GetComponent<G_TranformUI>();
        positionX = transform.Find("Position/XText/InputField").GetComponent<InputField>();
        positionY = transform.Find("Position/YText/InputField").GetComponent<InputField>();
        positionZ = transform.Find("Position/ZText/InputField").GetComponent<InputField>();
        positionCopy = transform.FindChild("Position/BtnCopy").GetComponent<Button>();
        positionCopy.onClick.AddListener(delegate { TransCopy(positionCopy); });
        positionPaste = transform.FindChild("Position/BtnPaste").GetComponent<Button>();
        positionPaste.onClick.AddListener(delegate { TransCopy(positionPaste); });

        rotationX = transform.Find("Rotation/XText/InputField").GetComponent<InputField>();
        rotationY = transform.Find("Rotation/YText/InputField").GetComponent<InputField>();
        rotationZ = transform.Find("Rotation/ZText/InputField").GetComponent<InputField>();
        rotationCopy = transform.FindChild("Rotation/BtnCopy").GetComponent<Button>();
        rotationCopy.onClick.AddListener(delegate { TransCopy(rotationCopy); });
        rotationPaste = transform.FindChild("Rotation/BtnPaste").GetComponent<Button>();
        rotationPaste.onClick.AddListener(delegate { TransCopy(rotationPaste); });

        scaleX = transform.Find("Scale/XText/InputField").GetComponent<InputField>();
        scaleY = transform.Find("Scale/YText/InputField").GetComponent<InputField>();
        scaleZ = transform.Find("Scale/ZText/InputField").GetComponent<InputField>();
        scaleCopy = transform.FindChild("Scale/BtnCopy").GetComponent<Button>();
        scaleCopy.onClick.AddListener(delegate { TransCopy(scaleCopy); });
        scalePaste = transform.FindChild("Scale/BtnPaste").GetComponent<Button>();
        scalePaste.onClick.AddListener(delegate { TransCopy(scalePaste); });
    }

    void Start()
    {
        positionX.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (positionX.text == string.Empty)
                {
                    positionX.text = "0";
                    moveTarget.transform.position = new Vector3(0f, moveTarget.transform.position.y, moveTarget.transform.position.z);

                }
                else if (float.TryParse(positionX.text, out value))
                {
                    //Debug.Log(value);
                   moveTarget.transform.position = new Vector3(value, moveTarget.transform.position.y, moveTarget.transform.position.z);
                }
            }
        });

        positionY.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (positionY.text == string.Empty)
                {
                    positionY.text = "0";
                    moveTarget.transform.position = new Vector3(moveTarget.transform.position.x, NumericalConversion.GetTargetPositionYByHeight(moveTarget.transform,0f), moveTarget.transform.position.z);

                }
                else if (float.TryParse(positionY.text, out value))
                {
                    moveTarget.transform.position = new Vector3(moveTarget.transform.position.x, NumericalConversion.GetTargetPositionYByHeight(moveTarget.transform, value), moveTarget.transform.position.z);
                }
            }

        });

        positionZ.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (positionZ.text == string.Empty)
                {
                    positionZ.text = "0";
                    moveTarget.transform.position = new Vector3(moveTarget.transform.position.x, moveTarget.transform.position.y, 0);

                }
                else if (float.TryParse(positionZ.text, out value))
                {
                    moveTarget.transform.position = new Vector3(moveTarget.transform.position.x, moveTarget.transform.position.y, value);
                }
            }

        });

        rotationX.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (rotationX.text == string.Empty)
                {
                    rotationX.text = "0";
                    moveTarget.transform.localEulerAngles = new Vector3(0f, moveTarget.transform.localEulerAngles.y, moveTarget.transform.localEulerAngles.z);

                }
                else if (float.TryParse(rotationX.text, out value))
                {
                    moveTarget.transform.localEulerAngles = new Vector3(value, moveTarget.transform.localEulerAngles.y, moveTarget.transform.localEulerAngles.z);
                }
            }

        });

        rotationY.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (rotationY.text == string.Empty)
                {
                    rotationY.text = "0";
                    moveTarget.transform.localEulerAngles = new Vector3(moveTarget.transform.localEulerAngles.x, 0f, moveTarget.transform.localEulerAngles.z);

                }
                else if (float.TryParse(rotationY.text, out value))
                {
                    moveTarget.transform.localEulerAngles = new Vector3(moveTarget.transform.localEulerAngles.x, value, moveTarget.transform.localEulerAngles.z);
                }
            }

        });

        rotationZ.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (rotationZ.text == string.Empty)
                {
                    rotationZ.text = "0";
                    moveTarget.transform.localEulerAngles = new Vector3(moveTarget.transform.localEulerAngles.x, moveTarget.transform.localEulerAngles.y, 0);

                }
                else if (float.TryParse(rotationZ.text, out value))
                {
                    moveTarget.transform.localEulerAngles = new Vector3(moveTarget.transform.localEulerAngles.x, moveTarget.transform.localEulerAngles.y, value);
                }
            }

        });

        scaleX.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (scaleX.text == string.Empty)
                {
                    scaleX.text = "0";
                    moveTarget.transform.localScale = new Vector3(0f, moveTarget.transform.localScale.y, moveTarget.transform.localScale.z);

                }
                else if (float.TryParse(scaleX.text, out value))
                {
                    moveTarget.transform.localScale = new Vector3(value, moveTarget.transform.localScale.y, moveTarget.transform.localScale.z);
                }
            }

        });

        scaleY.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (scaleY.text == string.Empty)
                {
                    scaleY.text = "0";
                    moveTarget.transform.localScale = new Vector3(moveTarget.transform.localScale.x, 0f, moveTarget.transform.localScale.z);

                }
                else if (float.TryParse(scaleY.text, out value))
                {
                    moveTarget.transform.localScale = new Vector3(moveTarget.transform.localScale.x, value, moveTarget.transform.localScale.z);
                }
            }

        });

        scaleZ.onValueChanged.AddListener(delegate(string var)
        {
            if (moveTarget != null)
            {
                if (scaleZ.text == string.Empty)
                {
                    scaleY.text = "0";
                    moveTarget.transform.localScale = new Vector3(moveTarget.transform.localScale.x, moveTarget.transform.localScale.y, 0);

                }
                else if (float.TryParse(scaleZ.text, out value))
                {
                    moveTarget.transform.localScale = new Vector3(moveTarget.transform.localScale.x, moveTarget.transform.localScale.y, value);
                }
            }

        });
    }

    void Update()
    {
        //if (moveTarget != null)
        //{
        //    //SetObj(moveTarget);
        //}
        //else
        //{
            
        //    positionX.text = "0";
        //    positionY.text = "0";
        //    positionZ.text = "0";
        //    rotationX.text = "0";
        //    rotationY.text = "0";
        //    rotationZ.text = "0";
        //    scaleX.text = "1";
        //    scaleY.text = "1";
        //    scaleZ.text = "1";
        //}
    }

    /// <summary>
    /// 显示目标物体的PRS
    /// </summary>
    /// <param name="obj"></param>
    public void SetObj(GameObject obj)
    {
        moveTarget = obj;
        this.positionX.text = moveTarget.transform.position.x.ToString();
        this.positionY.text = NumericalConversion.GetHeightAboveGround(moveTarget.transform).ToString();// moveTarget.transform.position.y.ToString();
        this.positionZ.text = moveTarget.transform.position.z.ToString();
        this.rotationX.text = moveTarget.transform.localEulerAngles.x.ToString();
        this.rotationY.text = moveTarget.transform.localEulerAngles.y.ToString();
        this.rotationZ.text = moveTarget.transform.localEulerAngles.z.ToString();
        this.scaleX.text = moveTarget.transform.localScale.x.ToString();
        this.scaleY.text = moveTarget.transform.localScale.y.ToString();
        this.scaleZ.text = moveTarget.transform.localScale.z.ToString();
    }
    /// <summary>
    /// 复制粘贴trans的属性
    /// </summary>
    public void TransCopy(Button btn)
    {
        if (btn == positionCopy)
        {
            //positionVector3 = new Vector3(float.Parse(positionX.text),float.Parse(positionY.text),float.Parse(positionZ.text));
            positionCopyTransform = new CopyTransform(positionX.text,positionY.text,positionZ.text);
            //positionVector3Value = true;
            positionPaste.GetComponent<Image>().color = Color.white;
            positionPaste.GetComponentInChildren<Text>().color = Color.white;
        }
        else if (btn == rotationCopy)
        {
            //rotationVector3 = new Vector3(float.Parse(rotationX.text), float.Parse(rotationY.text), float.Parse(rotationZ.text));
            rotationCopyTransform = new CopyTransform(rotationX.text,rotationY.text,rotationZ.text);
            //rotationVector3Value = true;
            rotationPaste.GetComponent<Image>().color = Color.white;
            rotationPaste.GetComponentInChildren<Text>().color = Color.white;
        }
        else if (btn == scaleCopy)
        {
            //scaleVector3 = new Vector3(float.Parse(scaleX.text), float.Parse(scaleY.text), float.Parse(scaleZ.text));
            scaleCopyTransform = new CopyTransform(scaleX.text,scaleY.text,scaleZ.text);
            //scaleVector3Value = true;
            scalePaste.GetComponent<Image>().color = Color.white;
            scalePaste.GetComponentInChildren<Text>().color = Color.white;
        }
        else if (btn == positionPaste && positionCopyTransform != null)
        {
            //positionX.text = positionVector3.x.ToString();
            //positionY.text = positionVector3.y.ToString();
            //positionZ.text = positionVector3.z.ToString();
            positionX.text = positionCopyTransform.X;
            positionY.text = positionCopyTransform.Y;
            positionZ.text = positionCopyTransform.Z;
        }
        else if (btn == rotationPaste && rotationCopyTransform != null)
        {
            //rotationX.text = rotationVector3.x.ToString();
            //rotationY.text = rotationVector3.y.ToString();
            //rotationZ.text = rotationVector3.z.ToString();
            rotationX.text = rotationCopyTransform.X;
            rotationY.text = rotationCopyTransform.Y;
            rotationZ.text = rotationCopyTransform.Z;
        }
        else if (btn == scalePaste && scaleCopyTransform != null)
        {
            //scaleX.text = scaleVector3.x.ToString();
            //scaleY.text = scaleVector3.y.ToString();
            //scaleZ.text = scaleVector3.z.ToString();
            scaleX.text = scaleCopyTransform.X;
            scaleY.text = scaleCopyTransform.Y;
            scaleZ.text = scaleCopyTransform.Z;
        }
    }
}

class CopyTransform
{
    public string X;
    public string Y;
    public string Z;
    public CopyTransform() { }

    public CopyTransform(string x, string y, string z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
