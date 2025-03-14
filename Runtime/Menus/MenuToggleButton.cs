using System.Collections;
using System.Collections.Generic;
using AWP;
using UnityEngine;

public class MenuToggleButton : ToggleButton
{
    [SerializeField]
    private Menu _menu;

    protected override bool ShowToggleValue => false;
    protected override bool ShowUnityEventsInInspector => false;

    protected  override void OnEnable()
    {
        base.OnEnable();
        _toggleValue = _menu.IsVisible;
    }

    protected override void OnToggleTrue()
    {
        base.OnToggleTrue();
        _menu.PushSelf();
    }

    protected override void OnToggleFalse()
    {
        base.OnToggleFalse();
        _menu.PopSelf();
    }
}
