using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AWP;
using Sirenix.OdinInspector;
using System.Linq;
using System;
using Sirenix.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    public class NodeComponentNetwork<TNode, TData> : MonoBehaviour where TNode : NodeComponent<TNode, TData>
    {
        [SerializeField]
        protected List<TNode> _nodeList;

        public List<TNode> NodeList => _nodeList;
        protected virtual Transform NodeHolder => transform;

        /// <summary>
        /// Breadth-First Algorithm
        /// https://www.youtube.com/watch?v=T_m27bhVQQQ
        /// </summary>
        /// <param name="startNode"></param>
        public List<TNode> GetShortestPath(TNode startNode, TNode endNode)
        {
            Queue<Vertice> queue = new Queue<Vertice>();
            List<TNode> visited = new List<TNode>();
            Dictionary<TNode, Tuple<TNode, int>> data = new Dictionary<TNode, Tuple<TNode, int>>();
            //int[] costs = Enumerable.Repeat(int.MaxValue, _nodeList.Count).ToArray();
            //int[] parents = Enumerable.Repeat(int.MaxValue, _nodeList.Count).ToArray();

            //data.Add(startNode, new Tuple<TNode, int>(startNode, 0));

            // Add initial connections
            visited.Add(startNode);
            startNode.Connections.ForEach(x =>
            {
                queue.Enqueue(new Vertice(startNode, x, 1));
            });

            // Iterate
            while (queue.Count > 0 && visited.Count < _nodeList.Count)
            {
                Vertice vertice = queue.Dequeue();
                visited.Add(vertice.To);

                if (!data.ContainsKey(vertice.To))
                {
                    data.Add(vertice.To, new Tuple<TNode, int>(vertice.From, vertice.Cost));
                    vertice.To.Connections.ForEach(x =>
                    {
                        if (visited.Contains(x)) return;
                        queue.Enqueue(new Vertice(vertice.To, x, vertice.Cost + 1));
                    });
                }
            }

            

            // Retrieve shortest path
            List<TNode> shortestPath = new List<TNode>();
            TNode navNode = endNode;
            while (navNode != startNode)
            {
                shortestPath.Insert(0, navNode);
                navNode = data[navNode].Item1;
            }

            // Debug
            // data.ForEach(x =>
            // {
            //     Debug.Log($"KEY: {x.Key} Value: {x.Value.Item1} {x.Value.Item2}");
            // });
            // shortestPath.ForEach(x =>
            // {
            //     Debug.Log($"SHORTEST PATH {x}");
            // });

            return shortestPath;
        }

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

        private struct Vertice
        {
            public TNode From;
            public TNode To;
            public int Cost;

            public Vertice(TNode from, TNode to, int cost)
            {
                From = from;
                To = to;
                Cost = cost;
            }
        }
    }
}
