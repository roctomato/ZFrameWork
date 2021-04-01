using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XLua;
using Zby;

[LuaCallCSharp]
public class LuaViewBase : CnViewBase
{
    LuaBehaviour luaBh;

    float interval= 0.33f;
    float lastUpdateStamp = 0;
    bool  stopUpdate = false;

    public LuaTable LuaClass{
       get{ return luaBh.scriptEnv;}
    }
    public void StopUpdate(bool stop)
    {
        stopUpdate = stop;
    }
    public void SetInterval(float v)
    {
        interval = v;
    }
    public override void OnLoad(params object[] args) //创建时调用，会在start前调用
    {     
        luaBh = new LuaBehaviour(this.transform);
        string stript_name = args[0] as string;
        LuaTable param = null;
        if (args.Length > 1) param = args[1] as LuaTable;
        luaBh.DoLoad<LuaViewBase>( stript_name, param, this);
    } 
    
    public override bool OnUnload() { return true; } //移除前调用
    public override void OnBehind(CnPanelObj topview ) { } //从顶层移到后一层

    void Start()
    {
        luaBh.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        do
        {
            float stamp = Time.realtimeSinceStartup;
            if (stopUpdate)
            {
                break;
            }
            
            if ( stamp - lastUpdateStamp < interval)
            {
                break;
            }

            luaBh.OnUpdate(stamp);
            lastUpdateStamp = stamp;
        } while (false);
       
    }

    void OnDestroy()
    {
        luaBh.OnDestroy();
    }
   
}
