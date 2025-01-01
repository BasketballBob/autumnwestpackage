using System.Collections;
using System.Collections.Generic;
using AWP;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace AWPEditor
{
    public class RandomCurveDrawer : OdinValueDrawer<RandomCurve>
    {
        private InspectorProperty _animationCurve;

        protected override void Initialize()
        {
            _animationCurve = this.Property.Children["_animationCurve"];
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            _animationCurve.Draw(label);
        }
    }
}
