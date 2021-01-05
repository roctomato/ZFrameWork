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
    LuaBehaviour luaBh;
    public GameObject UIObj { get { return gameObject; } }
    public LuaTable LuaClass{    get{ return luaBh.scriptEnv;}}

    static  public  LuaTable Attach( GameObject go, string stript_name, LuaTable param )
    {
        LuaTable ret = null;
        do{
            LuaNormalBehaiour mb = go.AddComponent<LuaNormalBehaiour>();
            if ( null == mb ){
                ZLog.E(go, "create {0} err", stript_name);
                break;
            }
            ret = mb.OnLoad(stript_name, param);
        }while(false);
        return ret;
    }

    static  public  LuaTable AttachIns( GameObject go, LuaTable luains, LuaTable param )
    {
        LuaTable ret = null;
        do{
            LuaNormalBehaiour mb = go.AddComponent<LuaNormalBehaiour>();
            if ( null == mb ){
                ZLog.E(go, "create {0} err", luains);
                break;
            }
            ret = mb.OnLoad(luains, param);
        }while(false);
        return ret;
    }
   
    static  public  LuaTable CreateIns( GameObject go, LuaTable lua_class, LuaTable param )
    {
        LuaTable ret = null;
        do{
            LuaNormalBehaiour mb = go.AddComponent<LuaNormalBehaiour>();
            if ( null == mb ){
                ZLog.E(go, "create {0} err", lua_class);
                break;
            }
            ret = mb.OnCreate(lua_class, param);
        }while(false);
        return ret;
    }
    public LuaTable  OnLoad(string stript_name, LuaTable param) 
    {   
        luaBh = new LuaBehaviour( this.transform);
        return luaBh.DoLoad<LuaNormalBehaiour>( stript_name, param, this);
    }  
    public LuaTable  OnLoad(LuaTable luains, LuaTable param) 
    {   
        luaBh = new LuaBehaviour( this.transform);
        return luaBh.DoLoad<LuaNormalBehaiour>( luains, param, this);
    }  
    public LuaTable  OnCreate(LuaTable luains, LuaTable param) 
    {   
        luaBh = new LuaBehaviour( this.transform);
        return luaBh.CreateLuaIns<LuaNormalBehaiour>( luains, param, this);
    }  
    public void DoUnload()
    {
        GameObject.DestroyImmediate(this.gameObject);
    }

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
