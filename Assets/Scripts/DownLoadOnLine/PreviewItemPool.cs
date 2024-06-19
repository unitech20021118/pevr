using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewItemPool : MonoBehaviour
{
    public GameObject PreviewItemPrefab;
    //public Transform PreviewItemPoolTransform;
    public Queue<GameObject> PreviewItemQueue = new Queue<GameObject>();
    //public Dictionary<string, GameObject> PreviewItemObjectsDictionary = new Dictionary<string, GameObject>();
    //private Queue<string> idQueue = new Queue<string>();
    private int MaxNum = 30;
    //private GameObject obj;
    /// <summary>
    /// 从对象池拿对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetPreviewItemGameObjectPool()
    {
        if (PreviewItemQueue.Count > 0)
        {
            return PreviewItemQueue.Dequeue();
        }
        return  Instantiate(PreviewItemPrefab);
    }
    /// <summary>
    /// 将对象添加到对象池
    /// </summary>
    /// <param name="id"></param>
    /// <param name="previewItemGameObject"></param>
    public void AddPreviewItemObjectPool(GameObject obj)
    {
        if (PreviewItemQueue.Count < MaxNum)
        {
            obj.transform.Find("icon").gameObject.SetActive(false);
            PreviewItemQueue.Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}
/// <summary>
/// 预览图对象池
/// </summary>
public class SpritePool
{
    public Dictionary<string, Sprite> SpritesPoolDictionary = new Dictionary<string, Sprite>();
    private Queue<string> idQueue = new Queue<string>();
    private int MaxNum = 50;

    /// <summary>
    /// 从对象池获取sprite
    /// </summary>
    public Sprite GetSpritePool(string id)
    {
        if (SpritesPoolDictionary.ContainsKey(id))
        {
            return SpritesPoolDictionary[id];
        }
        return null;
    }
    /// <summary>
    /// 将对象添加到对象池
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sprite"></param>
    public void AddSpritePool(string id, Sprite sprite)
    {
        if (SpritesPoolDictionary.ContainsKey(id))
        {
            return;
        }
        if (idQueue.Count >= MaxNum)
        {
            SpritesPoolDictionary.Remove(idQueue.Dequeue());
        }
        SpritesPoolDictionary.Add(id, sprite);
        idQueue.Enqueue(id);
    }
}

public class DownLoadingPreviewItemPool
{
    public Dictionary<string, GameObject> PreviewItemObjectsDictionary = new Dictionary<string, GameObject>();
    private List<string> idList = new List<string>();
    private int MaxNum = 20;
    private GameObject obj;
    /// <summary>
    /// 从对象池拿对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetPreviewItemGameObjectPool(string id)
    {
        if (PreviewItemObjectsDictionary.ContainsKey(id))
        {
            obj = PreviewItemObjectsDictionary[id];
            PreviewItemObjectsDictionary.Remove(id);
            idList.Remove(id);
            return obj;
        }
        return null;
    }
    /// <summary>
    /// 将对象添加到对象池
    /// </summary>
    /// <param name="id"></param>
    /// <param name="previewItemGameObject"></param>
    public void AddPreviewItemObjectPool(string id, GameObject previewItemGameObject)
    {
        if (PreviewItemObjectsDictionary.ContainsKey(id))
        {
            return;
        }
        if (idList.Count >= MaxNum)
        {
            PreviewItemObjectsDictionary.Remove(idList[0]);
            idList.Remove(idList[0]);
        }
        PreviewItemObjectsDictionary.Add(id, previewItemGameObject);
        idList.Add(id);
    }
}

