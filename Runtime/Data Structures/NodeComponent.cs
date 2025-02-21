using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    public abstract class NodeComponent<TNode, TData> : MonoBehaviour where TNode : NodeComponent<TNode, TData>
    {
        public TData Data;
        [OnCollectionChanged("SyncConnections")] [ListDrawerSettings(CustomRemoveElementFunction = "RemoveElement")]
        public List<TNode> Connections = new List<TNode>();

        public abstract void Initialize();

        #if UNITY_EDITOR
            protected void SyncConnections()
            {
                Connections.ForEach(x =>
                {
                    if (x.Connections.Contains(this as TNode)) return;
                    x.Connections.Add(this as TNode);
                    Undo.RecordObject(x, "Added connection");
                    EditorUtility.SetDirty(x);
                });
            }

            protected void RemoveElement(TNode element)
            {
                element.Connections.Remove(this as TNode);
                Connections.Remove(element);
            }
        #endif
    }
}
