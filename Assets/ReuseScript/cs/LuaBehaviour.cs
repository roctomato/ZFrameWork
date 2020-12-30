using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XLua;
using Zby;

public class LuaBehaviour
{
    LuaEnv luaEnv = LuaMain.MyLuaEnv;

    public LuaTable scriptEnv;
    public LuaFunction luaStart;
    public LuaFunction luaUpdate;
    public  LuaFunction luaOnDestroy;

    public LuaTable  DoLoad<T>(string stript_name, LuaTable param, T ins) 
    {
        object[] ret = luaEnv.DoString(string.Format(@"local view = require('{0}')
         return view()", stript_name, stript_name));
        scriptEnv = ret[0] as LuaTable;
        scriptEnv.Set("mono", ins);

        
        luaStart = scriptEnv.Get<LuaFunction>("start");
        luaUpdate = scriptEnv.Get<LuaFunction>("update");
        luaOnDestroy =scriptEnv.Get<LuaFunction>("ondestroy");
        
        LuaFunction OnCreate = scriptEnv.Get<LuaFunction>("oncreate");
        if (OnCreate != null)
        {
            OnCreate.Call( scriptEnv );
            OnCreate.Dispose();
        }

        LuaFunction luaAwake = scriptEnv.Get<LuaFunction>("awake");
        if (luaAwake != null)
        {
            luaAwake.Call( scriptEnv, param );
            luaAwake.Dispose();
        }
        if (param != null){
            param.Dispose();
        }
        return scriptEnv;   
    }

    public void OnStart()
    {
        if (luaStart != null)
        {
            luaStart.Call( scriptEnv );
        }
    }
    public void OnUpdate()
    {
        if (luaUpdate != null)
        {
            luaUpdate.Call( scriptEnv );
        }
    }
    public void OnDestroy()
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