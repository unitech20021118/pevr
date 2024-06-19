using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 鼠标长按事件（2s）
/// </summary>
public class MouseDownLong : Action<Main>
{
    /// <summary>
    /// 计时器
    /// </summary>
    private float time = 0;
    private bool b;
    /// <summary>
    /// 进度条
    /// </summary>
    private GameObject progressGameObject;
    private Ray ray;
    private RaycastHit hit;
    public override void DoAction(Main m)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Transform a = even.target.gameObject.transform;
                if (hit.collider.gameObject.transform == a)
                {
                    b = true;
                }
            }
        }
        if (b)
        {
            if (Input.GetMouseButton(0))
            {

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    //Transform a=Manager.Instace.gonggong.transform.GetChild(0);
                    Transform a = even.target.gameObject.transform;
                    if (hit.collider.gameObject.transform == a)
                    {
                        time += Time.deltaTime;
                        GetProgress();
                        progressGameObject.transform.position = Input.mousePosition + new Vector3(0, -30, 0);
                        progressGameObject.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = time / 2f;
                        if (time >= 2)
                        {
                            InitProgress();
                            even.Do();
                        }
                    }
                    else
                    {
                        InitProgress();
                    }
                }
                else
                {
                    InitProgress();
                }
            }
            else
            {
                InitProgress();
            }
        }
    }
    /// <summary>
    /// 获取进度条
    /// </summary>
    private void GetProgress()
    {
        if (progressGameObject == null)
        {
            progressGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Progress"));
            progressGameObject.transform.SetParent(GameObject.Find("Canvas").transform);
            //progressGameObject.transform.localPosition = new Vector3(0, -33, 0);
        }
    }
    /// <summary>
    /// 清空进度条的记录并重置时间
    /// </summary>
    private void InitProgress()
    {
        if (progressGameObject != null)
        {
            Object.Destroy(progressGameObject);
            progressGameObject = null;
        }
        b = false;
        time = 0;
    }


}
