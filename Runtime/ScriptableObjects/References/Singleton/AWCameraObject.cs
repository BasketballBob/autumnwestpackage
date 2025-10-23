using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class AWCameraObject : SingletonReferenceObject<AWCamera>
    {
        private const string CreateItemName = "AWCameraObject";
    }

    public interface IAWCameraReference
    {
        public AWCamera Camera { get; }
    }

    [System.Serializable]
    public sealed class AWCameraReference : IAWCameraReference
    {
        [SerializeField]
        private AWCamera _camera;

        public AWCamera Camera => _camera;
    }

    [System.Serializable]
    public sealed class AWCameraObjectReference : IAWCameraReference
    {
        [SerializeField]
        private AWCameraObject _camera;

        public AWCamera Camera => _camera.Reference;
    }
}
