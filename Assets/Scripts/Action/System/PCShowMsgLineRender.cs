using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Action.System
{
    public class PCShowMsgLineRender:MonoBehaviour
    {
        public GameObject ModelPos;
        public GameObject ImagePos;
        private LineRenderer _lineRenderer;
        private float _pointDistance = 1.3f;

        public Material mat;
        //void Awake()
        //{
        //    _lineRenderer = gameObject.GetComponent<LineRenderer>();
        //}

        void Start()
        {
            mat = Resources.Load<Material>("UIPrefab/LineRenderMaterial");
        }

        void OnPostRender()
        {
            if (ModelPos != null && ImagePos != null)
            {
                GL.PushMatrix();
                mat.SetPass(0);
                GL.LoadPixelMatrix();
                GL.Color(Color.yellow);
                GL.Begin(GL.LINES);
                var startPos = Camera.main.WorldToScreenPoint(ModelPos.transform.position);
                var endPos = ImagePos.GetComponent<RectTransform>().anchoredPosition;
                Debug.Log("EndPos : " + endPos);
                GL.Vertex3(endPos.x, endPos.y, 0);
                GL.End();
                GL.PopMatrix();
            }
            Debug.Log("PostRender");
            Debug.Log("model and imag : "+ModelPos + "  "+ImagePos);
        }

        //public void DrawLine()
        //{
        //    var startPos = ModelPos.transform.position;
        //    var endPos = ImagePos.transform.position;
        //    GameObject newObj = new GameObject("secondPos");
        //    newObj.transform.SetParent(ImagePos.transform, false);

        //    var imageLocalPos = ImagePos.transform.localPosition;

        //    if (JudgeDirection())
        //    {
        //        newObj.transform.localPosition = new Vector3(-imageLocalPos.x*_pointDistance, 0, 4);
        //    }
        //    else
        //    {
        //        newObj.transform.localPosition = new Vector3(imageLocalPos.x* _pointDistance, 0, 4);
        //    }

        //    //设置点的位置
        //    var secondPos = newObj.transform.position;
        //    _lineRenderer.SetPosition(0, startPos);
        //    _lineRenderer.SetPosition(1, secondPos);
        //    _lineRenderer.SetPosition(2, endPos);
        //}

        //判断文本框在模型的左边还是右边
        private bool JudgeDirection()
        {
            var modelscreenPos = Camera.main.WorldToScreenPoint(ModelPos.transform.position);
            var imgScreenPos = Camera.main.WorldToScreenPoint(ImagePos.transform.position);
            return modelscreenPos.x > imgScreenPos.x;
        }

        public void HideLine()
        {
            gameObject.SetActive(false);
        }

        public void SetLineDistence(float lineDistence)
        {
            _pointDistance = lineDistence;
        }
    }
}
