using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RecordOfSaveFile
{
    public class SaveFileItem : MonoBehaviour
    {

        private Record record;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(Record record)
        {
            this.record = record;
            transform.Find("TextName").GetComponent<Text>().text = record.PathOfRecord;
        }

        public void OnButtonClick()
        {
            SaveFileControl.Instance.ToOpenPathText.text = record.PathOfRecord;
        }
    }

}
