using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.SceneSingletons)]
    public abstract class AWSceneSingletons : MonoBehaviour
    {
        public AWCamera AWCamera;
        public MenuManager MenuManager;
        public CullingBounds CullingBounds;

        protected virtual void OnEnable()
        {
            AWGameManager.Current.LoadReferences(this);
        }

        protected void Reset()
        {
            SetDefaultReferences();
        }

        #if UNITY_EDITOR
            [Button()]
            protected virtual void SetDefaultReferences()
            {
                Undo.RecordObject(this, "Set Default References");
                AWCamera = GetReference<AWCamera>();
                MenuManager = GetReference<MenuManager>();
                CullingBounds = GetReference<CullingBounds>();
            }

            protected virtual TComponent GetReference<TComponent>() where TComponent : Component
            {
                return GameObject.FindFirstObjectByType<TComponent>(FindObjectsInactive.Include)?.GetComponent<TComponent>();
            }
        #endif
    }
}
