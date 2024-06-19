using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
[System.Serializable]
public class ListTree<T>  {

    private ListTreeNode<T> mRoot;
    public int nodeNum;
    public ListTreeNode<T> Root
    {
        get
        {
            return mRoot;
        }
        set
        {
            mRoot = value;
        }
    }

    public ListTree()
    {

    }

    public ListTree(ListTreeNode<T> root)
    {
        mRoot = root;
        nodeNum = 1;
    }

    //判断是否为空
    public bool IsEmpty()
    {
        return mRoot == null ? true : false;
    }

    public ListTreeNode<T> GetNode(T nodeData)
    {
        if (mRoot == null)
        {
            return null;
        }
        ListTreeNode<T> temp = null;
        Stack<ListTreeNode<T>> stack = new Stack<ListTreeNode<T>>();
        stack.Push(mRoot);
        while (stack.Count != 0)
        {
            temp = stack.Pop();
            if (temp.data.Equals(nodeData))
            {
                return temp;
            }
            for (int i = temp.children.Count - 1; i >= 0; i--)
            {
                stack.Push(temp.children[i]);
            }
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<T> GetNodeDataArray(ListTreeNode<T> node)
    {
        List<T> list = new List<T>();
        Stack<T> stack = new Stack<T>();
        while (node.parent != null)
        {
            stack.Push(node.data);
            node = node.parent;
        }
        stack.Push(mRoot.data);
        while (stack.Count != 0)
        {
            list.Add(stack.Pop());
        }
        return list;

    }



    public List<ListTreeNode<T>> GetNodeArray(ListTreeNode<T> node)
    {
        List<ListTreeNode<T>> list = new List<ListTreeNode<T>>();
        Stack<ListTreeNode<T>> stack = new Stack<ListTreeNode<T>>();
        while (node.parent != null)
        {
            stack.Push(node);
            node = node.parent;
        }
        stack.Push(mRoot);
        while (stack.Count != 0)
        {
            list.Add(stack.Pop());
        }
        return list;
    }

    public bool HaveNode(T nodeData)
    {
        ListTreeNode<T> temp = null;
        Stack<ListTreeNode<T>> stack = new Stack<ListTreeNode<T>>();
        stack.Push(mRoot);
        while (stack.Count != 0)
        {
            temp = stack.Pop();
            if (nodeData.Equals(temp.data))
            {
                return true;
            }
            for (int i = temp.children.Count - 1; i >= 0; i--)
            {
                stack.Push(temp.children[i]);
            }
        }
        return false;
    }

    public bool InsertNode(ListTreeNode<T> parent,T nodeData)
    {
        if (parent == null)
        {
            return false;
        }
        ListTreeNode<T> node = new ListTreeNode<T>(nodeData);
        node.children = parent.children;
        node.parent = parent;
        parent.children.Add(node);
        return true;
    }

    public bool DeleteNode(ListTreeNode<T> parent,T nodeData)
    {
        if (parent == null)
        {
            return false;
        }
        for (int i = 0; i < parent.children.Count; i++)
        {
            if (nodeData.Equals(parent.children[i].data))
            {
                //foreach (ListTreeNode<T> children in parent.children[i].children)
                //{

                //}
                //Informa<T>.allData.Remove(parent.children[i].data);
                //Delete(parent.children[i]);
                parent.children.RemoveAt(i);
                
            }
        }
        return true;
    }


    void Delete(ListTreeNode<T> node)
    {
        if (node.children.Count > 0)
        {
            foreach (ListTreeNode<T> l in node.children)
            {
                Delete(l);
            }
        }
        //Informa<T>.allData.Remove(node.data);
        //node.data
    }

    //public void Destroy(T data)
    //{
    //  GameObject obj=  Manager.Instace.dictFromObjectToInforma[typeof(T)]
    //}

    public bool AddLeave(ListTreeNode<T> parent,T nodeData )
    {
        if (parent == null)
        {
            return false;
        }
        for (int i = 0; i < parent.children.Count; i++)
        {
            if (parent.children[i].data.Equals(nodeData))
            {
                return false;
            }
        }
        ListTreeNode<T> node = new ListTreeNode<T>(nodeData);
        node.parent = parent;
        parent.children.Add(node);
        nodeNum++;
        return true;
    }

    public bool AddLeave(ListTreeNode<T> parent, T nodeData, out ListTreeNode<T> node)
    {
        node = new ListTreeNode<T>(nodeData);
        if (parent == null)
        {
            return false;
        }
        for (int i = 0; i < parent.children.Count; i++)
        {
            if (parent.children[i].data.Equals(nodeData))
            {
                node = parent.children[i];
                return false;
            }
        }
        parent.children.Add(node);
        node.parent = parent;
        nodeNum++;
        return true;
    }

    public bool IsLeave(ListTreeNode<T> node)
    {
        return node.children.Count == 0 ? true : false;
    }






}
