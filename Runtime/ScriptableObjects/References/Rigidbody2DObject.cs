using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]    
    public class Rigidbody2DObject : ReferenceObject<Rigidbody2D>
    {
        private const string CreateItemName = "Rigidbody2DObject";
    }

    public interface IRigidbody2DReference
    {
        public Rigidbody2D Rigidbody2D { get; }
    }

    [System.Serializable]
    public sealed class RigidbodyReference : IRigidbody2DReference
    {
        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;
    }

    [System.Serializable]
    public sealed class Rigidbody2DObjectReference : IRigidbody2DReference
    {
        [SerializeField]
        private Rigidbody2DObject _rigidbody2D;

        public Rigidbody2D Rigidbody2D => _rigidbody2D.Reference;
    }
}
