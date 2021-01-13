using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LuaMain : MonoBehaviour
{
    public delegate  void OnQuit();
    
    LuaEnv luaenv ;
    float lastGCTime = 0;
    float GCInterval = 1;//1 second 
    LuaCustomLoader _customLoader;

    public event OnQuit quitDl;

    static LuaMain instance;

    static public LuaMain InitLuaEvn( GameObject host,  OnQuit cb=null, float gcIns=1)
    {
        if ( null == instance){
            instance = host.AddComponent<LuaMain>();
            instance.luaenv = new LuaEnv();
            if (cb != null)
                instance.quitDl += cb;
            instance.GCInterval = gcIns;
            instance._customLoader = new LuaCustomLoader();
        } 
        return instance;
    }

    static public LuaMain Ins
    {
        get
        {
            return instance;
        }
    }

    static public LuaEnv MyLuaEnv
    {
        get
        {
            return instance.luaenv;
        }
    }
    
    public void InitZipLoader(string zipfile)
    {
        _customLoader.InitZipFile(zipfile);
        luaenv.AddLoader(_customLoader.LoaderFromZipFile);
    }

    public void InitNormalFileLoader(string[] paths)
    {
        _customLoader.AddPath(paths);
        luaenv.AddLoader(_customLoader.LoaderFromLoacalFile);
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
        if (quitDl != null)
            quitDl();
        
        luaenv.Tick();
        luaenv.Dispose();
    }
    
    public bool StartLua(string module, string function)
    {
        bool ret = false;
        do{
            luaenv.DoString(string.Format("require '{0}'", module));
            LuaFunction main = luaenv.Global.Get<LuaFunction>(function);
            main.Call();
            main.Dispose();
            ret = true;
        }while(false);
        return ret;
    }
}
