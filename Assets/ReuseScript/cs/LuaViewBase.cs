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
    
    public LuaTable LuaClass{
       get{ return luaBh.scriptEnv;}
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
        luaBh.OnUpdate();
    }

    void OnDestroy()
    {
        luaBh.OnDestroy();
    }
   
}
