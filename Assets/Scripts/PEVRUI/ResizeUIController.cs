using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeUIController : MonoBehaviour
{

    /// <summary>
    /// 左上角
    /// </summary>
    public Image LeftUp;
    /// <summary>
    /// 左下角
    /// </summary>
    public Image LeftDown;
    /// <summary>
    /// 右上角
    /// </summary>
    public Image RightUp;
    /// <summary>
    /// 右下角
    /// </summary>
    public Image RightDown;
    /// <summary>
    /// 距离边缘的距离小于10个像素
    /// </summary>
    public float Deviation = 10f;

    private Texture2D udCursorTexture2D;

    private Texture2D lrCursorTexture2D;
    private Texture2D luCursorTexture2D;
    private Texture2D ruCursorTexture2D;

    private bool cursorChanged;
    private bool pointerDown;
    private bool pointerExit;

    private CursorMode cursorMode = CursorMode.Auto;


    /// <summary>
    /// canvas的宽
    /// </summary>
    private float Width = 800f;
    /// <summary>
    /// canvas的高
    /// </summary>
    private float Height = 450f;
    /// <summary>
    /// 要控制的ui的rect
    /// </summary>
    public RectTransform TargetRectTransform;


    private float tpx;
    private float tpy;
    private float tsx;
    private float tsy;

    /// <summary>
    /// 鼠标在屏幕比例下的坐标
    /// </summary>
    private Vector2 mousePositionVector2;
    /// <summary>
    /// 屏幕缩放比例
    /// </summary>
    private float proportion;

    private float offsetX;

    private float offsetY;

    // Use this for initialization
    void Start()
    {
        luCursorTexture2D = Resources.Load<Texture2D>("NewUIPic/tubiao/LU");
        lrCursorTexture2D = Resources.Load<Texture2D>("NewUIPic/tubiao/LR");
        udCursorTexture2D = Resources.Load<Texture2D>("NewUIPic/tubiao/UD");
        ruCursorTexture2D = Resources.Load<Texture2D>("NewUIPic/tubiao/RU");
        //luCursorTexture2D.Resize(30, 30);
        //proportion = Width / Screen.width;
        if (transform.parent != null)
        {
            TargetRectTransform = transform.parent.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 ve;
        //    ve = new Vector2(Input.mousePosition.x * 800 / Screen.width, Input.mousePosition.y * 800 / Screen.width);
        //    Debug.LogError(ve);
        //}
        if (cursorChanged && !pointerDown && pointerExit)
        {
            Cursor.SetCursor(null,Vector2.zero, cursorMode);
            cursorChanged = false;
        }
        if (Manager.Instace._Playing)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 鼠标进入角标位置时
    /// </summary>
    public void CornerMarkerPointEnter(string str)
    {
        //修改鼠标样式
        switch (str)
        {
            case "LU":
                Cursor.SetCursor(luCursorTexture2D, new Vector2(50f,50f), cursorMode);
                break;
            case "LD":
                Cursor.SetCursor(ruCursorTexture2D, new Vector2(50f, 50f), cursorMode);
                break;
            case "RU":
                Cursor.SetCursor(ruCursorTexture2D, new Vector2(50f, 50f), cursorMode);
                break;
            case "RD":
                Cursor.SetCursor(luCursorTexture2D, new Vector2(50f, 50f), cursorMode);
                break;
            default:
                break;
        }
        pointerExit = false;
        cursorChanged = true;
    }

    /// <summary>
    /// 鼠标按住角标时
    /// </summary>
    public void CornerMarkerPointerDown(string str)
    {
        proportion = Width / Screen.width;
        tpx = TargetRectTransform.anchoredPosition.x;
        tpy = TargetRectTransform.anchoredPosition.y;
        tsx = TargetRectTransform.rect.width;
        tsy = TargetRectTransform.rect.height;
        //修改锚点位置
        switch (str)
        {
            case "LU":
                //修改中心点位置从左下角到右下角
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, tpx + tsx, tsx);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, tpy, tsy);

                TargetRectTransform.pivot = new Vector2(1, 0);
                break;
            case "LD":
                //修改中心点位置从左下角到右上角

                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, tpx + tsx, tsx);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, tpy + tsy, tsy);
                TargetRectTransform.pivot = new Vector2(1, 1);
                break;
            case "RU":
                //不用修改锚点位置
                //TargetRectTransform.pivot = new Vector2(0, 0);
                //TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, TargetRectTransform.anchoredPosition.x, tsx);
                //TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Height + tpy - tsy, tsy);

                break;
            case "RD":
                //修改中心点位置从左下角到左上角

                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, tpx, tsx);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, tpy + tsy, tsy);
                TargetRectTransform.pivot = new Vector2(0, 1);
                break;
            default:
                break;
        }
        pointerDown = true;
    }

    /// <summary>
    /// 鼠标拖动角标时
    /// </summary>
    public void CornerMarkerDrag(string str)
    {
        mousePositionVector2 = Input.mousePosition * proportion;
        //修改要修改的rect的大小等属性
        switch (str)
        {
            case "LU":
                tsx = TargetRectTransform.anchoredPosition.x - mousePositionVector2.x;
                tsy = mousePositionVector2.y - TargetRectTransform.anchoredPosition.y;
                break;
            case "LD":
                tsx = TargetRectTransform.anchoredPosition.x - mousePositionVector2.x;
                tsy = TargetRectTransform.anchoredPosition.y - mousePositionVector2.y;
                break;
            case "RU":
                tsx = mousePositionVector2.x - TargetRectTransform.anchoredPosition.x;
                tsy = mousePositionVector2.y - TargetRectTransform.anchoredPosition.y;
                break;
            case "RD":
                tsx = mousePositionVector2.x - TargetRectTransform.anchoredPosition.x;
                tsy = TargetRectTransform.anchoredPosition.y - mousePositionVector2.y;
                break;
            default:

                break;

        }
        TargetRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,tsx);
        TargetRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,tsy);

    }
    /// <summary>
    /// 鼠标松开角标时
    /// </summary>
    public void CornerMarkerPointerUp(string str)
    {
        tpx = TargetRectTransform.anchoredPosition.x;
        tpy = TargetRectTransform.anchoredPosition.y;
        tsx = TargetRectTransform.sizeDelta.x;
        tsy = TargetRectTransform.sizeDelta.y;

        //Debug.LogError(tpx + " " + tpy + " " + tsx + " " + tsy);
        //将锚点复原
        switch (str)
        {
            case "LU":
                //修改中心点位置从右下角到左下角
                TargetRectTransform.pivot = new Vector2(0, 0);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, tpx - tsx, tsx);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, tpy, tsy);

                break;
            case "LD":
                //修改中心点位置从右上角到左下角
                TargetRectTransform.pivot = new Vector2(0, 0);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, tpx - tsx, tsx);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, tpy - tsy, tsy);
                break;
            case "RU":
                break;
            case "RD":
                //修改中心点位置从左上角到左下角
                TargetRectTransform.pivot = new Vector2(0, 0);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, tpx, tsx);
                TargetRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, tpy - tsy, tsy);
                break;
            default:
                break;
        }
        pointerDown = false;
    }
    /// <summary>
    /// 鼠标离开角标位置时
    /// </summary>
    public void CornerMarkerPointExit(string str)
    {
        //将鼠标样式复原
        //switch (str)
        //{
        //    case "LU":
        //        //Cursor.SetCursor(null, Vector2.zero, cursorMode);
        //        break;
        //    case "LD":
        //        //Cursor.SetCursor(null, Vector2.zero, cursorMode);
        //        break;
        //    case "RU":
        //        //Cursor.SetCursor(null, Vector2.zero, cursorMode);
        //        break;
        //    case "RD":
        //        //Cursor.SetCursor(null, Vector2.zero, cursorMode);
        //        break;
        //    default:
        //        break;
        //}
        pointerExit = true;
    }



    public void GetOffset()
    {
        offsetX = Input.mousePosition.x - transform.parent.position.x;
        offsetY = Input.mousePosition.y - transform.parent.position.y;
    }

    public void FollowMouse()
    {
        transform.parent.position = new Vector3(Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY, transform.parent.position.z);
    }
}
