using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class CameraPosManagerObject : SingletonReferenceObject<CameraPosManager>
    {
        private const string CreateItemName = "CameraPosManagerObject";
    }

    public interface ICameraPosManagerReference
    {
        public CameraPosManager CameraPosManager { get; }
    }

    [System.Serializable]
    public sealed class CameraPosManagerReference : ICameraPosManagerReference
    {
        [SerializeField]
        private CameraPosManager _cameraPosManager;

        public CameraPosManager CameraPosManager => _cameraPosManager;
    }

    [System.Serializable]
    public sealed class CameraPosManagerObjectReference : ICameraPosManagerReference
    {
        [SerializeField]
        private CameraPosManagerObject _cameraPosManager;

        public CameraPosManager CameraPosManager => _cameraPosManager.Reference;
    }
}
