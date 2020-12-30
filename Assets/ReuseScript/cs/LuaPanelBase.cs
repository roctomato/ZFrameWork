using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XLua;
using Zby;

[LuaCallCSharp]
public class LuaPanelBase : CnPanelObj
{
    LuaBehaviour luaBh;
 
    public override void OnLoad(params object[] args) 
    { 
        luaBh = new LuaBehaviour();
        string stript_name = args[0] as string;
        LuaTable param = null;
        if (args.Length > 1) param = args[1] as LuaTable;
        luaBh.DoLoad<LuaPanelBase>( stript_name, param, this);
    }
    
    public override bool OnUnload() 
    { 
        luaBh.OnDestroy();
        return true; 
    } 
  
    public LuaTable LuaClass{
       get{ return luaBh.scriptEnv;}
    }
}
