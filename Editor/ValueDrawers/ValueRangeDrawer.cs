using System;
using System.Collections;
using System.Collections.Generic;
using AWP;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace AWPEditor
{
    [DrawerPriority(1)]
    public class ValueRangeDrawer<T> : OdinValueDrawer<ValueRange<T>>, IDefinesGenericMenuItems where T : IComparable
    {
        private InspectorProperty _rangeMode;
        private InspectorProperty _minConstant;
        private InspectorProperty _maxConstant;
        private InspectorProperty _testCurve;

        protected override void Initialize()
        {
            _rangeMode = this.Property.Children["Mode"];
            _minConstant = this.Property.Children["_minConstant"];
            _maxConstant = this.Property.Children["_maxConstant"];
            _testCurve = this.Property.Children["_testCurve"];
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.BeginHorizontalPropertyLayout(label);

            switch ((ValueRange.RangeMode)_rangeMode.ValueEntry.WeakSmartValue)
            {
                case ValueRange.RangeMode.Constant:
                    _minConstant.Draw(null);
                    break;
                case ValueRange.RangeMode.TwoConstants:
                    _minConstant.Draw(null);
                    _maxConstant.Draw(null);
                    break;
            }   

            _rangeMode.ValueEntry.WeakSmartValue = (ValueRange.RangeMode)EditorGUILayout.EnumPopup(
                (ValueRange.RangeMode)_rangeMode.ValueEntry.WeakSmartValue, GUILayout.Width(20));
            SirenixEditorGUI.EndHorizontalPropertyLayout();

            _testCurve.ValueEntry.WeakSmartValue = (AnimationCurve)EditorGUILayout.CurveField((AnimationCurve)_testCurve.ValueEntry.WeakSmartValue);

            //SirenixEditorGUI.BeginBox();
            //_rangeMode.Draw();
            //Debug.Log((float)_constant.ValueEntry.WeakSmartValue);
            //_constant.Draw();
            //_minConstant.Draw();
            //this.CallNextDrawer(label);
            //SirenixEditorGUI.EndBox();
        }

        public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            //genericMenu.AddItem(new GUIC)
        }
    }
}
