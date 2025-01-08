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
    /*public class AnimationCurve01Drawer : OdinValueDrawer<AnimationCurve01>
    {
        private InspectorProperty _self;

        protected override void Initialize()
        {
            _self = this.Property;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.BeginHorizontalPropertyLayout(label);

            //EditorGUILayout.CurveField((AnimationCurve)_self.ValueEntry.WeakSmartValue);
            //_self.


            //_self.MarkSerializationRootDirty();

            // if (((AnimationCurve01)_self.Property.ValueEntry).keys.Length > 1)
            // {
            //     ((AnimationCurve01)_self.Property.ValueEntry).keys[0].time = 0;
            //     ((AnimationCurve01)_self.Property.ValueEntry).keys[((AnimationCurve01)_self.Property.ValueEntry).keys.Length - 1].time = 1;
                
            //     ((AnimationCurve01)_self.Property.ValueEntry).keys.ForEach((x) =>
            //     {
            //         x.time = Mathf.Clamp01(x.time);
            //         x.value = Mathf.Clamp01(x.value);
            //     });

            //     Debug.Log("EEEEE");
            // }
            
            SirenixEditorGUI.EndHorizontalPropertyLayout();
        }
    }*/
}
