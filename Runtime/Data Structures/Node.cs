using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class Node<TData> : NodeBase<Node<TData>>
    {
        public TData Data;
    }

    public abstract class NodeBase<TNode> 
    {
        public List<TNode> Connections;
    }
}
