using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMsgInit  {

    public static Dictionary<string, string> actionNames;
    public static Dictionary<string, string> As ;
    public static Dictionary<string, string> Bs ;
    public static Dictionary<string, string> Cs ;
    public static Dictionary<string,string>  Ds ;
    public static Dictionary<string, string> Es ;

    //设置动作状态的文字信息
    public static  void Init()
    {
        As=new Dictionary<string, string>();
        As.Add("SetFirstPerson", "设置第一人称");
        //As.Add("SetThirdPerson","设置第三人称");
        As.Add("FollowPlayer", "跟随主角");
        As.Add("CameraAction", "特写镜头");
       

        Bs = new Dictionary<string, string>();
        Bs.Add("OtherActionWithEvent", "发送消息");
        Bs.Add("TriggerEvent", "触发器事件");
        Bs.Add("ColliderEvent", "碰撞器事件");
        Bs.Add("WaitingSenconds", "设置延迟");
        Bs.Add("DeleteObj", "销毁物体");
        Bs.Add("CreateRoom", "创建或加入房间");
        //Bs.Add("JoinRoom", "加入房间");
        Bs.Add("MotionTrigger", "划动触发器");
        Bs.Add("LoadScene", "加载场景");
      

        Cs= new Dictionary<string, string>(); ;
        Cs.Add("ShowMsg", "激光提示信息");
        Cs.Add("ShowBtn", "VR按钮");
        Cs.Add("ShowWorldVideo", "vr视频");
     

        Ds= new Dictionary<string, string>(); ;
        Ds.Add("PCShowMsg", "显示文本信息");
        Ds.Add("ShowImg", "显示图片");
        Ds.Add("ShowButton", "显示按钮");
        Ds.Add("ShowVideo", "播放视频");
        Ds.Add("CloseInterface", "关闭界面");
      

        Es= new Dictionary<string, string>(); ;
        Es.Add("ChangeColor", "改变材质颜色");
        Es.Add("AddLightReources", "添加灯光");
        Es.Add("ParticleControl", "粒子控制");
        Es.Add("PlayAnimation", "播放动画");
        Es.Add("PhysicalSetting", "物理设置");
        Es.Add("SetVisibility", "设置可见性");
        //Es.Add("Translate","");
        //Es.Add("TransColor", "物体变色");
        Es.Add("TransMove", "物体移动");
        Es.Add("TransRotate", "物体旋转");
        Es.Add("TransScale", "物体缩放");
        Es.Add("MoveToward", "向目标移动");
        Es.Add("PlayAudio", "音效设置");
        Es.Add("DragObj", "设为可移动物体");
        Es.Add("LightObj", "环境光调整");
        //Es.Add("GetPosition", "获取坐标");
        Es.Add("PIShow", "三维展示");
        Es.Add("WorldText", "3D文本");
        Es.Add("StopAnimation", "动画停止");
        Es.Add("SetHighLight", "高光描边");
        Es.Add("MoveTarget", "移动到目标的位置");
        Es.Add("ShootingMode", "射击模式");
        Es.Add("SetTransform", "设置Transform");
        Es.Add("FollowMouseMovement", "跟随鼠标移动（有限制）");
        Es.Add("Delivery","传送");
        actionNames = new Dictionary<string, string>();
        foreach (var n in As)
        {
            actionNames.Add(n.Key,n.Value);
        }
        foreach (var n in Bs)
        {
            actionNames.Add(n.Key, n.Value);
        }
        foreach (var n in Cs)
        {
            actionNames.Add(n.Key, n.Value);
        }
        foreach (var n in Ds)
        {
            actionNames.Add(n.Key, n.Value);
        }
        foreach (var n in Es)
        {
            actionNames.Add(n.Key, n.Value);
        }
        actionNames.Add("ChoosePlayer", "选择角色界面");
        actionNames.Add("SetCurrentStatePersonId", "设置子任务");
        actionNames.Add("SetTask", "设置任务");
        actionNames.Add("SetPlayer", "设置角色");
        actionNames.Add("ShowPowerPoint", "PPT");
        actionNames.Add("ShowInterface", "添加UI");

    }

}
