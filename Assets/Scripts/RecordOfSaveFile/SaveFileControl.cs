using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace RecordOfSaveFile
{
	public class SaveFileControl : MonoBehaviour
	{

		public static SaveFileControl Instance;

		/// <summary>
		/// 要打开的存档的地址
		/// </summary>
		public Text ToOpenPathText;
		/// <summary>
		/// 浏览
		/// </summary>
		public Button BrowseButton;
		/// <summary>
		/// 打开存档
		/// </summary>
		public Button OpenFileButton;

		public Transform ContentTransform;

		private List<Record> records;
		/// <summary>
		/// 是否是不保存退出
		/// </summary>
		private bool quitWithoutSaving;
		/// <summary>
		/// 是否已经保存
		/// </summary>
		private bool saved;
		/// <summary>
		/// 确认保存的提示界面
		/// </summary>
		public GameObject SaveTipGameObject;
		/// <summary>
		/// 打开自动保存存档按钮的提示
		/// </summary>
		public GameObject TimeSaveTipObj;


		void Awake()
        {
			Instance = this;
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void OpenSaveFileList()
        {
			TheTools.Tools.Instance.DeleteAllChild(ContentTransform);

			transform.GetChild(0).gameObject.SetActive(true);

			records = RecordSaveFile.GetRecordSF();
            if (records != null && records.Count > 0)
            {
				for (int i = 0; i < records.Count; i++)
				{
					GameObject obj = Instantiate(Resources.Load<GameObject>("RecordOfSaveFile/Item_SaveFile"));
					obj.transform.SetParent(ContentTransform);
					obj.transform.localScale = Vector3.one;
					obj.GetComponent<SaveFileItem>().Init(records[i]);
				}
			}
		}


		public void Open()
        {
            if (!string.IsNullOrEmpty(ToOpenPathText.text))
            {
				SceneCtrl.OpenStatePath = ToOpenPathText.text;
				Manager.Instace.OpenNewScene();

				if (records != null && records.Count > 0)
                {
					RecordSaveFile.record = records.FirstOrDefault(a => a.PathOfRecord == ToOpenPathText.text);
                }
			}
        }

		public void Browse()
        {
			ToOpenPathText.text = IOHelper.OpenFileDlgToLoad();
		}


		void OnApplicationQuit()
        {
			//Debug.LogError("quitWithoutSaving" + quitWithoutSaving);

			if (quitWithoutSaving)
            {
				return;
            }
            if (RecordSaveFile.SaveJudgementPrompt())
            {
				//弹出保存提示
				SaveTipGameObject.SetActive(true);
				Button btn;
				btn = SaveTipGameObject.transform.Find("BtnSave").GetComponent<Button>();
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(SaveAndQuit);
				btn = SaveTipGameObject.transform.Find("BtnNSave").GetComponent<Button>();
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(QuitWithoutSave);
				btn = SaveTipGameObject.transform.Find("BtnCancel").GetComponent<Button>();
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(Cancel);
				quitWithoutSaving = false;
				Application.CancelQuit();
            }
            
        }
		/// <summary>
		/// 保存并退出
		/// </summary>
		public void SaveAndQuit()
        {
			Manager.Instace.SaveClick();
			Application.Quit();
		}
		public void QuitWithoutSave()
        {
			quitWithoutSaving = true;
			Application.Quit();
		}
		public void Cancel()
        {
			//关闭提示界面
			SaveTipGameObject.SetActive(false);
		}

		public void Quit()
        {
			Application.Quit();
		}


		/// <summary>
		/// 当要打开新场景时
		/// </summary>
		public void OnOpenNewSceneButtonClick()
        {
			if (RecordSaveFile.SaveJudgementPrompt())
			{
				//弹出保存提示
				SaveTipGameObject.SetActive(true);
				Button btn;
				btn = SaveTipGameObject.transform.Find("BtnSave").GetComponent<Button>();
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(SaveAndOpenNewScene);
				btn = SaveTipGameObject.transform.Find("BtnNSave").GetComponent<Button>();
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(OpenNewSceneWithoutSave);
				btn = SaveTipGameObject.transform.Find("BtnCancel").GetComponent<Button>();
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(Cancel);
				Application.CancelQuit();
            }
            else
            {
				Manager.Instace.OpenNewScene();
			}
		}
		/// <summary>
		/// 保存并打开新场景
		/// </summary>
		public void SaveAndOpenNewScene()
        {
			Manager.Instace.SaveClick();
			Manager.Instace.OpenNewScene();
		}
		/// <summary>
		/// 不保存打开新场景
		/// </summary>
		public void OpenNewSceneWithoutSave()
        {
			Manager.Instace.OpenNewScene();
		}

		/// <summary>
		/// 打开定时保存的文档
		/// </summary>
		public void OpenTimeSaveButtonClick()
        {
			ToOpenPathText.text = Application.dataPath + "/SaveFile/automaticSF.pevrsf";
		}

		public void TimeSaveButtonTip(bool open)
        {
			TimeSaveTipObj.SetActive(open);
		}
	}

}
