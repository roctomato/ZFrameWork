using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class TestSdkPanel : CnViewBase
{
    TestSdk_component _component;
    string _method;
    string _param;

    // Use this for initialization
    void Start () {
        ZLog.D(this, "start order {0}", this._zOrder);
        SetClickEventOnce(_component.btnCall, ClickCall, new object[] { "Call java" });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ClickCall(object[] args)
    {
        _method = _component.iptMethod.text;
        _param = _component.iptParam.text;
        ZLog.D(this, "click button method {0}({1})", _method, _param);
        
    }
    public override CnUiComponent GetCnUiComponent()
    {
        if (null == _component)
        {
            _component = new TestSdk_component();
        }
        return _component;
    }
    //
    public override void OnLoad(params object[] args) 
    {
        ZLog.D(this, "onload order {0}", this._zOrder);
    }
    public override bool OnUnload() {
        ZLog.D(this, "onunload order {0}", this._zOrder);
        return true;
    }
    public override void OnBehind(CnViewBase topview) //从顶层移到后一层
    {
        ZLog.D(this, "OnBehind order {0}", this._zOrder);
    }
    public override void OnTop(CnViewBase topview) //从后层变到顶层
    {
        ZLog.D(this, "OnTop order {0}", this._zOrder);
    }  
}
