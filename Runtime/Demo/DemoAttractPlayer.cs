using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace AWP
{
    public class DemoAttractPlayer : MonoBehaviour
    {
        public static DemoAttractPlayer Current { get; private set; }

        [SerializeField]
        private VideoPlayer _videoPlayer;

        private void OnEnable()
        {
            if (Current == null) Current = this;
        }

        private void OnDisable()
        {
            if (Current == this) Current = null;
        }
    }
}
