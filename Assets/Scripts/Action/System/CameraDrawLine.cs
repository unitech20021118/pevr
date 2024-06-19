using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Action.System
{
    public class CameraDrawLine:MonoBehaviour
    {
        public GameObject ModelPos;
        public GameObject ImagePos;
        private bool _isDrawLine;
        private Material _mat;
        private float _lineDistance = 1.2f;
        private Camera _camera;
        void Start()
        {
            _mat = Resources.Load<Material>("UIPrefab/LineRenderMaterial");
            _camera = this.GetComponent<Camera>();
        }

        void OnPostRender()
        {
            if (_isDrawLine)
            {
                GL.PushMatrix();
                _mat.SetPass(0);
                GL.LoadPixelMatrix();
                var startPos = GetStartPos(ModelPos.transform.position);
                var imageWidth = ImagePos.GetComponent<RectTransform>().rect.width*2;
                var imageHeight = ImagePos.GetComponent<RectTransform>().rect.height/2*3;
                var endPos = GetEndPos(ImagePos.transform.position,imageWidth,imageHeight);
                var secondPos = GetSecondPos(startPos, endPos, imageWidth, _lineDistance);
                DrawLineByGl(startPos, secondPos);
                DrawLineByGl(secondPos, endPos);
                GL.PopMatrix();
            }
        }

        public void SetDrawLine(bool active)
        {
            _isDrawLine = active;
        }

        private Vector2 GetStartPos(Vector3 modelPos)
        {
            var pos = _camera.WorldToScreenPoint(modelPos);
            return new Vector2(pos.x, pos.y);
        }

        private Vector2 GetEndPos(Vector3 endPos,float width,float height)
        {
            var pos = endPos;
            return new Vector2(pos.x - width/2, pos.y-height/2);
            //return  new Vector2(pos.x, pos.y);
        }

        private Vector2 GetSecondPos(Vector2 startPos, Vector2 endPos, float imageWidth ,float lineDistance)
        {
            Vector2 secondPos;
            if (JudgeDirection(startPos, endPos))
            {
                secondPos = new Vector2(endPos.x + imageWidth * lineDistance, endPos.y);
            }
            else
            {
                secondPos = new Vector2(endPos.x - imageWidth * lineDistance, endPos.y);
            }
            return secondPos;
        }

        private bool JudgeDirection(Vector2 startPos, Vector2 endPos)
        {
            return startPos.x > endPos.x;
        }

        //判断文本框在模型的左边还是右边
        public void DrawLineByGl(Vector2 startPos, Vector2 endPos)
        {
            GL.Begin(GL.LINES);
            GL.Color(_mat.color);
            GL.Vertex3(startPos.x, startPos.y, 0);
            GL.Vertex3(endPos.x, endPos.y, 0);
            GL.End();
        }

        public void SetDistance(float lineDistance)
        {
            _lineDistance = lineDistance;
        }
    }
}
