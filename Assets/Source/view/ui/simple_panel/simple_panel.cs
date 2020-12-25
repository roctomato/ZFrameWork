using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class SimplePanel : CnPanelObj
{
    Panel_component _component;

    ~SimplePanel()
    {
        ZLog.D(null, "~SimplePanel()");
    }
    public override CnUiComponent GetCnUiComponent()
    {
        if (null == _component)
        {
            _component = new Panel_component();
        }
        return _component;
    }
    void ClickCall(object[] args)
    {
        ZLog.D(null, "click button");
        this.DoUnload();
    }
   
    public override void OnLoad(params object[] args) 
    {
        ZLog.D(null, "onload order {0}", this.ZOrder);
        SetClickEventOnce(_component._btn1, ClickCall, new object[] { "Call java" });
    }
    public override bool OnUnload() {
        ZLog.D(null, "onunload order {0}", this.ZOrder);
        
        return true;
    }
}