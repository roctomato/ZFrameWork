using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XLua;
using Zby;

[LuaCallCSharp]
public class LuaNormalBehaiour :  MonoBehaviour
{
    // Start is called before the first frame update
    LuaEnv luaEnv = LuaMain.MyLuaEnv;

    private LuaTable scriptEnv;
    private LuaFunction luaStart;
    private LuaFunction luaUpdate;
    private LuaFunction luaOnDestroy;

    public GameObject UIObj { get { return gameObject; } }
    static  public  bool Attach( GameObject go, string stript_name, LuaTable param )
    {
        bool ret = false;
        do{
            LuaNormalBehaiour mb = go.AddComponent<LuaNormalBehaiour>();
            if ( null == mb ){
                ZLog.E(go, "create {0} err", stript_name);
                break;
            }
            ret = mb.OnLoad( stript_name, param);
        }while(false);
        return ret;
    }
    public bool  OnLoad(string stript_name, LuaTable param) 
    {   
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
        if (param != null){
            param.Dispose();
        }
        return true;
    }    
    public void DoUnload()
    {
        GameObject.DestroyImmediate(this.gameObject);
    }
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
