using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ListTreeNode<T>  {

    public ListTreeNode<T> parent;
    public List<ListTreeNode<T>> children;
    public T data;

    public ListTreeNode()
    {

    }

    public ListTreeNode(T data)
    {
        
        this.data = data;
        children = new List<ListTreeNode<T>>();
    }

    public ListTreeNode(T data, List<ListTreeNode<T>> list)
    {
        
        this.data = data;
        children = list;
    }

    public ListTreeNode(T data, List<ListTreeNode<T>> list, ListTreeNode<T> parent)
    {
        this.parent = parent;
        this.data = data;
        children = list;
    }
}
