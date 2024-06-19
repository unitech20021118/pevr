using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

//Edit by 潘晓峰
namespace Assets.Scripts.Undo.Commands
{
    public class UndoRedoManager : MonoBehaviour
    {
        public List<UndoRedoCommand> Commands = new List<UndoRedoCommand>();

        public static UndoRedoManager Instance = null;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            AddCommand(new UndoRedoCommand());
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                UnDoCommand();
        }

        public void AddCommand(UndoRedoCommand command)
        {
            Commands.Add(command);
            command.RedoCommand();
            //  Debug.Log("执行命令：" + command.NameOfCommand);
        }

        public void UnDoCommand()
        {
            if (Commands.Count > 1)
            {
                UndoRedoCommand command = Commands[Commands.Count - 1];
                Commands.Remove(command);
                command.UndoCommand();
                //  Debug.Log("撤销命令:" + command.NameOfCommand);
            }
        }

        public IEnumerator CreateGameObjByUndo(CreateUndoObjInfo undoObjInfo)
        {

            var objInfoTag = undoObjInfo.Tag;
            var modelpath = undoObjInfo.ModelPath;
            var imgpath = undoObjInfo.ImagePath;
            var objInfoName = undoObjInfo.Name;

            var gameobjectList = global::Manager.Instace.gameobjectList;
            var assetList = Manager.Instace.GetassetList();
            var ufbxi = Manager.Instace.ufbxi;
            var dictFromObjectToInforma = Manager.Instace.dictFromObjectToInforma;
            var listTree = Manager.Instace.allDataInformation.listTree;


            while (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    yield break;
                }
                yield return null;
            }
            if (objInfoTag.Equals("sky"))
            {
                Material skystyle;
                skystyle = Resources.Load<Material>(modelpath);
                RenderSettings.skybox = skystyle;
            }
            else
            {
                if (!gameobjectList.ContainsKey(objInfoName))
                {
                    var s = string.Empty;
                    string[] sarr = modelpath.Split('/');
                    for (var i = 0; i < sarr.Length - 1; i++)
                    {
                        s += sarr[i] + "/";
                    }
                    var houzhui = sarr[sarr.Length - 1].Split('.')[1];
                    var assetsname = s + sarr[sarr.Length - 1].Split('.')[0];
                    if (houzhui == "3dpro")
                    {
                        var assets = AssetBundle.LoadFromFile(modelpath);
                        print(assets.name);
                        string[] ssr = assets.name.Split('.');
                        print(assets.LoadAsset<GameObject>(ssr[0]));
                        var obj = assets.LoadAsset<GameObject>(ssr[0]); //资源加载到内存
                        assetList.Add(assets);
                        obj.name = objInfoName;
                        gameobjectList.Add(objInfoName, obj); //对加载的资源进行管理
                    }
                    else if (houzhui == "FBX")
                    {

                        var ssname = sarr[sarr.Length - 1].Split('.')[0];
                        ufbxi.setting.paths.SetModelPath(s, ssname);
                        print(modelpath);
                        yield return ufbxi.Load();
                        var obj = Instantiate<GameObject>(ufbxi.GetObject());
                        Destroy(ufbxi.GetObject());
                        obj.name = objInfoName;
                        obj.transform.localScale = new Vector3(100, 100, 100);
                        gameobjectList.Add(objInfoName, obj);
                    }
                }

                GameObject temp;
                ObjectInfo objectData;
                var gCreateObject = GetComponent<G_CreateObject>();
                if (objInfoTag.Equals("scene") || objInfoTag.Equals("terrain"))
                {
                    temp = gCreateObject.CreateGameObject(gameobjectList[objInfoName], false, objInfoName, imgpath, true);
                }
                else
                {
                    temp = GetComponent<G_CreateObject>().CreateGameObject(gameobjectList[objInfoName], true, objInfoName, imgpath, true);
                }
                //修改undo后物体的位置
                temp.transform.position = undoObjInfo.Position;
                temp.transform.rotation = undoObjInfo.Rotation;
                temp.transform.localScale = undoObjInfo.LocalScale;
                objectData = new ObjectInfo(modelpath, imgpath, temp.transform, temp.name, false);

                dictFromObjectToInforma.Add(temp, objectData);
                var parent = listTree.Root;
                listTree.AddLeave(parent, objectData);

                //todo 添加状态
               // Manager.Instace.AddStateInfoByUndo(undoObjInfo.Data,temp);

                //添加Undo组件
                var objInfo = new CreateUndoObjInfo()
                {
                    Tag = objInfoTag,
                    ModelPath = modelpath,
                    ImagePath = imgpath,
                    Name = objInfoName,
                };
                if (temp.GetComponent<CreateObjUndoComponent>() == null)
                {
                    var undoComponent = temp.AddComponent<CreateObjUndoComponent>();
                    undoComponent.Init(objInfo);
                    Debug.Log("CreteGamObject ：" + temp);
                }
            }
        }
    }
}
