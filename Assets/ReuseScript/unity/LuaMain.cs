using System;
using UnityEngine;
using System.IO;
using XLua;

[LuaCallCSharp]
public class LuaMain : MonoBehaviour
{
    public delegate  void OnQuit();
    
    LuaEnv luaenv ;
    float lastGCTime = 0;
    float GCInterval = 1;//1 second 
    OnQuit quitDl=null;
    
    

    static LuaMain instance;

    static public LuaEnv InitLuaEvn( GameObject host, LuaEnv.CustomLoader loader, OnQuit cb=null, float gcIns=1)
    {
        if ( null == instance){
            instance = host.AddComponent<LuaMain>();
            instance.luaenv = new LuaEnv();
            instance.luaenv.AddLoader(loader);
            instance.quitDl = cb;
            instance.GCInterval = gcIns;
        } 
        return instance.luaenv;
    }

    static public LuaEnv MyLuaEnv
    {
        get
        {
            return instance.luaenv;
        }
    }
    

    void Update()
    {
        if (Time.time - lastGCTime > GCInterval)
        {
            luaenv.Tick();
            lastGCTime = Time.time;
        }
    }

    void OnApplicationQuit()
    {
        if (quitDl != null){
            quitDl();
        }
        luaenv.Dispose();
    }
    
}
