using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Action.System
{
    public class PCShowMsgHelpCompoennt:MonoBehaviour
    {
        private Button _btnClose;
        private CameraDrawLine _lineRenderCotrol;
        void Awake()
        {
            _btnClose = gameObject.transform.FindChild("Button").GetComponent<Button>();
            _btnClose.onClick.RemoveAllListeners();
            _btnClose.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            gameObject.SetActive(false);
            if(_lineRenderCotrol != null)
            _lineRenderCotrol.SetDrawLine(false);
        }

        public void SetLineRender(CameraDrawLine component)
        {
            _lineRenderCotrol = component;
        }
    }
}
