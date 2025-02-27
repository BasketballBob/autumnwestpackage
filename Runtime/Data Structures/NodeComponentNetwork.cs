using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AWP;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    public class NodeComponentNetwork<TNode, TData> : MonoBehaviour where TNode : NodeComponent<TNode, TData>
    {
        [SerializeField]
        protected List<TNode> _nodeList;

        protected virtual Transform NodeHolder => transform;

        #if UNITY_EDITOR
            [Button(ButtonSizes.Large)]
            private void AddNode()
            {
                Undo.SetCurrentGroupName("Added Node");
                GameObject gameObject = new GameObject($"Node - {_nodeList.Count}");
                gameObject.transform.SetParent(NodeHolder);
                Undo.RegisterCreatedObjectUndo(gameObject, "Created new node");
                TNode newNode = gameObject.AddComponent<TNode>();
                newNode.Initialize();
                _nodeList.Add(newNode);

                PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
                EditorUtility.SetDirty(gameObject);
            }

            [Button(ButtonSizes.Large)]
            private void RemoveNode()
            {
                Undo.SetCurrentGroupName("Removed Node");
                Undo.DestroyObjectImmediate(_nodeList[_nodeList.Count - 1].gameObject);
                _nodeList.Remove(_nodeList[_nodeList.Count - 1]);

                PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
                EditorUtility.SetDirty(gameObject);
            }
        #endif
    }
}
