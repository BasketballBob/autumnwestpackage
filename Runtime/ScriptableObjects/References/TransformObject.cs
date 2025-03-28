using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class TransformObject : SingletonReferenceObject<Transform>
    {
        private const string CreateItemName = "TransformObject";
    }

    public interface ITransformReference
    {
        public Transform Transform { get; }
    }

    [System.Serializable]
    public sealed class TransformReference : ITransformReference
    {
        [SerializeField]
        private Transform _transform;

        public Transform Transform => _transform;
    }

    [System.Serializable]
    public sealed class TransformObjectReference : ITransformReference
    {
        [SerializeField]
        private TransformObject _transform;

        public Transform Transform => _transform.Reference;
    }
}
