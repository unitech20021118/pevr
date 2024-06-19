using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Login
{
    public class MassageBoxPanelComponent:MonoBehaviour
    {
        private Button _btnConform;
        private Text _text;
       public void Init()
        {
           
            _btnConform = gameObject.transform.FindChild("BtnConform").GetComponent<Button>();
            _btnConform.onClick.AddListener(OnConform);
            _text = gameObject.transform.FindChild("Text").GetComponent<Text>();
        }

        private void OnConform()
        {
            HidePanel();
        }

        public void ShowMassage(string msgText)
        {
            ShowPanel();
            _text.text = msgText;
        }

        public void ShowMassage(string msgText,System.Action conformAction)
        {
            ShowPanel();
            _btnConform.onClick.AddListener(conformAction.Invoke);
            _text.text = msgText;
        }

        public void HidePanel()
        {
            gameObject.SetActive(false);
        }

        public void ShowPanel()
        {
            Debug.Log(_btnConform);
            _btnConform.onClick.RemoveAllListeners();
            _btnConform.onClick.AddListener(OnConform);
            gameObject.SetActive(true);
        }
    }
}
