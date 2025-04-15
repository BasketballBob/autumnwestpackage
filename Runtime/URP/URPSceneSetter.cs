using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AWP
{
    public class URPSceneSetter : MonoBehaviour
    {
        private static URPSettings InitialSettings;

        [SerializeField]
        private UniversalRenderPipelineAsset _urpAsset;

        [SerializeField] [InlineProperty] [HideLabel]
        private URPSettings _settings;

        private void Awake()
        {
            if (InitialSettings == null)
            {
                InitialSettings = new URPSettings(_urpAsset);
            }
        }

        private void OnEnable()
        {
            OverrideSettings();
        }

        private void OnDisable()
        {
            RevertSettings();
        }

        private void OverrideSettings()
        {
            _settings.ApplyValues(_urpAsset);
        }

        private void RevertSettings()
        {
            InitialSettings.ApplyValues(_urpAsset);
        }
    }
}
