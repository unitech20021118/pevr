using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Scripts.DownLoadOnLine;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UserData = Assets.Scripts.DownLoadOnLine.UserData;


public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    /// <summary>
    /// 服务器商店数据文本的地址
    /// </summary>
    private string serverShopDataPath;
    /// <summary>
    /// 服务器用户数据的地址
    /// </summary>
    private string serverUserDataPath;
    /// <summary>
    /// 图片下载地址前缀
    /// </summary>
    private string spritePath;
    /// <summary>
    /// 模型下载地址前缀
    /// </summary>
    private string modelPath;
    /// <summary>
    /// 商店数据链表
    /// </summary>
    private List<ModelInformation> ShopDataList;
    /// <summary>
    /// 用户数据
    /// </summary>
    //private Assets.Scripts.DownLoadOnLine.UserData userData;
    /// <summary>
    /// 预设
    /// </summary>
    public GameObject CommodityPrefab;
    /// <summary>
    /// 商店未下载的模型存放的页面
    /// </summary>
    public GameObject ShopGameObject;
    /// <summary>
    /// 已下载的模型存放页面
    /// </summary>
    public GameObject DownGameObject;
    /// <summary>
    /// 未下载的模型的存放位置的父物体
    /// </summary>
    public Transform ShopContentTransform;
    /// <summary>
    /// 已下载的存放位置的父物体
    /// </summary>
    public Transform DownContentedTransform;
    /// <summary>
    /// previewitem对象池存储位置
    /// </summary>
    public Transform PreviewItemPoolTransform;
    /// <summary>
    /// 正在下载中的对象的存放位置
    /// </summary>
    public Transform DownLoadingPreviewItemPoolTransform;
    /// <summary>
    /// 没有可供下载的模型的遮罩物体
    /// </summary>
    public GameObject NoModelImageGameObject;
    ///// <summary>
    ///// 当前打开的商店数据来源地址
    ///// </summary>
    //public string CurrentShopDataPath;
    /// <summary>
    /// 显示未下载的按钮
    /// </summary>
    //public Button ShowShopButton;
    /// <summary>
    /// 显示已下载的按钮
    /// </summary>
    //public Button ShowDownLoadButton;
    /// <summary>
    /// 显示的是否是未下载的
    /// </summary>
    private bool showShop = true;
    private string path;

    private SpritePool spritePool = new SpritePool();
    /// <summary>
    /// preview的对象池
    /// </summary>
    public PreviewItemPool previewItemPool = new PreviewItemPool();
    /// <summary>
    /// 正在下载的对象的对象池
    /// </summary>
    public DownLoadingPreviewItemPool DownLoadingPreviewItemPool = new DownLoadingPreviewItemPool();
    /// <summary>
    /// 并线处理下载的协成的列表
    /// </summary>
    private List<Coroutine> coroutines = new List<Coroutine>();

    private Coroutine preViewDownLoadCoroutine;

    private Dictionary<string, string> idNameDictionary = new Dictionary<string, string>();
    /// <summary>
    /// 用户已下载的模型的记录文本文件夹地址
    /// </summary>
    private string userDownDataFolderPath;

    public string SpritePath
    {
        get
        {
            return spritePath;
        }

        set
        {
            spritePath = value;
        }
    }

    public string ModelPath
    {
        get
        {
            return modelPath;
        }

        set
        {
            modelPath = value;
        }
    }



    //-----------------------分页部分-----------------------------------------------
    /// <summary>
    /// 商店总页数
    /// </summary>
    private int shopAllPages;
    /// <summary>
    /// 已下载的总页数
    /// </summary>
    private int downAllPages;
    /// <summary>
    /// 商店当前页
    /// </summary>
    private int shopCurrentPage;
    /// <summary>
    /// 已下载的当前页
    /// </summary>
    private int downCurrentPage;
    /// <summary>
    /// 商店修改页面输入框
    /// </summary>
    public InputField ShopPageInputField;
    /// <summary>
    /// 已下载的修改页面输入框
    /// </summary>
    public InputField DownPageInputField;
    /// <summary>
    /// 商店显示总页码的文本
    /// </summary>
    public Text ShopAllPagesText;
    /// <summary>
    /// 已下载的显示总页码的文本
    /// </summary>
    public Text DownAllPagesText;
    /// <summary>
    /// 当前类别未下载的商品数据链表
    /// </summary>
    private List<ModelInformation> notDownShopDataList = new List<ModelInformation>();
    /// <summary>
    /// 所有未下载的商品数据
    /// </summary>
    private List<ModelInformation> allNotDownShopDataList = new List<ModelInformation>();
    /// <summary>
    /// 已下载的商品数据链表
    /// </summary>
    private List<ModelInformation> downShopDataList = new List<ModelInformation>();


    public Dictionary<string, int> BrowsedPage = new Dictionary<string, int>();

    //--------------------------------搜索部分----------------------------------
    /// <summary>
    /// 当前打开的类别的英文名字
    /// </summary>
    private string currentTypeName;
    /// <summary>
    /// 搜索按钮
    /// </summary>
    public Button SearchButton;
    /// <summary>
    /// 取消按钮
    /// </summary>
    public Button CancleButton;
    /// <summary>
    /// 搜索框
    /// </summary>
    public InputField SearchInputField;
    /// <summary>
    /// 符合搜索条件的列表
    /// </summary>
    private List<ModelInformation> searchList = new List<ModelInformation>();
    /// <summary>
    /// 被搜索的列表
    /// </summary>
    private List<ModelInformation> toSearchList = new List<ModelInformation>();
    /// <summary>
    /// 当前是否是搜索
    /// </summary>
    private bool search;

    //---------------------------------批量下载部分-----------------------------------
    /// <summary>
    /// 当前页的所有模型的列表
    /// </summary>
    private List<PreviewItem> CurrentPageModeList = new List<PreviewItem>();
    /// <summary>
    /// 被批量下载选中的商品的列表
    /// </summary>
    public List<ModelInformation> BulkDownloadList = new List<ModelInformation>();
    /// <summary>
    /// 并行下载最大数量
    /// </summary>
    private int ParallelDownMaxNum = 4;
    /// <summary>
    /// 并行下载已下载的数量
    /// </summary>
    private int DownLoadNum;
    /// <summary>
    /// 需要并行下载的总数量
    /// </summary>
    private int AllDownLoadNum;
    public GameObject ParallelDownPanel;

    private List<Coroutine> parallelDowncoroutines = new List<Coroutine>();

    public Text DownloadedText;
    public Text AllText;
    public Image ProgressImg;


    /// <summary>
    /// 开始批量下载
    /// </summary>
    public void StartBulkDownLoad()
    {
        if (BulkDownloadList!=null&&BulkDownloadList.Count>0)
        {
            ParallelDownPanel.SetActive(true);
            DownLoadNum = 0;
            AllDownLoadNum = BulkDownloadList.Count;
            AllText.text = AllDownLoadNum.ToString();
            ProgressImg.fillAmount = 0;
            StartCoroutine(DoParallelDown());
        }
    }

    public IEnumerator DoParallelDown()
    {
        if (BulkDownloadList!=null && BulkDownloadList.Count>0)
        {
            for (int i = 0; i < BulkDownloadList.Count; i++)
            {
                if (parallelDowncoroutines.Count < ParallelDownMaxNum)
                {
                    parallelDowncoroutines.Add(StartCoroutine(DoBulkDownLoad(BulkDownloadList[i])));
                }
                while (parallelDowncoroutines.Count >= ParallelDownMaxNum)
                {
                    //Debug.Log(parallelDowncoroutines.Count + "   " + ParallelDownMaxNum);
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(0.1f);
                Debug.Log("parallelDowncoroutines.Count:" + parallelDowncoroutines.Count + "ParallelDownMaxNum:" + ParallelDownMaxNum + "i:" + i);
            }
        }
        else
        {
            //未选中物品
        }
    }

    public IEnumerator DoBulkDownLoad(ModelInformation data)
    {
        Debug.Log("StartDown:" + data.ModelID);
        byte[] picturebyte;
        byte[] modelbyte;
        Sprite sprite;
        //先从预览图对象池中找预览图
        sprite = spritePool.GetSpritePool(data.ModelID);
        if (sprite != null)
        {
            picturebyte = sprite.texture.EncodeToJPG();
            File.WriteAllBytes(Application.streamingAssetsPath + "/ink2/" + data.ModelName_EN + ".png", picturebyte);
        }
        else
        {
            WWW image = new WWW(ShopManager.Instance.SpritePath + data.ModelID);
            yield return image;
            if (image.error == null)
            {
                picturebyte = image.bytes;
                File.WriteAllBytes(Application.streamingAssetsPath + "/ink2/" + data.ModelName_EN + ".png", picturebyte);
            }
            else
            {
                Debug.LogError("下载模型预览图时出现错误：" + image.error);
            }
            yield return null;
        }

        WWW model = new WWW(ShopManager.Instance.ModelPath + data.ModelID);
        while (!model.isDone)
        {
            yield return null;
        }
        if (model.error == null)
        {
            modelbyte = model.bytes;
            File.WriteAllBytes(TheTools.Tools.Instance.GetAssteBundlesPath() + "/" + data.ModelTypeName_EN + "/" + data.ModelName_EN + ".3dpro", modelbyte);
        }
        else
        {
            Debug.LogError("下载模型时出现错误：" + model.error);
        }
        WriteText(data);

        ParallelDownMaxNum++;
        //Debug.LogError(ParallelDownMaxNum);
        DownLoadNum++;
        DownloadedText.text = DownLoadNum.ToString();
        ProgressImg.fillAmount = (float)DownLoadNum / (float)AllDownLoadNum;
        if (ProgressImg.fillAmount==1)
        {

            BulkDownloadList.Clear();
            ParallelDownPanel.SetActive(false);
            Manager.Instace.RefershResourcePanel();

        }
        Debug.Log("EndDown:" + data.ModelID);
    }


    /// <summary>
    /// 写文件
    /// </summary>
    public void WriteText(ModelInformation data)
    {
        Debug.Log("开始写文件");
        //读取文件
        string jsonstr = File.ReadAllText(Application.streamingAssetsPath + "/" + data.ModelTypeName_EN + ".txt", Encoding.GetEncoding("Unicode"));
        JsonData charactersJd = JsonMapper.ToObject(jsonstr);
        JsonData character = charactersJd[data.ModelTypeName_EN];
        JsonData xxx = new JsonData();
        xxx["name"] = data.ModelName_EN;
        xxx["modelpath"] = "AssetBundles/" + data.ModelTypeName_EN + "/" + data.ModelName_EN + ".3dpro";
        xxx["name2"] = data.ModelName_CH;
        character.ValueAsArray().Add(xxx);
        charactersJd[data.ModelTypeName_EN] = character;
        jsonstr = JsonMapper.ToJson(charactersJd);
        jsonstr = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(jsonstr));
        File.WriteAllText(Application.streamingAssetsPath + "/" + data.ModelTypeName_EN + ".txt", jsonstr, Encoding.GetEncoding("Unicode"));

        Debug.Log("写入完成");
        //将已下载的商店物品的id记录下来
        ShopManager.Instance.CommitDownId(data);
    }

    public void SelectAllButtonClick()
    {
        if (CurrentPageModeList!=null&&CurrentPageModeList.Count>0)
        {
            for (int i = 0; i < CurrentPageModeList.Count; i++)
            {
                CurrentPageModeList[i].SelectedToggle.isOn = true;
            }
        }
    }
    public void SelectNoneButtonClick()
    {
        if (CurrentPageModeList != null && CurrentPageModeList.Count > 0)
        {
            for (int i = 0; i < CurrentPageModeList.Count; i++)
            {
                CurrentPageModeList[i].SelectedToggle.isOn = false;
            }
        }
    }


















    public int ShopCurrentPage
    {
        get
        {
            return shopCurrentPage;
        }

        set
        {
            //判断页码是否不重复且合法
            if (value > 0 && value <= shopAllPages)
            {
                shopCurrentPage = value;
                ShopPageInputField.text = value.ToString();
                if (search)
                {
                    StartCoroutine(PreViewDownLoad(searchList, value));
                }
                else
                {
                    preViewDownLoadCoroutine = StartCoroutine(PreViewDownLoad(notDownShopDataList, value));

                    //记录当前分类浏览到那一页，方便下次打开该分类时自动打开该页面
                    if (BrowsedPage.ContainsKey(currentTypeName))
                    {
                        BrowsedPage[currentTypeName] = shopCurrentPage;
                    }
                    else
                    {
                        BrowsedPage.Add(currentTypeName, shopCurrentPage);
                    }
                }
            }//如果不
            else
            {
                ShopPageInputField.text = shopCurrentPage.ToString();
            }
        }
    }

    public int DownCurrentPage
    {
        get
        {
            return downCurrentPage;
        }

        set
        {
            //判断页码是否不重复且合法
            if (value > 0 && value <= downAllPages)
            {
                downCurrentPage = value;
                DownPageInputField.text = value.ToString();
                StartCoroutine(ShowLocalPreview(downShopDataList, value));

            }//如果页码重复或不合法
            else
            {
                DownPageInputField.text = downCurrentPage.ToString();
            }
        }
    }

    public bool ShowShop
    {
        get
        {
            return showShop;
        }

        set
        {
            showShop = value;
            if (value == false)
            {

                ShopGameObject.SetActive(false);
                DownGameObject.SetActive(true);
            }
            else
            {
                ShopGameObject.SetActive(true);
                DownGameObject.SetActive(false);
            }
        }
    }

    //public ModelType ModelType
    //{
    //    get
    //    {
    //        return modelType;
    //    }

    //    set
    //    {
    //        modelType = value;
    //    }
    //}





    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //初始化服务器等相关地址
        if (File.Exists(Application.streamingAssetsPath + "/serverpath.txt"))
        {
            string str = File.ReadAllText(Application.streamingAssetsPath + "/serverpath.txt", Encoding.Unicode);
            ServerPath serverPath = JsonMapper.ToObject<ServerPath>(str);
            serverShopDataPath = serverPath.ServerShopDataPath;
            serverUserDataPath = serverPath.ServerUserDataPath;
            SpritePath = serverPath.SpriteDownPath;
            ModelPath = serverPath.ModelDownPath;
        }
        else
        {
            Debug.LogError("没有找到记录服务器地址的文本！请检查");
            serverShopDataPath = "";
            serverUserDataPath = "";
            SpritePath = "";
            ModelPath = "";
        }



        path = Application.dataPath;
        path = path.Substring(0, path.LastIndexOf("/")) + "/AssetBundles/";

        //animalPath = path + "animal/";
        //animalTXT = "animal";

        //animationPath = path + "animathion/";
        //animationTXT = "animation";

        //characterPath = path + "character/";
        //characterTXT = "character";

        //plantPath = path + "plant/";
        //plantTXT = "plant";

        //geometryPath = path + "geometry/";
        //geometryTXT = "geometry";


        //foodPath = path + "food/";
        //foodTXT = "food";

        //weaponPath = path + "weapon/";
        //weaponTXT = "weapon";

        //equipPath = path + "equip/";
        //equipTXT = "equip";

        //estatePath = path + "estate/";
        //estateTXT = "estate";

        //dianliPath = path + "dianli/";
        //dianliTXT = "dianli";

        //scenePath = path + "scene/";
        //sceneTXT = "scene";

        //signalPath = path + "signal/";
        //signalTXT = "signal";

        //terrainPath = path + "terrain/";
        //terrainTXT = "terrain";

        //texiaoPath = path + "texiao/";
        //texiaoTXT = "texiao/";

        //toolPath = path + "tool/";
        //toolTXT = "tool";

        //vehiclePath = path + "vehicle/";
        //vehicleTXT = "vehicle";

        //windoorPath = path + "windoor/";
        //windoorTXT = "windoor";

        //otherPath = path + "other/";
        //otherTXT = "other";

        //testPath = path + "test5/";
        //testTXT = "test5";


        userDownDataFolderPath = Application.streamingAssetsPath + "/UserDownData/";
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 切换按钮按下时
    /// </summary>
    public void ChangeButtonClick(bool showShop)
    {
        if (showShop != ShowShop)
        {
            ShowShop = showShop;
            if (!showShop)
            {
                DownCurrentPage = 1;
            }
        }
    }

    public void InitShopData(string str)
    {
        ShowShop = true;
        //if (currentTypeName==str)
        //{
        //    return;
        //}
        currentTypeName = str;
        search = false;
        if (preViewDownLoadCoroutine != null)
        {
            StopCoroutine(preViewDownLoadCoroutine);
            preViewDownLoadCoroutine = null;
        }

        if (coroutines != null && coroutines.Count > 0)
        {
            for (int i = 0; i < coroutines.Count; i++)
            {
                if (coroutines[i] != null)
                {
                    StopCoroutine(coroutines[i]);
                }
            }
            coroutines.Clear();
        }

        Classification(str);
        SearchInputField.text = "";
    }

    /// <summary>
    /// 分类
    /// </summary>
    public void Classification(string type)
    {
        GetShopDatas();
        GetUserData();
        //获取所有未下载的数据
        if (allNotDownShopDataList != null && allNotDownShopDataList.Count > 0)
        {
            allNotDownShopDataList.Clear();
        }
        if (downShopDataList.Count > 0)
        {
            downShopDataList.Clear();
        }
        for (int i = 0; i < ShopDataList.Count; i++)
        {
            allNotDownShopDataList.Add(ShopDataList[i]);
        }
        if (allNotDownShopDataList != null && allNotDownShopDataList.Count > 0)
        {
            //for (int i = allNotDownShopDataList.Count - 1; i >= 0; i--)
            //{
            //    for (int j = 0; j < userData.AllList.Count; j++)
            //    {
            //        if (allNotDownShopDataList[i].ModelID == userData.AllList[j])
            //        {
            //            allNotDownShopDataList.Remove(allNotDownShopDataList[i]);
            //            break;
            //        }
            //    }
            //}

            for (int i = 0; i < ShopImformation.Instance.ModelTypes.Count; i++)
            {
                //Debug.LogError(ShopImformation.Instance.ModelTypes[i].UserDownModelIDList.Count);
                for (int j = 0; j < ShopImformation.Instance.ModelTypes[i].UserDownModelIDList.Count; j++)
                {
                    ModelInformation shopdata = allNotDownShopDataList.Find(modelInformation =>
                        modelInformation.ModelID ==
                        ShopImformation.Instance.ModelTypes[i].UserDownModelIDList[j]);
                    if (shopdata != null)
                    {
                        allNotDownShopDataList.Remove(shopdata);
                    }
                }
            }

            ModelTypeInformation modelType = ShopImformation.Instance.GetModelTypeByTypeName_EN(type);
            SetClass(modelType.ModelTypeName_CH, ref modelType.NotDownModelInformations, ref modelType.DownModelInformations, modelType.UserDownModelIDList);
            //switch (type)
            //{
            //    case "geometry":
            //        SetClass("几何体",  ref _geometryList, ref _downGemetryList, userData.GeometryList);
            //        ModelType = ModelType.geometry;
            //        break;
            //    case "animal":
            //        SetClass("动物",  ref _animalList, ref _downAnimalList, userData.AnimalList);
            //        ModelType = ModelType.animal;
            //        break;
            //    case "animation":
            //        SetClass("动画",  ref _animationList, ref _downAnimationList, userData.AnimationList);
            //        ModelType = ModelType.animation;
            //        break;
            //    case "character":
            //        SetClass("角色",  ref _characterList, ref _downCharacterList, userData.CharacterList);
            //        ModelType = ModelType.character;
            //        break;
            //    case "plant":
            //        SetClass("石头植物",  ref _plantList, ref _downPlantList, userData.PlantList);
            //        ModelType = ModelType.plant;
            //        break;

            //    case "equip":
            //        SetClass("生活用品",  ref _equipList, ref _downEquipList, userData.EquipList);
            //        ModelType = ModelType.equip;
            //        break;
            //    case "estate":
            //        SetClass("家具",  ref _estateList, ref _downEstateList, userData.EstateList);
            //        ModelType = ModelType.estate;
            //        break;
            //    case "dianli":
            //        SetClass("生活电器",  ref _dianliList, ref _downDianliList, userData.DianliList);
            //        ModelType = ModelType.dianli;
            //        break;

            //    case "scene":
            //        SetClass("场景", ref _sceneList, ref _downSceneList, userData.SceneList);
            //        ModelType = ModelType.scene;
            //        break;
            //    case "signal":
            //        SetClass("灯光",  ref _signalList, ref _downSignalList, userData.SignalList);
            //        ModelType = ModelType.signal;
            //        break;
            //    case "terrain":
            //        SetClass("地形",  ref _terrainList, ref _downTerrainList, userData.TerrainList);
            //        ModelType = ModelType.terrain;
            //        break;
            //    case "texiao":
            //        SetClass("特效",  ref _texiaoList, ref _downTexiaoList, userData.TexiaoList);
            //        ModelType = ModelType.texiao;
            //        break;

            //    case "vehicle":
            //        SetClass("车辆",  ref _vehicleList, ref _downVehicleList, userData.VehicleList);
            //        ModelType = ModelType.vehicle;
            //        break;
            //    case "weapon":
            //        SetClass("武器",  ref _weaponList, ref _downVehicleList, userData.WeaponList);
            //        ModelType = ModelType.weapon;
            //        break;
            //    case "food":
            //        SetClass("食品饮料",  ref _foodList, ref _downVehicleList, userData.FoodList);
            //        ModelType = ModelType.food;
            //        break;


            //    case "other":
            //        SetClass("无类别",  ref _otherList, ref _downOtherList, userData.OtherList);
            //        ModelType = ModelType.other;
            //        break;
            //    default:
            //        NoModelImageGameObject.SetActive(true);
            //        ModelType = ModelType.none;
            //        break;
            //}
        }
        else
        {
            Debug.LogError("总的商店数据为空！！！");
        }
    }
    /// <summary>
    /// 分页
    /// </summary>
    public void Paging()
    {
        if (!search)
        {
            if (notDownShopDataList.Count > 0)
            {
                NoModelImageGameObject.SetActive(false);
                if (downShopDataList != null && downShopDataList.Count > 0)
                {
                    //计算未下载的总页数
                    if (notDownShopDataList.Count % 10 == 0)
                    {
                        shopAllPages = notDownShopDataList.Count / 10;
                    }
                    else
                    {
                        shopAllPages = notDownShopDataList.Count / 10 + 1;
                    }
                    //计算已下载的总页数
                    if (downShopDataList.Count % 10 == 0)
                    {
                        downAllPages = downShopDataList.Count / 10;
                    }
                    else
                    {
                        downAllPages = downShopDataList.Count / 10 + 1;
                    }
                }
                else
                {
                    //计算未下载的总页数
                    if (notDownShopDataList.Count % 10 == 0)
                    {
                        shopAllPages = notDownShopDataList.Count / 10;
                    }
                    else
                    {
                        shopAllPages = notDownShopDataList.Count / 10 + 1;
                    }
                    //当已下载的物品数量为0时设置总页数为1
                    downAllPages = 1;
                }
            }
            else
            {
                NoModelImageGameObject.SetActive(true);
            }
            ShopAllPagesText.text = "/" + shopAllPages;
            DownAllPagesText.text = "/" + downAllPages;

            if (BrowsedPage.ContainsKey(currentTypeName))
            {
                if (BrowsedPage[currentTypeName] > shopAllPages)
                {
                    ShopCurrentPage = shopAllPages;
                    //Debug.LogError("12345");
                }
                else
                {
                    ShopCurrentPage = BrowsedPage[currentTypeName];
                    //Debug.LogError("12345");
                }
            }
            else
            {
                //设置页码为第一页
                ShopCurrentPage = 1;
            }
            //DownCurrentPage = 1;
        }
        else
        {
            if (searchList.Count > 0)
            {
                if (searchList.Count % 10 == 0)
                {
                    shopAllPages = searchList.Count / 10;
                }
                else
                {
                    shopAllPages = searchList.Count / 10 + 1;
                }
            }
            else
            {
                shopAllPages = 1;
            }
            ShopAllPagesText.text = "/" + shopAllPages;
            ShopCurrentPage = 1;
        }

    }
    /// <summary>
    /// 设置页码跳转
    /// </summary>
    public void SetPage(bool isdown)
    {
        int p;
        if (isdown)
        {
            p = int.Parse(DownPageInputField.text);
            DownCurrentPage = p;
        }
        else
        {
            p = int.Parse(ShopPageInputField.text);
            ShopCurrentPage = p;
        }
    }
    /// <summary>
    /// 上一页或下一页
    /// </summary>
    /// <param name="a"></param>
    public void AddShopPage(int a)
    {
        ShopCurrentPage = ShopCurrentPage + a;
    }
    public void AddDownPage(int a)
    {
        DownCurrentPage = DownCurrentPage + a;
    }

    IEnumerator ShowLocalPreview(List<ModelInformation> shopDatas, int page)
    {

        //当前页第一个元素
        int a;
        //当前页最后一个元素
        int b;
        //判断页码是否合法
        if (page > 0 && page <= downAllPages)
        {
            if (page == downAllPages)
            {
                a = (page - 1) * 10;
                b = shopDatas.Count;
            }
            else
            {
                a = (page - 1) * 10;
                b = a + 10;
            }
        }
        else
        {
            a = 0;
            b = 0;
        }
        //把物体放入对象池
        int childNum = DownContentedTransform.childCount;
        for (int i = 0; i < childNum; i++)
        {
            GameObject obj = DownContentedTransform.GetChild(0).gameObject;
            if (obj.GetComponent<PreviewItem>().DownLoading)
            {
                DownLoadingPreviewItemPool.AddPreviewItemObjectPool(
                    obj.GetComponent<PreviewItem>().shopData.ModelID, obj.gameObject);
                obj.transform.SetParent(DownLoadingPreviewItemPoolTransform);
            }
            else
            {
                obj.transform.FindChild("icon").gameObject.SetActive(false);
                previewItemPool.AddPreviewItemObjectPool(obj);
                obj.transform.SetParent(PreviewItemPoolTransform);
                obj.transform.localPosition = Vector3.zero;
            }
        }
        //从对象池取物体
        if (shopDatas.Count > 0)
        {
            for (int i = a; i < b; i++)
            {
                GameObject obj = DownLoadingPreviewItemPool.GetPreviewItemGameObjectPool(shopDatas[i].ModelID);
                if (obj == null)
                {
                    obj = previewItemPool.GetPreviewItemGameObjectPool();
                    obj.transform.SetParent(DownContentedTransform);
                    obj.transform.localScale = Vector3.one;
                    obj.GetComponent<PreviewItem>().SetShopData(shopDatas[i], true);
                    //coroutines.Add(StartCoroutine(DoShowInformation(obj.transform.FindChild("icon").GetComponent<Image>(), shopDatas[i].ID)));

                }
                else
                {
                    obj.transform.SetParent(DownContentedTransform);
                }
                yield return null;
            }
        }
    }
    /// <summary>
    /// 加载某一页预览图的协成
    /// </summary>
    /// <param name="shopDatas">未下载的商店数据链表</param>
    /// <param name="page">页码</param>
    /// <returns></returns>
    IEnumerator PreViewDownLoad(List<ModelInformation> shopDatas, int page, bool isdown = false)
    {
        Transform content;

        int allpage;
        if (isdown)
        {
            content = DownContentedTransform;
            allpage = downAllPages;
        }
        else
        {
            content = ShopContentTransform;
            allpage = shopAllPages;
        }

        //当前页第一个元素
        int a;
        //当前页最后一个元素
        int b;
        //判断页码是否合法
        if (page > 0 && page <= allpage)
        {
            if (page == allpage)
            {
                a = (page - 1) * 10;
                b = shopDatas.Count;
            }
            else
            {
                a = (page - 1) * 10;
                b = a + 10;
            }
        }
        else
        {
            a = 0;
            b = 0;
        }
        
        //停止正在加载预览图的携程
        //if (coroutines.Count > 0)
        //{
        //    for (int i = 0; i < coroutines.Count; i++)
        //    {
        //        if (coroutines[i] != null)
        //        {
        //            StopCoroutine(coroutines[i]);
        //        }
        //    }
        //    coroutines.Clear();
        //}
        //把物体放入对象池
        int childNum = content.childCount;
        for (int i = 0; i < childNum; i++)
        {
            GameObject obj = content.GetChild(0).gameObject;
            if (obj.GetComponent<PreviewItem>().DownLoading)
            {
                DownLoadingPreviewItemPool.AddPreviewItemObjectPool(
                    obj.GetComponent<PreviewItem>().shopData.ModelID, obj.gameObject);
                obj.transform.SetParent(DownLoadingPreviewItemPoolTransform);
            }
            else
            {
                obj.transform.FindChild("icon").gameObject.SetActive(false);
                previewItemPool.AddPreviewItemObjectPool(obj);
                obj.transform.SetParent(PreviewItemPoolTransform);
                obj.transform.localPosition = Vector3.zero;
            }
        }
        //从对象池取物体
        if (shopDatas.Count > 0)
        {
            CurrentPageModeList.Clear();
            for (int i = a; i < b; i++)
            {
                GameObject obj = DownLoadingPreviewItemPool.GetPreviewItemGameObjectPool(shopDatas[i].ModelID);
                if (obj == null)
                {
                    obj = previewItemPool.GetPreviewItemGameObjectPool();
                    obj.transform.SetParent(content);
                    obj.transform.localScale = Vector3.one;
                    obj.GetComponent<PreviewItem>().SetShopData(shopDatas[i], GetIsDown(shopDatas[i]));
                    coroutines.Add(StartCoroutine(DoShowInformation(obj.transform.FindChild("icon").GetComponent<Image>(), shopDatas[i].ModelID)));
                }
                else
                {
                    obj.transform.SetParent(content);
                }
                CurrentPageModeList.Add(obj.GetComponent<PreviewItem>());
                yield return null;
            }
        }
    }

    IEnumerator DoShowInformation(Image image, string id)
    {
        Sprite TheSprite = spritePool.GetSpritePool(id);
        if (TheSprite == null)
        {
            WWW www = new WWW(SpritePath + id);
            //WWW www=new WWW(@"file:///"+ shopDatas[i].SpritePath);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError(www.error);
            }
            Texture2D tex2d = www.texture;
            TheSprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f));
            spritePool.AddSpritePool(id, TheSprite);
        }
        image.sprite = TheSprite;
        image.gameObject.SetActive(true);
    }


    /// <summary>
    /// 读取本地的用户数据文档
    /// </summary>
    /// <returns></returns>
    public void GetUserData()
    {
        //if (File.Exists(Application.streamingAssetsPath + "/userdata.txt"))
        //{
        //    string str = File.ReadAllText(Application.streamingAssetsPath + "/userdata.txt");
        //    userData = JsonMapper.ToObject<Assets.Scripts.DownLoadOnLine.UserData>(str);
        //}
        //else
        //{
        //    Debug.LogError("没有找到本地的用户数据文本！！！");
        //}

        for (int i = 0; i < ShopImformation.Instance.ModelTypes.Count; i++)
        {
            if (File.Exists(userDownDataFolderPath + ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN))
            {
                if (File.ReadAllText(userDownDataFolderPath +
                    ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN).Length > 0)
                {
                    ShopImformation.Instance.ModelTypes[i].UserDownModelIDList = JsonMapper.ToObject<List<string>>(
                    File.ReadAllText(userDownDataFolderPath +
                    ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN));
                }
                //Debug.LogError(ShopImformation.Instance.ModelTypes[i].UserDownModelIDList.Count);
            }
            else
            {
                File.Create(userDownDataFolderPath + ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN).Close();

                //读取已经在资源列表中的模型名字
                string jsonstr = File.ReadAllText(Application.streamingAssetsPath + "/" + ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN + ".txt", Encoding.GetEncoding("Unicode"));
                JsonData Jd = JsonMapper.ToObject(jsonstr);
                JsonData character = Jd[ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN];
                foreach (JsonData jsonData in character)
                {
                    //Debug.LogError(jsonData["name"]);
                    ModelInformation modelInformation = ShopDataList.Find(mif =>
                        mif.ModelName_EN == jsonData["name"].ToString());
                    if (modelInformation != null)
                    {
                        ShopImformation.Instance.ModelTypes[i].UserDownModelIDList.Add(modelInformation.ModelID);
                    }
                }
                File.WriteAllText(userDownDataFolderPath + ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN, JsonMapper.ToJson(ShopImformation.Instance.ModelTypes[i].UserDownModelIDList));
            }


        }

    }
    /// <summary>
    /// 读取从服务器下载到本地的商店数据文档
    /// </summary>
    /// <param name="datatxtname">不同下载地址的数据文本文件名</param>
    /// <returns></returns>
    public List<ModelInformation> GetShopDatas()
    {
        //string str = File.ReadAllText(Application.streamingAssetsPath + "/shopdata.txt", Encoding.UTF8);
        //if (string.IsNullOrEmpty(CurrentShopDataPath))
        //{
        //    CurrentShopDataPath = "shopdata.txt";
        //}
        string str = File.ReadAllText(Application.streamingAssetsPath + "/" + "shopdata.txt", Encoding.UTF8);
        string[] shopdata = str.Split('\n');
        ShopDataList = new List<ModelInformation>();
        for (int i = 0; i < shopdata.Length - 1; i++)
        {
            string[] s = shopdata[i].Split('\t');
            ModelInformation shopData = new ModelInformation(s[0], s[1], s[2], s[3].TrimEnd("\n\r".ToCharArray()));
            ShopDataList.Add(shopData);
        }
        Debug.LogError(ShopDataList[ShopDataList.Count - 1].ModelName_CH);
        return ShopDataList;
    }

    public void StartDown()
    {
        StartCoroutine(DoDownShopData(serverShopDataPath));
        //StartCoroutine(DoDownUserData(serverUserDataPath));
    }
    /// <summary>
    /// 从服务器下载商店数据文本到本地
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator DoDownShopData(string url)
    {
        //Debug.LogError(url);
        Debug.Log("开始下载商店数据文本");
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            File.WriteAllBytes(Application.streamingAssetsPath + "/shopdata.txt", www.bytes);
            Debug.Log("商店数据下载完成");
        }
        else
        {
            Debug.LogError("商店数据下载失败:" + www.error);
        }
    }
    /// <summary>
    /// 从服务器下载用户数据文本到本地
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator DoDownUserData(string url)
    {
        //WWW www = new WWW(@"file:///" + url);
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            File.WriteAllBytes(Application.streamingAssetsPath + "/userdata.txt", www.bytes);
            Debug.LogError("用户数据下载完成");
        }
        else
        {
            Debug.LogError("用户数据下载失败:" + www.error);
        }
    }

    /// <summary>
    /// 记录已下载物品的id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    public void CommitDownId(ModelInformation shopData)
    {
        RefreshData(shopData);

        //switch (shopData.type)
        //{
        //    case "几何体":
        //        //userData.GeometryList.Add(shopData.ID);
        //        //if (_geometryList.Contains(shopData))
        //        //{
        //        //    _geometryList.Remove(shopData);
        //        //}
        //        //_downGemetryList.Add(shopData);
        //        RefreshData(ref _geometryList,ref userData.GeometryList,shopData, "geometry");
        //        break;
        //    case "动物":
        //        //userData.AnimalList.Add(shopData.ID);
        //        //if (_animalList.Contains(shopData))
        //        //{
        //        //    _animalList.Remove(shopData);
        //        //}
        //        //_downAnimalList.Add(shopData);
        //        RefreshData(ref _animalList, ref userData.AnimalList, shopData, "animal");
        //        break;
        //    case "动画":
        //        //userData.AnimationList.Add(shopData.ID);
        //        //if (_animationList.Contains(shopData))
        //        //{
        //        //    _animationList.Remove(shopData);
        //        //}
        //        //_downAnimationList.Add(shopData);
        //        RefreshData(ref _animationList, ref userData.AnimationList, shopData, "animation");
        //        break;
        //    case "角色":
        //        //userData.CharacterList.Add(shopData.ID);
        //        //if (_characterList.Contains(shopData))
        //        //{
        //        //    _characterList.Remove(shopData);
        //        //}
        //        //_downCharacterList.Add(shopData);
        //        RefreshData(ref _characterList, ref userData.CharacterList, shopData, "character");
        //        break;
        //    case "石头植物":
        //        //userData.PlantList.Add(shopData.ID);
        //        //if (_plantList.Contains(shopData))
        //        //{
        //        //    _plantList.Remove(shopData);
        //        //}
        //        //_downPlantList.Add(shopData);
        //        RefreshData(ref _plantList, ref userData.PlantList, shopData, "plant");
        //        break;
        //    case "生活用品":
        //        //userData.EquipList.Add(shopData.ID);
        //        //if (_equipList.Contains(shopData))
        //        //{
        //        //    _equipList.Remove(shopData);
        //        //}
        //        //_downEquipList.Add(shopData);
        //        RefreshData(ref _equipList, ref userData.EquipList, shopData, "equip");
        //        break;
        //    case "武器":
        //        //userData.EstateList.Add(shopData.ID);
        //        //if (_estateList.Contains(shopData))
        //        //{
        //        //    _estateList.Remove(shopData);
        //        //}
        //        //_downEstateList.Add(shopData);
        //        RefreshData(ref _weaponList, ref userData.WeaponList, shopData, "weapon");
        //        break;
        //    case "食品饮料":
        //        //userData.EstateList.Add(shopData.ID);
        //        //if (_estateList.Contains(shopData))
        //        //{
        //        //    _estateList.Remove(shopData);
        //        //}
        //        //_downEstateList.Add(shopData);
        //        RefreshData(ref _foodList, ref userData.FoodList, shopData, "food");
        //        break;
        //    case "家具":
        //        //userData.EstateList.Add(shopData.ID);
        //        //if (_estateList.Contains(shopData))
        //        //{
        //        //    _estateList.Remove(shopData);
        //        //}
        //        //_downEstateList.Add(shopData);
        //        RefreshData(ref _estateList, ref userData.EstateList, shopData, "estate");
        //        break;
        //    case "电力":
        //        //userData.KggList.Add(shopData.ID);
        //        //if (_kggList.Contains(shopData))
        //        //{
        //        //    _kggList.Remove(shopData);
        //        //}
        //        //_downKggList.Add(shopData);
        //        RefreshData(ref _dianliList, ref userData.DianliList, shopData, "dianli");
        //        break;
        //    case "场景":
        //        //userData.SceneList.Add(shopData.ID);
        //        //if (_sceneList.Contains(shopData))
        //        //{
        //        //    _sceneList.Remove(shopData);
        //        //}
        //        //_downSceneList.Add(shopData);
        //        RefreshData(ref _sceneList, ref userData.SceneList, shopData, "scene");
        //        break;
        //    case "灯光":
        //        //userData.SignalList.Add(shopData.ID);
        //        //if (_signalList.Contains(shopData))
        //        //{
        //        //    _signalList.Remove(shopData);
        //        //}
        //        //_downSignalList.Add(shopData);
        //        RefreshData(ref _signalList, ref userData.SignalList, shopData, "signal");
        //        break;
        //    case "地形":
        //        //userData.TerrainList.Add(shopData.ID);
        //        //if (_terrainList.Contains(shopData))
        //        //{
        //        //    _terrainList.Remove(shopData);
        //        //}
        //        //_downTerrainList.Add(shopData);
        //        RefreshData(ref _terrainList, ref userData.TerrainList, shopData, "terrain");
        //        break;
        //    case "特效":
        //        //userData.TexiaoList.Add(shopData.ID);
        //        //if (_texiaoList.Contains(shopData))
        //        //{
        //        //    _texiaoList.Remove(shopData);
        //        //}
        //        //_downTexiaoList.Add(shopData);
        //        RefreshData(ref _texiaoList, ref userData.TexiaoList, shopData, "texiao");
        //        break;

        //    case "车辆":
        //        //userData.VehicleList.Add(shopData.ID);
        //        //if (_vehicleList.Contains(shopData))
        //        //{
        //        //    _vehicleList.Remove(shopData);
        //        //}
        //        //_downVehicleList.Add(shopData);
        //        RefreshData(ref _vehicleList, ref userData.VehicleList, shopData, "vehicle");
        //        break;

        //    case "其他":
        //        //userData.OtherList.Add(shopData.ID);
        //        //if (_otherList.Contains(shopData))
        //        //{
        //        //    _otherList.Remove(shopData);
        //        //}
        //        //_downOtherList.Add(shopData);
        //        RefreshData(ref _otherList, ref userData.OtherList, shopData, "other");
        //        break;
        //    default:
        //        //userData.OtherList.Add(shopData.ID);
        //        //if (_otherList.Contains(shopData))
        //        //{
        //        //    _otherList.Remove(shopData);
        //        //}
        //        //_downOtherList.Add(shopData);
        //        RefreshData(ref _otherList, ref userData.OtherList, shopData, "other");
        //        break;
        //}
        //userData.AllList.Add(shopData.ID);
        //string str = JsonMapper.ToJson(userData);
        //File.WriteAllText(Application.streamingAssetsPath + "/userdata.txt", str);
    }

    public void AddIDName(string name, string id)
    {
        //读取记录
        string str = File.ReadAllText(Application.streamingAssetsPath + "/IdName");
        if (string.IsNullOrEmpty(str))
        {
            idNameDictionary = new Dictionary<string, string>();
        }
        else
        {
            idNameDictionary = JsonMapper.ToObject<Dictionary<string, string>>(str);
        }
        try
        {
            if (idNameDictionary.ContainsKey(name))
            {
                return;
            }
            idNameDictionary.Add(name, id);
            str = JsonMapper.ToJson(idNameDictionary);
            File.WriteAllText(Application.streamingAssetsPath + "/IdName", str);

        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void DeleteIdName(string name, string type)
    {
        //读取记录
        string str = File.ReadAllText(Application.streamingAssetsPath + "/IdName");
        if (string.IsNullOrEmpty(str))
        {
            idNameDictionary = new Dictionary<string, string>();
        }
        else
        {
            idNameDictionary = JsonMapper.ToObject<Dictionary<string, string>>(str);
        }
        try
        {
            for (int i = 0; i < ShopImformation.Instance.ModelTypes.Count; i++)
            {
                if (ShopImformation.Instance.ModelTypes[i].UserDownModelIDList.Contains(idNameDictionary[name]))
                {
                    ShopImformation.Instance.ModelTypes[i].UserDownModelIDList.Remove(idNameDictionary[name]);
                    File.WriteAllText(userDownDataFolderPath + ShopImformation.Instance.ModelTypes[i].ModelTypeName_EN, JsonMapper.ToJson(ShopImformation.Instance.ModelTypes[i].UserDownModelIDList));
                }
            }

            //str = File.ReadAllText(Application.streamingAssetsPath + "/userdata.txt");
            //Assets.Scripts.DownLoadOnLine.UserData ud = JsonMapper.ToObject<Assets.Scripts.DownLoadOnLine.UserData>(str);
            //if (idNameDictionary.ContainsKey(name))
            //{
            //    switch (type)
            //    {
            //        case "geometry":
            //            if (ud.GeometryList.Contains(idNameDictionary[name]))
            //            {
            //                ud.GeometryList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "animal":
            //            if (ud.AnimalList.Contains(idNameDictionary[name]))
            //            {
            //                ud.AnimalList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;

            //        case "food":
            //            if (ud.FoodList.Contains(idNameDictionary[name]))
            //            {
            //                ud.FoodList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "weapon":
            //            if (ud.WeaponList.Contains(idNameDictionary[name]))
            //            {
            //                ud.WeaponList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "animation":
            //            if (ud.AnimationList.Contains(idNameDictionary[name]))
            //            {
            //                ud.AnimationList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "character":
            //            if (ud.CharacterList.Contains(idNameDictionary[name]))
            //            {
            //                ud.CharacterList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "plant":
            //            if (ud.PlantList.Contains(idNameDictionary[name]))
            //            {
            //                ud.PlantList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "equip":
            //            if (ud.EquipList.Contains(idNameDictionary[name]))
            //            {
            //                ud.EquipList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "estate":
            //            if (ud.EstateList.Contains(idNameDictionary[name]))
            //            {
            //                ud.EstateList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "dianli":
            //            if (ud.DianliList.Contains(idNameDictionary[name]))
            //            {
            //                ud.DianliList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "scene":
            //            if (ud.SceneList.Contains(idNameDictionary[name]))
            //            {
            //                ud.SceneList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "signal":
            //            if (ud.SignalList.Contains(idNameDictionary[name]))
            //            {
            //                ud.SignalList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "terrain":
            //            if (ud.TerrainList.Contains(idNameDictionary[name]))
            //            {
            //                ud.TerrainList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "texiao":
            //            if (ud.TexiaoList.Contains(idNameDictionary[name]))
            //            {
            //                ud.TexiaoList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "tool":
            //            if (ud.ToolList.Contains(idNameDictionary[name]))
            //            {
            //                ud.ToolList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "vehicle":
            //            if (ud.VehicleList.Contains(idNameDictionary[name]))
            //            {
            //                ud.VehicleList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "windoor":
            //            if (ud.WindoorList.Contains(idNameDictionary[name]))
            //            {
            //                ud.WindoorList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        case "other":
            //            if (ud.OtherList.Contains(idNameDictionary[name]))
            //            {
            //                ud.OtherList.Remove(idNameDictionary[name]);
            //                ud.AllList.Remove(idNameDictionary[name]);
            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //    str = JsonMapper.ToJson(ud);
            //    File.WriteAllText(Application.streamingAssetsPath+"/userdata.txt",str);
            //    //InitShopData(type);
            //}


        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void RefreshData(ModelInformation shopData)
    {
        ModelTypeInformation modelTypeInformation =
            ShopImformation.Instance.GetModelTypeByTypeName_EN(shopData.ModelTypeName_EN);
        Debug.Log(modelTypeInformation.UserDownModelIDList.Count);
        modelTypeInformation.UserDownModelIDList.Add(shopData.ModelID);
        if (modelTypeInformation.NotDownModelInformations.Contains(shopData))
        {
            modelTypeInformation.NotDownModelInformations.Remove(shopData);
        }
        //userData.AllList.Add(shopData.ModelID);
        string str = JsonMapper.ToJson(modelTypeInformation.UserDownModelIDList);
        if (!File.Exists(userDownDataFolderPath + modelTypeInformation.ModelTypeName_EN))
        {
            File.Create(userDownDataFolderPath + modelTypeInformation.ModelTypeName_EN).Close();
        }
        File.WriteAllText(userDownDataFolderPath + modelTypeInformation.ModelTypeName_EN, str);
        //InitShopData(type);
    }
    /// <summary>
    /// 临时下载的对比
    /// </summary>
    public bool GetIsDown(ModelInformation shopdata)
    {
        for (int i = 0; i < ShopImformation.Instance.GetModelTypeByTypeName_EN(shopdata.ModelTypeName_EN).UserDownModelIDList.Count; i++)
        {
            if (ShopImformation.Instance.GetModelTypeByTypeName_EN(shopdata.ModelTypeName_EN).UserDownModelIDList[i] == shopdata.ModelID)
            {
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// 分类
    /// </summary>
    /// <param name="str"></param>
    /// <param name="once"></param>
    /// <param name="dataList"></param>
    /// <param name="userDownDataList"></param>
    public void SetClass(string str, ref List<ModelInformation> dataList, ref List<ModelInformation> downDataList, List<string> userDownDataList)
    {
        //if (!once)
        //{

        if (dataList.Count > 0)
        {
            dataList.Clear();
        }
        for (int i = 0; i < ShopDataList.Count; i++)
        {
            if (ShopDataList[i].ModelTypeName_CH == str)
            {
                dataList.Add(ShopDataList[i]);
                ShopDataList.Remove(ShopDataList[i]);
                i--;
            }
        }
        //once = true;
        if (userDownDataList != null && userDownDataList.Count > 0)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                for (int j = 0; j < userDownDataList.Count; j++)
                {
                    if (dataList[i].ModelID == userDownDataList[j])
                    {
                        downDataList.Add(dataList[i]);
                        dataList.Remove(dataList[i]);
                        i--;
                        break;
                    }
                }
            }

        }
        downShopDataList = downDataList;
        notDownShopDataList = dataList;
        Paging();
        //}

        //else
        //{
        //    downShopDataList = downDataList;
        //    notDownShopDataList = dataList;
        //    Paging();
        //}
    }

    /// <summary>
    /// 搜索
    /// </summary>
    //public void SearchBtnClick()
    //{
    //    searchList.Clear();
    //    //注释部分是分类搜索

    //    switch (ModelType)
    //    {
    //        case ModelType.geometry:
    //            toSearchList = _geometryList;
    //            break;
    //        case ModelType.animal:
    //            toSearchList = _animalList;
    //            break;
    //        case ModelType.animation:
    //            toSearchList = _animationList;
    //            break;
    //        case ModelType.character:
    //            toSearchList = _characterList;
    //            break;
    //        case ModelType.plant:
    //            toSearchList = _plantList;
    //            break;
    //        case ModelType.equip:
    //            toSearchList = _equipList;
    //            break;
    //        case ModelType.estate:
    //            toSearchList = _estateList;
    //            break;
    //        case ModelType.dianli:
    //            toSearchList = _dianliList;
    //            break;
    //        case ModelType.scene:
    //            toSearchList = _sceneList;
    //            break;
    //        case ModelType.signal:
    //            toSearchList = _signalList;
    //            break;
    //        case ModelType.terrain:
    //            toSearchList = _terrainList;
    //            break;
    //        case ModelType.texiao:
    //            toSearchList = _texiaoList;
    //            break;
    //        case ModelType.tool:
    //            toSearchList = _toolList;
    //            break;
    //        case ModelType.vehicle:
    //            toSearchList = _vehicleList;
    //            break;
    //        case ModelType.other:
    //            toSearchList = _otherList;
    //            break;
    //        default:
    //            break;

    //    }
    //    ////在所有未下载的模型中进行搜索
    //    //toSearchList = allNotDownShopDataList;

    //    if (!string.IsNullOrEmpty(SearchInputField.text))
    //    {
    //        for (int i = 0; i < toSearchList.Count; i++)
    //        {
    //            if (SearchInputField.text == toSearchList[i].Name2)
    //            {
    //                searchList.Add(toSearchList[i]);
    //            }
    //            else if (SearchInputField.text.Length < toSearchList[i].Name2.Length)
    //            {
    //                for (int j = 0; j < toSearchList[i].Name2.Length - SearchInputField.text.Length + 1; j++)
    //                {
    //                    if (toSearchList[i].Name2.Substring(j, SearchInputField.text.Length) == SearchInputField.text)
    //                    {
    //                        searchList.Add(toSearchList[i]);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        if (searchList.Count > 0)
    //        {
    //            //分页
    //            search = true;
    //            Paging();
    //        }
    //        else
    //        {
    //            Debug.LogError("没有搜索到任何内容");
    //        }
    //    }
    //}
    /// <summary>
    /// 取消搜索的按钮点击
    /// </summary>
    public void CancelBtnClick()
    {
        search = false;
        SearchInputField.text = "";
        //隐藏取消搜索按钮
        CancleButton.gameObject.SetActive(false);
        InitShopData(currentTypeName);
    }

    /// <summary>
    /// 当搜索框中内容发生改变时
    /// </summary>
    public void OnValueChanged()
    {
        if (SearchInputField.text != "")
        {
            //当搜索框中的内容不为“”时显示取消搜索的按钮
            CancleButton.gameObject.SetActive(true);
        }
        else
        {
            //当搜索框中的内容为“”时隐藏取消搜索的按钮
            search = false;
            CancleButton.gameObject.SetActive(false);
        }
    }
    ///// <summary>
    ///// 打开不同来源的模型数据
    ///// </summary>
    ///// <param name="path">不同来源的商店文本数据地址</param>
    //public void OpenOtherPathShopData(string path)
    //{
    //    CurrentShopDataPath = path;
    //    InitShopData(currentTypeName);
    //}
}

/// <summary>
/// 记录相关地址的类
/// </summary>
class ServerPath
{
    /// <summary>
    /// 服务器的商店数据地址
    /// </summary>
    public string ServerShopDataPath;
    /// <summary>
    /// 服务器用户数据地址
    /// </summary>
    public string ServerUserDataPath;
    /// <summary>
    /// 预览图下载地址（在后面加上每个模型的id就对应了不同的地址）
    /// </summary>
    public string SpriteDownPath;
    /// <summary>
    /// 模型下载地址（在后面加上每个模型的id就对应了不同的地址）
    /// </summary>
    public string ModelDownPath;

    public ServerPath() { }

    public ServerPath(string serverShopDataPath, string serverUserDataPath, string spriteDownPath, string modelDownPath)
    {
        ServerShopDataPath = serverShopDataPath;
        ServerUserDataPath = serverUserDataPath;
        SpriteDownPath = spriteDownPath;
        ModelDownPath = modelDownPath;
    }
}