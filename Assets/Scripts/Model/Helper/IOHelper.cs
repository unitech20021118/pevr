using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Runtime.InteropServices;

public static class IOHelper
{

    public static string GetModelFileName()
    {
        OpenFileDlg ofn = new OpenFileDlg();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "模型文件(*.3dpro)\0*.3dpro\0";//过滤
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = "打开模型";
        ofn.defExt = "JPG";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        ofn.dlgOwner = SaveFileDialog.GetForegroundWindow();//将打开的窗口置顶
        if (OpenFileDialog.GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }

    public static string OpenFileDlgToSave()
    {
        SaveFileDlg sfn = new SaveFileDlg();
        sfn.structSize = Marshal.SizeOf(sfn);
        sfn.filter = "文件(*.pevrsf)\0*.pevrsf\0";
        sfn.file = new string(new char[256]);
        sfn.maxFile = sfn.file.Length;
        sfn.fileTitle = new string(new char[64]);
        sfn.maxFileTitle = sfn.fileTitle.Length;
        sfn.initialDir = Application.dataPath;
        sfn.title = "保存";
        sfn.defExt = "pevrsf";
        sfn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        sfn.dlgOwner = SaveFileDialog.GetForegroundWindow();
        if (SaveFileDialog.GetSaveFileName(sfn))
        {
            return sfn.file;
        }
        return string.Empty;
    }

    public static string OpenFileDlgToLoad()
    {
        OpenFileDlg ofn = new OpenFileDlg();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "文件(*.pevrsf)\0 *.pevrsf\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = "打开";
        ofn.defExt = "pevrsf";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        ofn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }

    public static string Open3dproToLoad()
    {
        OpenFileDlg sfn = new OpenFileDlg();
        sfn.structSize = Marshal.SizeOf(sfn);
        sfn.filter = "文件(*.3dpro)\0*.3dpro\0";
        sfn.file = new string(new char[256]);
        sfn.maxFile = sfn.file.Length;
        sfn.fileTitle = new string(new char[64]);
        sfn.maxFileTitle = sfn.fileTitle.Length;
        sfn.initialDir = Application.dataPath;
        sfn.title = "保存";
        sfn.defExt = "3dpro";
        sfn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        sfn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(sfn))
        {
            return sfn.file;
        }
        return string.Empty;
    }

    public static string OpenobjToLoad()
    {
        OpenFileDlg sfn = new OpenFileDlg();
        sfn.structSize = Marshal.SizeOf(sfn);
        sfn.filter = "文件(*.obj)\0*.obj\0";
        sfn.file = new string(new char[256]);
        sfn.maxFile = sfn.file.Length;
        sfn.fileTitle = new string(new char[64]);
        sfn.maxFileTitle = sfn.fileTitle.Length;
        sfn.initialDir = Application.dataPath;
        sfn.title = "保存";
        sfn.defExt = "obj";
        sfn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(sfn))
        {
            return sfn.file;
        }
        return string.Empty;
    }

    public static string OpenfbxToLoad()
    {
        OpenFileDlg sfn = new OpenFileDlg();
        sfn.structSize = Marshal.SizeOf(sfn);
        sfn.filter = "文件(*.fbx)\0*.fbx\0";
        sfn.file = new string(new char[256]);
        sfn.maxFile = sfn.file.Length;
        sfn.fileTitle = new string(new char[64]);
        sfn.maxFileTitle = sfn.fileTitle.Length;
        sfn.initialDir = Application.dataPath;
        sfn.title = "保存";
        sfn.defExt = "fbx";
        sfn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        sfn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(sfn))
        {
            return sfn.file;
        }
        return string.Empty;
    }

    public static string GetAudioFileName()
    {
        OpenFileDlg ofn = new OpenFileDlg();
        ofn.structSize = Marshal.SizeOf(ofn);
        //ofn.filter = "音效文件(*.wav)\0*.wav\0音效文件(*.ogg)\0*.ogg\0";//过滤
        ofn.filter = "音效文件(*.wav,*.mp3,*.ogg)\0*.wav;*.mp3;*.ogg\0";//过滤
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = "选择文件";
        ofn.defExt = "wav";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        ofn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }

    public static string GetImageName()
    {
        OpenFileDlg ofn = new OpenFileDlg();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "(*.jpg*.png)\0*.jpg;*.png\0";//过滤
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = "选择文件";
        ofn.defExt = "png";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        ofn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }

    public static string GetPNG()
    {
        OpenFileDlg ofn = new OpenFileDlg();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "图片文件(*.png)\0*.png\0";//过滤
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = "选择图片";
        ofn.defExt = "png";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        ofn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }
    public static string GetVideoName()
    {
        OpenFileDlg ofn = new OpenFileDlg();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "(*.mp4,*.mov,*.m4v,*.mkv,*.ts,*.webm,*.flv,*.vob,*.mpg,*.wmv,*.3gp,*.ogv)\0*.mp4;*.mov;*.m4v;*.mkv;*.ts;*.webm;*.flv;*.vob;*.mpg;*.wmv;*.3gp;*.ogv\0";//Media Files;*.mp4;*.mov;*.m4v;*.avi;*.mkv;*.ts;*.webm;*.flv;*.vob;*.ogg;*.ogv;*.mpg;*.wmv;*.3gp;Audio Files;*wav;*.mp3;*.mp2;*.m4a;*.wma;*.aac;*.au;*.flac";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = "选择文件";
        ofn.defExt = "mp4";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        ofn.dlgOwner = OpenFileDialog.GetForegroundWindow();
        if (OpenFileDialog.GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }
}
