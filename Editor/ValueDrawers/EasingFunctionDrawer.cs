using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using AWP;
using UnityEditor;
using System;
using Sirenix.Utilities.Editor;
using System.Linq;
using Sirenix.OdinInspector.Editor.Internal;

namespace AWPEditor
{
    [DrawerPriority(1)]
    public class EasingFunctionDrawer : OdinValueDrawer<EasingFunction>
    {
        private InspectorProperty _itemIndex;

        protected override void Initialize()
        {
            _itemIndex = this.Property.Children["ItemIndex"];
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            string[] nameArray = new string[EasingFunction.Items.Length];
            int[] indexArray = new int[EasingFunction.Items.Length];
            for (int i = 0; i < EasingFunction.Items.Length; i++)
            {
                nameArray[i] = EasingFunction.Items[i].Name;
                indexArray[i] = i;
            }

            _itemIndex.ValueEntry.WeakSmartValue = SirenixEditorFields.Dropdown<int>(rect, label,
                (int)_itemIndex.ValueEntry.WeakSmartValue, indexArray, nameArray, null);
        }
    }
}
