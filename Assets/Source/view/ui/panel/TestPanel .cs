using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class TestPanel : CnViewBase
{
    float _waitSenconds = 10;

	// Use this for initialization
	void Start () {
        ZLog.D(this, "start order {0}", this._zOrder);
        this.StartCoroutine("TestUnload");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator TestUnload()
    {
        yield return new WaitForSeconds(_waitSenconds);
        this.DoUnload();
    }
    //
    public override void OnLoad(params object[] args) 
    {
        _waitSenconds = (float)args[0] ;
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
