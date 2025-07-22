using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    /// <summary>
    /// Used to scale the children of this transform relative to the parent without scaling the parent
    /// Useful for modifying size of prefabs
    /// </summary>
    public class ChildScaler : MonoBehaviour
    {
        [SerializeField] [ReadOnly]
        private Vector3 _scaleModification = Vector3.one;
        [SerializeField]
        private Vector3 _newSelfScale = Vector3.one;

#if UNITY_EDITOR
        [Button]
        private void SetScale()
        {
            Undo.RecordObject(this, "SetScale");

            //transform.localScale = ;

            AdjustChildScale(new Vector3(transform.localScale.x / _scaleModification.x,
                transform.localScale.y / _scaleModification.y,
                transform.localScale.z / _scaleModification.z));
            AdjustChildScale(_newSelfScale);

            void AdjustChildScale(Vector3 newScale)
            {
                Vector3 oldScale = transform.localScale;
                transform.localScale = newScale;

                List<SavedTransform> savedTransforms = new List<SavedTransform>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Undo.RecordObject(transform.GetChild(i), "SaveChild");
                    savedTransforms.Add(new SavedTransform()
                    {
                        Transform = transform.GetChild(i),
                        Position = transform.GetChild(i).position,
                        Scale = transform.GetChild(i).lossyScale,
                        Rotation = transform.GetChild(i).rotation
                    });
                }

                transform.localScale = oldScale;
                savedTransforms.ForEach(x =>
                {
                    x.Transform.position = x.Position;
                    x.Transform.SetLossyScale(x.Scale);
                    x.Transform.rotation = x.Rotation;
                });

            }

            _scaleModification = _newSelfScale;
        }
#endif

        private class SavedTransform
        {
            public Transform Transform;
            public Vector3 Position;
            public Vector3 Scale;
            public Quaternion Rotation;
        }
    }
}
