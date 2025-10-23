using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class CullingBoundsObject : SingletonReferenceObject<CullingBounds>
    {
        private const string CreateItemName = "CullingBoundsObject";
    }

    public interface ICullingBoundsReference
    {
        public CullingBounds CullingBounds { get; }
    }

    [System.Serializable]
    public sealed class CullingBoundsReference : ICullingBoundsReference
    {
        [SerializeField]
        private CullingBounds _cullingBounds;

        public CullingBounds CullingBounds => _cullingBounds;
    }

    [System.Serializable]
    public sealed class CullingBoundsObjectReference : ICullingBoundsReference
    {
        [SerializeField]
        private CullingBoundsObject _cullingBounds;

        public CullingBounds CullingBounds => _cullingBounds.Reference;
    }
}
