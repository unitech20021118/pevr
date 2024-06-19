using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Data._data.action;
using TheTools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Action.UI
{
    public class ShootingModeUI:ActionUI
    {
        private ShootingMode _shootingMode;
        private ShootingModeInforma _shootingModeInforma;
        private Button _btnSelectEvent;
        private Text _txtSelectEvent;
        private Button _btnSelectGun;
        private GameObject selectDirectoryGameObject;
        private GameObject selectDirectoryItemPrefab;
        public GameObject SelectDirectoryGameObject
        {
            get
            {
                if (selectDirectoryGameObject == null)
                {
                    selectDirectoryGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/PPTSelectDirectory"));
                    selectDirectoryGameObject.transform.SetParent(Manager.Instace.transform);
                    selectDirectoryGameObject.transform.localPosition = new Vector3(215f, 70f, 0f);

                    selectDirectoryItemPrefab = Resources.Load<GameObject>("Prefabs/PPTSelectItem");

                }
                return selectDirectoryGameObject;
            }
        }
        void Awake()
        {
            _btnSelectEvent = gameObject.transform.FindChild("ChooseEvent").GetComponent<Button>();
            _btnSelectEvent.onClick.AddListener(LoadShootingEvent);
            _txtSelectEvent = gameObject.transform.FindChild("ChooseEvent/Text").GetComponent<Text>();
            _txtSelectEvent.text = "选择响应事件";
            _btnSelectGun = gameObject.transform.FindChild("ButtonSelectGuns").GetComponent<Button>();
            _btnSelectGun.onClick.AddListener(SelectGuns);
        }


        public override Action<Main> CreateAction()
        {
            action = new ShootingMode();
            action.isOnce = true;
            _shootingMode = (ShootingMode)action;
            _shootingModeInforma = new ShootingModeInforma(true);
            actionInforma = _shootingModeInforma;
            GetStateInfo().actionList.Add(actionInforma);
            actionInforma.name = "ShootingMode";

            return base.CreateAction();
        }

        private void LoadShootingEvent()
        {
            Manager.Instace.ChooseEventPanel.SetActive(true);
            Manager.Instace.ChooseEventPanel.GetComponent<CurrentEditorActon>().SetEditorAction(gameObject);
        }

        public override Action<Main> LoadAction(ActionInforma actionInforma)
        {
            _shootingModeInforma = (ShootingModeInforma)actionInforma;
            this.actionInforma = actionInforma;
            action = new ShootingMode();
            action.isOnce = true;
            _shootingMode = (ShootingMode)action;
            _shootingMode.duringTime = _shootingModeInforma.durtime;
            _shootingMode.GunName = _shootingModeInforma.GunName;
            _btnSelectGun.GetComponentInChildren<Text>().text = _shootingModeInforma.GunName;
            foreach (Events e in Manager.Instace.eventlist)
            {
                if (e.name == _shootingModeInforma.eventName)
                {
                    _shootingMode.even = e;
                    _txtSelectEvent.text = e.name;
                }
            }
            return base.LoadAction(actionInforma);
        }
        /// <summary>
        /// 选择枪械
        /// </summary>
        public void SelectGuns()
        {
            GameObject GunItem;
            SelectDirectoryGameObject.SetActive(true);
            string[] paths = Directory.GetFiles(Tools.Instance.GetAssteBundlesPath() + "/Gun");
            Tools.Instance.DeleteAllChild(selectDirectoryGameObject.transform.Find("Viewport/Content"));
            foreach (string path in paths)
            {
                GunItem = Instantiate(selectDirectoryItemPrefab);
                GunItem.transform.SetParent(selectDirectoryGameObject.transform.Find("Viewport/Content"));
                GunItem.GetComponentInChildren<Text>().text = Tools.Instance.GetFolderNameByFolderPath(path,true);
                string folName = Tools.Instance.GetFolderNameByFolderPath(path,true);
                GunItem.GetComponent<Button>().onClick.AddListener(delegate
                {
                    _btnSelectGun.GetComponentInChildren<Text>().text = folName;
                    _shootingMode.GunName = folName;
                    _shootingModeInforma.GunName = folName;
                    selectDirectoryGameObject.SetActive(false);
                });
            }
        }
    }
}
