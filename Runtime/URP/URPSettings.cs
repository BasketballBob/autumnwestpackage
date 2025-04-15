using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AWP
{
    [System.Serializable]
    public class URPSettings
    {
        [Header("Shadows")]
        public float ShadowDistance = 50;

        public URPSettings(UniversalRenderPipelineAsset urpAsset)
        {
            ShadowDistance = urpAsset.shadowDistance;
        }

        public void ApplyValues(UniversalRenderPipelineAsset urpAsset)
        {
            urpAsset.shadowDistance = ShadowDistance;
        }
    }
}
