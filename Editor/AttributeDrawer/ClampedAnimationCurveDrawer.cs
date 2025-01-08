using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace AWPEditor
{
    public class ClampedAnimationCurveDrawer : OdinAttributeDrawer<ClampedAnimationCurveAttribute, AnimationCurve>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            ClampValues();
            EditorGUILayout.LabelField(label);
            this.ValueEntry.SmartValue = EditorGUILayout.CurveField(this.ValueEntry.SmartValue, Color.green, new Rect(0, 0, 1, 1));
        }

        private void ClampValues()
        {
            for (int i = 0; i < this.ValueEntry.SmartValue.keys.Length; i++)
            {
                this.ValueEntry.SmartValue.MoveKey(0, new Keyframe(.5f, .5f));
            }
        }
    }
}
