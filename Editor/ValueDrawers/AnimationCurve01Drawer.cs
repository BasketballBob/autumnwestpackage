using System.Collections;
using System.Collections.Generic;
using AWP;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AWPEditor
{
    public class AnimationCurve01Drawer : OdinValueDrawer<AnimationCurve01>
    {
        private PropertyValueEntry _self;

        protected override void Initialize()
        {
            _self = this.Property.BaseValueEntry;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.BeginHorizontalPropertyLayout(label);

            // AnimationCurve curve = (AnimationCurve)EditorGUILayout.CurveField((AnimationCurve)_self.ValueEntry.WeakSmartValue, Color.red, new Rect(0, 0, 1, 1));
            // if (curve.keys.Length > 1)
            // {
            //     curve.keys[0].time = 0;
            //     curve.keys[curve.keys.Length - 1].time = 1;
                
            //     curve.keys.ForEach((x) =>
            //     {
            //         x.time = Mathf.Clamp01(x.time);
            //         x.value = Mathf.Clamp01(x.value);
            //     });

            //     Debug.Log("EEEEE");
            // }

            _self.WeakSmartValue = (AnimationCurve01)EditorGUILayout.CurveField((AnimationCurve)_self.WeakSmartValue, Color.red, new Rect(0, 0, 1, 1));
            _self.Update();
            
            SirenixEditorGUI.EndHorizontalPropertyLayout();
        }
    }
}
