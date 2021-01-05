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
    public LuaTable LuaClass{
       get{ return luaBh.scriptEnv;}
    }
    public LuaTable  OnLoad(string stript_name, LuaTable param) 
    {   
        luaBh = new LuaBehaviour( this.transform);
        return luaBh.DoLoad<LuaNormalBehaiour>( stript_name, param, this);
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
