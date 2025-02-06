using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWStaticCamera : AWCamera
    {
        public static AWStaticCamera Current;

        [SerializeField]
        private AWCameraObject _cameraRef;

        private void Awake()
        {
            if (Current == null)
            {
                Current = this;
                _cameraRef.Reference = this;
                DontDestroyOnLoad(transform.gameObject);
            }
            else Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (Current == this) 
            {
                Current = null;
                _cameraRef.Reference = null;
            }
        }
    }
}
