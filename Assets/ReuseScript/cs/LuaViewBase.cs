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
    // Start is called before the first frame update
    LuaEnv luaEnv = LuaMain.MyLuaEnv;

    private LuaTable scriptEnv;
    private LuaFunction luaStart;
    private LuaFunction luaUpdate;
    private LuaFunction luaOnDestroy;


    public override void OnLoad(params object[] args) 
    { 
        string stript_name = args[0] as string;
        LuaTable param = null;
        if (args.Length > 1) param = args[1] as LuaTable;

        object[] ret = luaEnv.DoString(string.Format(@"local view = require('{0}')
         return view()", stript_name, stript_name));
        
        scriptEnv = ret[0] as LuaTable;
        scriptEnv.Set("mono", this);

        LuaFunction luaAwake = scriptEnv.Get<LuaFunction>("awake");
        luaStart = scriptEnv.Get<LuaFunction>("start");
        luaUpdate = scriptEnv.Get<LuaFunction>("update");
        luaOnDestroy =scriptEnv.Get<LuaFunction>("ondestroy");

        if (luaAwake != null)
        {
            luaAwake.Call( scriptEnv, param );
            luaAwake.Dispose();
        }

    } //创建时调用，会在start前调用
    
    public override bool OnUnload() { return true; } //移除前调用
    public override void OnBehind(CnPanelObj topview ) { } //从顶层移到后一层

     // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart.Call( scriptEnv );
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate.Call( scriptEnv );
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy.Call( scriptEnv );
                luaOnDestroy.Dispose();
            }
            
            if (luaUpdate != null) luaUpdate.Dispose();
            if (luaStart != null) luaStart.Dispose();
            scriptEnv.Dispose();
        }
}
