using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Moden_List_Class
{
    public GameObject moden;
    public List<GameObject> moden_point;
    public List<Vector2> point_pos_list;
}





public class CameraTakePhoto : MonoBehaviour
{

   
    public GameObject[] CameraBox;
    public Transform Moden_Box;
    public Toggle isBox;
    public Toggle isAllTake;
    public GameObject take_photo_window;
    public Dropdown color_drop;
    public Slider light_slider;
    public Light light;
    public Text AllCameraViewBoxText;


    private List<Moden_List_Class> moden_list;
   

    private string picture_name;
    private int picture_num;
    private Camera cam;
    private Transform cam_pos;
    private string xml_localPath;
    private XmlDocument xmlDoc;


    private List<GameObject> AllCameraViewBox;
    private GameObject center_camerapoint;
    private float center_camerapoint_value;
    private Vector3 cam_back_pos;
    private Quaternion cam_back_roa;

    void Start ()
	{
	    picture_name = "picture";
        picture_num = 0;
	    cam=Camera.main;
	    cam_pos = CameraBox[0].transform;
        moden_list = new List<Moden_List_Class>();
	    xml_localPath = Application.streamingAssetsPath + "/xml/" + "base.xml";
	    xmlDoc = new XmlDocument();
	    xmlDoc.Load(xml_localPath);
	    AllCameraViewBox=new List<GameObject>();
	    center_camerapoint_value = -10;


	}
    //拍摄
    public void ShotPictures()
    {
        cam_back_pos = cam.transform.position;
        cam_back_roa = cam.transform.rotation;
        if (isBox.isOn)
        {
            for (int i = 0; i < Moden_Box.childCount; i++)
            {
                if (Moden_Box.GetChild(i).GetComponent<BoxCollider>())
                {
                    Moden_List_Class modenbox = new Moden_List_Class();
                    List<GameObject> moden_point = new List<GameObject>();
                    List<Vector2> point_pos_list = new List<Vector2>();
                    Create_Moden(point_pos_list, Moden_Box.GetChild(i).gameObject, moden_point);
                    modenbox.moden = Moden_Box.GetChild(i).gameObject;
                    modenbox.moden_point = moden_point;
                    modenbox.point_pos_list = point_pos_list;
                    moden_list.Add(modenbox);
                }
            }

            take_photo_window.SetActive(true);

           
        }
 
        else
        {
            if (isAllTake.isOn)
            {
                foreach (var camp in AllCameraViewBox)
                {
                    camp.SetActive(false);
                }
                foreach (var camp in AllCameraViewBox)
                {
                    cam.transform.position = camp.transform.position;
                    cam.transform.rotation = camp.transform.rotation;
                    for (int l = 0; l < 4; l++)
                    {
                        Light_All_Change(l);
                        for (int c = 0; c < color_drop.options.Count; c++)
                        {
                            switch (c)
                            {
                                case 0: Change_Material(Moden_Box.gameObject, Color.white); break;
                                case 1: Change_Material(Moden_Box.gameObject, Color.red); break;
                                case 2: Change_Material(Moden_Box.gameObject, Color.green); break;
                                case 3: Change_Material(Moden_Box.gameObject, Color.blue); break;
                            }
                            Picture_Make();

                        }
                    }
                }
                foreach (var camp in AllCameraViewBox)
                {
                    camp.SetActive(true);
                }
            }

            else
            {
                Picture_Make();
               
            }
            Reset();
        }

    }

