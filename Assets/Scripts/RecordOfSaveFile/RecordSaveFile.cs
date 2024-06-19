using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System.Linq;
using System.Text;

namespace RecordOfSaveFile
{
    public static class RecordSaveFile
    {
        public static Record record { get; set; }
        /// <summary>
        /// 存档的记录的保存位置
        /// </summary>
        private static string RecordPath = Application.dataPath + "/Save/";
        private static string RecordName = "RecordSF.rsf";

        /// <summary>
        /// 获取存档的记录
        /// </summary>
        /// <returns></returns>
        public static List<Record> GetRecordSF()
        {
            if (File.Exists(RecordPath + RecordName))
            {
                return JsonMapper.ToObject<List<Record>>(File.ReadAllText(RecordPath + RecordName, Encoding.UTF8));
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 将新存档记录下来
        /// </summary>
        /// <param name="str">存档的地址</param>
        /// <param name="id">如果给的id是0，表示这是个新存档，如果大于0则是已经打开的存档，保存时覆盖保存</param>
        public static void SaveRecordSF(string str, int id = 0)
        {
            List<Record> recordList = GetRecordSF();
            Record re;
            if (id == 0)
            {
                if (recordList != null)
                {

                    re = new Record(recordList[recordList.Count - 1].ID + 1, str);
                    Record r = recordList.FirstOrDefault(a => a.PathOfRecord == str.Replace('\\', '/'));
                    if (r!=null)
                    {
                        recordList.Remove(r);
                    }
                    recordList.Add(re);
                }
                else
                {
                    recordList = new List<Record>();
                    re = new Record(1, str);
                    recordList.Add(re);
                }
                record = re;
            }
            else if (id > 0)
            {
                if (recordList != null)
                {
                    recordList.FirstOrDefault(a => a.ID == id).PathOfRecord = str;
                }
                else
                {
                    recordList = new List<Record>();
                    re = new Record(1, str);
                    recordList.Add(re);
                    record = re;
                }
                
            }
            else
            {
                return;
            }
            
            if (!Directory.Exists(RecordPath))
            {
                Directory.CreateDirectory(RecordPath);
            }
            File.WriteAllText(RecordPath + RecordName, JsonMapper.ToJson(recordList), Encoding.UTF8);
        }
        /// <summary>
        /// 判断是否需要保存
        /// </summary>
        public static bool SaveJudgementPrompt()
        {
            if (!Directory.Exists(RecordPath))
            {
                Directory.CreateDirectory(RecordPath);
            }
            Manager.Instace.SaveToFile(RecordPath + "automatic" + Manager.Instace.SaveFileFormat);
            string str1 = File.ReadAllText(RecordPath + "automatic" + Manager.Instace.SaveFileFormat, Encoding.UTF8);
            if (str1.Length <= 3403)
            {
                return false;
            }
            else
            {
                if (record == null)
                {
                    return true;
                }
                else
                {
                    string str2 = File.ReadAllText(record.PathOfRecord, Encoding.UTF8);
                    //Debug.LogError(str1.Length + "      " + str2.Length);
                    if (str1.Length == str2.Length)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }

    public class Record
    {
        public int ID;
        public string PathOfRecord;
        public Record() { }
        public Record(int id, string path)
        {
            ID = id;
            PathOfRecord = path.Replace('\\','/');
        }
    }

}
