using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AWP;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AWPEditor
{
    public class SerializedActionDrawer : OdinValueDrawer<SerializedAction>
    {
        private InspectorProperty _serializedAction;
        private InspectorProperty _gameObject;
        private InspectorProperty _actionCall;
        private InspectorProperty _parameter1;

        protected override void Initialize()
        {
            _serializedAction = this.Property;
            _gameObject = this.Property.Children["_gameObject"];
            _actionCall = this.Property.Children["_actionCall"];
            _parameter1 = _actionCall.Children["Parameter1"];
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            return; //DEBUG

            _gameObject.Draw();
            if ((GameObject)_gameObject.ValueEntry.WeakSmartValue == null) return;

            //int actionIndex = -1;
            ValueDropdownList<ActionCall> actionList = ((SerializedAction)_serializedAction.ValueEntry.WeakSmartValue).GetAllFunctions();

            Debug.Log("EEEE");
            
            _actionCall.ValueEntry.WeakSmartValue = SirenixEditorFields.Dropdown<ActionCall>(rect, 
                GUIContent.none, (ActionCall)_actionCall.ValueEntry.WeakSmartValue, actionList.Select(x => x.Value).ToArray(), 
                actionList.Select(x => x.Text).ToArray());

            _actionCall.ValueEntry.WeakSmartValue = null;
        }
    }
}