    //确认拍摄
    public void Confirm()
    {
        if (isAllTake.isOn)
        {
            foreach (var camp in AllCameraViewBox)
            {
                camp.SetActive(false);
            }
            foreach (var camp in AllCameraViewBox)
            {
                cam.transform.position = camp.transform.position;
                cam.transform.rotation = camp.transform.rotation;
                for (int l = 0; l < 4; l++)
                {
                    Light_All_Change(l);
                    for (int c = 0; c < color_drop.options.Count; c++)
                    {
                        switch (c)
                        {
                            case 0: Change_Material(Moden_Box.gameObject, Color.white); break;
                            case 1: Change_Material(Moden_Box.gameObject, Color.red); break;
                            case 2: Change_Material(Moden_Box.gameObject, Color.green); break;
                            case 3: Change_Material(Moden_Box.gameObject, Color.blue); break;
                        }
                        CreateXml(picture_num, moden_list);
                        Picture_Make();

                    }
                }
            }

            foreach (var camp in AllCameraViewBox)
            {
                camp.SetActive(true);
            }
        }

        else
        {
            CreateXml(picture_num, moden_list);
            Picture_Make();

        }
       
        Reset();
    }

    //取消拍摄
    public void Cancel()
    {
        Reset();
    }

    //生成图片
    private void Picture_Make()
    {
        foreach (var n in CameraBox)
        {
            if (n.transform.parent != null)
            {
                if (n.transform.parent.gameObject.activeSelf && n.GetComponent<Camera>().enabled)
                {
                    cam = n.GetComponent<Camera>();
                    cam_pos = n.transform.parent;
                    cam_back_pos = cam.transform.position;
                    cam_back_roa= cam.transform.rotation;
                    break;
                }
            }
            else
            {
                if (n.activeSelf && n.GetComponent<Camera>().enabled)
                {
                    cam = n.GetComponent<Camera>();
                    cam_pos = n.transform;
                    cam_back_pos = cam.transform.position;
                    cam_back_roa = cam.transform.rotation;
                    break;
                }
            }

        }

        RenderTexture tex = new RenderTexture(Screen.width, Screen.height, 16);
        cam.targetTexture = tex;
        cam.Render();
        RenderTexture.active = tex;
        Texture2D t = new Texture2D(Screen.width, Screen.height);
        t.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // 然后将这些纹理数据，成一个png图片文件  
        byte[] bytes = t.EncodeToPNG();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(tex);
        string filename = Application.streamingAssetsPath + "/cameraimages/" + picture_name + picture_num + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);
        picture_num++;
    }


    //生成包围盒平面点位
    private List<Vector3> Make_Camera_Point(List<Vector2> point_pos_list, List<GameObject> moden_point,Transform moden)
    {
        List<Vector3> Camera_Points=new List<Vector3>();
        Vector3 a = moden.forward;
        Vector3 b = cam.transform.position - moden.position;
        float angle = Vector3.Angle(a, b);
        Debug.Log("angle:"+angle);
        if (cam_pos.position.x > moden.position.x)
        {
            if (angle < 30)
            {
                Camera_Points.Add(moden_point[0].transform.position);
                Camera_Points.Add(moden_point[4].transform.position);
                Camera_Points.Add(moden_point[6].transform.position);
                Camera_Points.Add(moden_point[2].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[0].transform.position, moden_point[4].transform.position, moden_point[6].transform.position, moden_point[2].transform.position);
            }

            else if (angle < 75)
            {
                Camera_Points.Add(moden_point[1].transform.position);
                Camera_Points.Add(moden_point[4].transform.position);
                Camera_Points.Add(moden_point[6].transform.position);
                Camera_Points.Add(moden_point[3].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[1].transform.position, moden_point[4].transform.position, moden_point[6].transform.position, moden_point[3].transform.position);

            }

            else if (angle < 120)
            {
                Camera_Points.Add(moden_point[1].transform.position);
                Camera_Points.Add(moden_point[0].transform.position);
                Camera_Points.Add(moden_point[2].transform.position);
                Camera_Points.Add(moden_point[3].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[1].transform.position, moden_point[0].transform.position, moden_point[2].transform.position, moden_point[3].transform.position);

            }
            else if (angle < 165)
            {
                Camera_Points.Add(moden_point[5].transform.position);
                Camera_Points.Add(moden_point[0].transform.position);
                Camera_Points.Add(moden_point[2].transform.position);
                Camera_Points.Add(moden_point[7].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[5].transform.position, moden_point[0].transform.position, moden_point[2].transform.position, moden_point[7].transform.position);

            }

            else
            {
                Camera_Points.Add(moden_point[5].transform.position);
                Camera_Points.Add(moden_point[1].transform.position);
                Camera_Points.Add(moden_point[3].transform.position);
                Camera_Points.Add(moden_point[7].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[5].transform.position, moden_point[1].transform.position, moden_point[3].transform.position, moden_point[7].transform.position);

            }
        }
        else
        {
            if (angle < 30)
            {
                Camera_Points.Add(moden_point[0].transform.position);
                Camera_Points.Add(moden_point[4].transform.position);
                Camera_Points.Add(moden_point[6].transform.position);
                Camera_Points.Add(moden_point[2].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[0].transform.position, moden_point[4].transform.position, moden_point[6].transform.position, moden_point[2].transform.position);

            }

            else if (angle < 75)
            {
                Camera_Points.Add(moden_point[0].transform.position);
                Camera_Points.Add(moden_point[5].transform.position);
                Camera_Points.Add(moden_point[7].transform.position);
                Camera_Points.Add(moden_point[2].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[0].transform.position, moden_point[5].transform.position, moden_point[7].transform.position, moden_point[2].transform.position);

            }

            else if (angle < 120)
            {
                Camera_Points.Add(moden_point[4].transform.position);
                Camera_Points.Add(moden_point[5].transform.position);
                Camera_Points.Add(moden_point[7].transform.position);
                Camera_Points.Add(moden_point[6].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[4].transform.position, moden_point[5].transform.position, moden_point[7].transform.position, moden_point[6].transform.position);

            }
            else if (angle < 165)
            {
                Camera_Points.Add(moden_point[4].transform.position);
                Camera_Points.Add(moden_point[1].transform.position);
                Camera_Points.Add(moden_point[3].transform.position);
                Camera_Points.Add(moden_point[6].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[4].transform.position, moden_point[1].transform.position, moden_point[3].transform.position, moden_point[6].transform.position);

            }

            else
            {
                Camera_Points.Add(moden_point[5].transform.position);
                Camera_Points.Add(moden_point[1].transform.position);
                Camera_Points.Add(moden_point[3].transform.position);
                Camera_Points.Add(moden_point[7].transform.position);
                Get_Moden_Point_Screen_Pos(point_pos_list,moden_point[5].transform.position, moden_point[1].transform.position, moden_point[3].transform.position, moden_point[7].transform.position);

            }

        }
        return Camera_Points;
    }
    //生成包围盒
    private void Create_Moden(List<Vector2> point_pos_list, GameObject moden, List<GameObject> moden_point)
    {

       
            GameObject moden_line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            moden_line.layer = 8;
            moden_line.name = "moden_line";
            moden_line.transform.parent = moden.transform;
            moden_line.transform.rotation = moden.transform.localRotation;
            moden_line.transform.localScale = moden.GetComponent<BoxCollider>().size;
            moden_line.transform.localPosition = moden.GetComponent<BoxCollider>().center;
            moden_line.GetComponent<MeshRenderer>().enabled = false;
            Create_Point(moden_point, "moden_point0", moden_line.transform, 1, 1, 1);
            Create_Point(moden_point, "moden_point1", moden_line.transform, 1, 1, -1);
            Create_Point(moden_point, "moden_point2", moden_line.transform, 1, -1, 1);
            Create_Point(moden_point, "moden_point3", moden_line.transform, 1, -1, -1);
            Create_Point(moden_point, "moden_point4", moden_line.transform, -1, 1, 1);
            Create_Point(moden_point, "moden_point5", moden_line.transform, -1, 1, -1);
            Create_Point(moden_point, "moden_point6", moden_line.transform, -1, -1, 1);
            Create_Point(moden_point, "moden_point7", moden_line.transform, -1, -1, -1);
            moden_line.AddComponent<LineRenderer>().numPositions=5;
            List<Vector3> camera_point= Make_Camera_Point(point_pos_list,moden_point, moden.transform);
            DrawLine(moden_line.GetComponent<LineRenderer>(), camera_point[0], camera_point[1], camera_point[2], camera_point[3]);
        

    }

    //生成包围盒点位
    private void Create_Point(List<GameObject> moden_point_list, string name, Transform parent, float x, float y, float z)
    {
        GameObject moden_point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        moden_point.name = name;
        moden_point.transform.parent = parent;
        moden_point.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        moden_point.layer = 8;
        moden_point.transform.localPosition = new Vector3(0.5f * x, 0.5f * y, 0.5f * z);
        moden_point.GetComponent<MeshRenderer>().enabled = false;
        moden_point_list.Add(moden_point);

    }
    //生成包围盒平面
    private void DrawLine(LineRenderer line, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        line.SetPosition(0, p1);
        line.SetPosition(1, p2);
        line.SetPosition(2, p3);
        line.SetPosition(3, p4);
        line.SetPosition(4, p1);
        line.SetWidth(0.1f, 0.1f);
    }
    //获取包围盒平面点位坐标
    public void Get_Moden_Point_Screen_Pos(List<Vector2> list, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
   
            Vector2 s1 = Camera.main.WorldToScreenPoint(p1);
            Vector2 s2 = Camera.main.WorldToScreenPoint(p2);
            Vector2 s3 = Camera.main.WorldToScreenPoint(p3);
            Vector2 s4 = Camera.main.WorldToScreenPoint(p4);
            list.Add(s1);
            list.Add(s2);
            list.Add(s3);
            list.Add(s4);


    }
    //生成模型包围盒平面XML信息
    private void CreateXml(int xml_num, List<Moden_List_Class> list)
    {

        if (File.Exists(xml_localPath))
        {
            string savePath = Application.streamingAssetsPath + "/cameraimages/" +"cam"+ xml_num + ".xml";
            XmlNode root = xmlDoc.SelectSingleNode("annotation");//获取根节点
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement element_object = xmlDoc.CreateElement("object");//增加节点
                root.AppendChild(element_object);
                XmlElement element_name = xmlDoc.CreateElement("name");
                element_name.InnerText = list[i].moden.name;
                element_object.AppendChild(element_name);
                XmlElement element_pose = xmlDoc.CreateElement("pose");
                element_pose.InnerText = "Right";
                element_object.AppendChild(element_pose);
                XmlElement element_truncated = xmlDoc.CreateElement("truncated");
                element_truncated.InnerText = "0";
                element_object.AppendChild(element_truncated);
                XmlElement element_difficult = xmlDoc.CreateElement("difficult");
                element_difficult.InnerText = "0";
                element_object.AppendChild(element_difficult);
                XmlElement element_bndbox = xmlDoc.CreateElement("bndbox");
                element_object.AppendChild(element_bndbox);
                XmlElement element_xmin = xmlDoc.CreateElement("xmin");
                element_xmin.InnerText = (int)list[i].point_pos_list[0].x + "";
                element_bndbox.AppendChild(element_xmin);
                XmlElement element_ymin = xmlDoc.CreateElement("ymin");
                element_ymin.InnerText = (int)list[i].point_pos_list[0].y + "";
                element_bndbox.AppendChild(element_ymin);
                XmlElement element_xmax = xmlDoc.CreateElement("xmax");
                element_xmax.InnerText = (int)list[i].point_pos_list[2].x + "";
                element_bndbox.AppendChild(element_xmax);
                XmlElement element_ymax = xmlDoc.CreateElement("ymax");
                element_ymax.InnerText = (int)list[i].point_pos_list[2].y + "";
                element_bndbox.AppendChild(element_ymax);
                xmlDoc.AppendChild(root);
            }
            xmlDoc.Save(savePath);
        }
    }


    //设置模型颜色
    private void Change_Material(GameObject moden, Color color)
    {
       
       
            if (moden.GetComponent<Renderer>() &&  moden.layer == 9)
            {
                moden.GetComponent<Renderer>().material.color = color;
            }
            for (int i = 0; i < moden.transform.childCount; i++)
            {
                if (moden.transform.GetChild(i).GetComponent<Renderer>() && moden.layer == 9)
                {
                    moden.transform.GetChild(i).GetComponent<Renderer>().material.color = color;
                   
                }
                if (moden.transform.GetChild(i).childCount > 0)
                {
                    Change_Material(moden.transform.GetChild(i).gameObject, color);
                }

            }

        
    }

    public void Color_Valeu_Change()
    {
        switch (color_drop.value)
        {
            case 0: Change_Material(Moden_Box.gameObject,Color.white);break;
            case 1: Change_Material(Moden_Box.gameObject, Color.red); break;
            case 2: Change_Material(Moden_Box.gameObject, Color.green); break;
            case 3: Change_Material(Moden_Box.gameObject, Color.blue); break;
        }
        
    }

    //设置模型光线
    public void Light_Change()
    {
        light.GetComponent<Light>().intensity = 1 + light_slider.value;
    }

    public void Light_All_Change(float num)
    {
        float n = num / 4;
        light.GetComponent<Light>().intensity = 1 + n;
    }

    //设置摄影环形角度
    public void All_Take_Photo_Set_Value_Change()
    {
        if (isAllTake.isOn)
        {
            Create_All_Take_Photo_Point_Deep(center_camerapoint_value);
        }
        else
        {
            GameObject.Destroy(center_camerapoint);
            foreach (var n in AllCameraViewBox)
            {
                GameObject.Destroy(n);
            }
            AllCameraViewBox=new List<GameObject>();
        }
    }
   
    //生成环形拍摄扩大点位
    private void Create_All_Take_Photo_Point_Deep(float value)
    {
        center_camerapoint = new GameObject();
        center_camerapoint.transform.position = cam.gameObject.transform.position + new Vector3(0, 0, value);
        for (int i = 0; i < 7; i++)
        {
            GameObject cameraviewpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cam.transform.RotateAround(center_camerapoint.transform.position, Vector3.up, 45);
            cameraviewpoint.transform.position = cam.transform.position;
            cameraviewpoint.transform.rotation = cam.transform.rotation;
            AllCameraViewBox.Add(cameraviewpoint);
        }
        cam.transform.RotateAround(center_camerapoint.transform.position, Vector3.up, 45);

    }

    //重置环形拍摄点位
    public void All_Take_Photo_Button_Reset()
    {
        if (isAllTake.isOn)
        {
            GameObject.Destroy(center_camerapoint);
            foreach (var n in AllCameraViewBox)
            {
                GameObject.Destroy(n);
            }
            AllCameraViewBox = new List<GameObject>();
            center_camerapoint_value = -10;
            AllCameraViewBoxText.text = center_camerapoint_value + 10 + "";
            Create_All_Take_Photo_Point_Deep(center_camerapoint_value);
        }
           
    }



    //设置环形拍摄点位距离幅度
    public void All_Take_Photo_Button(int v)
    {
        if (isAllTake.isOn)
        {
            GameObject.Destroy(center_camerapoint);
            foreach (var n in AllCameraViewBox)
            {
                GameObject.Destroy(n);
            }
            AllCameraViewBox = new List<GameObject>();
            center_camerapoint_value += v;
            AllCameraViewBoxText.text = center_camerapoint_value + 10 + "";
            Create_All_Take_Photo_Point_Deep(center_camerapoint_value);
        }
           

    }

    //拍摄重置
    public void Reset()
    {
        if (moden_list.Count > 0)
        {
            foreach (var n in moden_list)
            {
                GameObject.Destroy(n.moden_point[0].transform.parent.gameObject);
            }
            moden_list=new List<Moden_List_Class>();
        }
       
        Change_Material(Moden_Box.gameObject, Color.white);
        light.GetComponent<Light>().intensity = 1;
        cam.transform.position = cam_back_pos;
        cam.transform.rotation = cam_back_roa;

    }
}
