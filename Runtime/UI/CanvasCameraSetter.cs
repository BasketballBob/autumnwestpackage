using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        [SerializeReference]
        private IAWCameraReference _cameraReference;

        private void OnEnable()
        {
            GetComponent<Canvas>().worldCamera = _cameraReference.Camera.Camera;
        }
    }
}
