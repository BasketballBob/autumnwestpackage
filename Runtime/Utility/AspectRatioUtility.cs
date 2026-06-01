using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Script used to lock camera view to a specific aspect ratio (and add black bars otherwise)
    /// Reference: https://www.youtube.com/watch?v=PClWqhfQlpU
    /// UI Scale reference: https://www.youtube.com/watch?v=62jJmyIxtVM
    /// God bless you Max O'Didily 
    /// </summary>
    public class AspectRatioUtility : MonoBehaviour
    {
        [SerializeField]
        private float _targetWidth = 16;
        [SerializeField]
        private float _targetHeight = 9;

        public static float ScreenWidth => Screen.width;
        public static float ScreenHeight => Screen.height;
        public static float WindowAspect => ScreenWidth / ScreenHeight;

        [Button]
        private void Test()
        {
            Adjust(AWGameManager.Camera, _targetWidth, _targetHeight);
        }

        private void Update()
        {
            Test();
        }

        public static void Adjust(Camera cam, float targetWidth, float targetHeight)
        {
            float targetAspect = targetWidth / targetHeight;
            float scaleHeight = WindowAspect / targetAspect;

            Debug.Log($"ADJUST w:{ScreenWidth} h:{ScreenHeight} targetAspect:{targetAspect} scaleHeight:{scaleHeight}");

            // If screen is too tall
            if (scaleHeight < 1.0f)
            {
                Rect rect = cam.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1f - scaleHeight) / 2f;

                cam.rect = rect;
            }
            // If screen is too wide
            else
            {
                float scaleWidth = 1f / scaleHeight;

                Rect rect = cam.rect;

                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0;

                cam.rect = rect;
            }
        }
    }
}
