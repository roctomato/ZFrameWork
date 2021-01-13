using System;

using UnityEngine;

using WebSocketSharp;

using XLua;
using Zby;


[LuaCallCSharp]
public class LuaWebSocket : ZbyWebSocket, WebSocketHandler
{


    public LuaTable scriptEnv;
    public LuaFunction luaOnOpen;
    public LuaFunction luaOnTxtMsg;
    public LuaFunction luaOnRawMsg;
    public LuaFunction luaOnErr;
    public LuaFunction luaOnDisconnect;

    public LuaWebSocket(LuaTable sp)
    {
        ZLog.D(null, "in LuaWebSocket");
        Init(this, LuaMain.Ins);
        LuaMain.Ins.quitDl += OnQuit;
        scriptEnv = sp;
        Register();
    }
    public void OpenUrl(string url)
    {
        this.Open(url);
    }
    void Register()
    {
        luaOnOpen = scriptEnv.Get<LuaFunction>("OnOpen");
        luaOnTxtMsg = scriptEnv.Get<LuaFunction>("OnTxtMsg");
        luaOnRawMsg = scriptEnv.Get<LuaFunction>("OnRawMsg");
        luaOnErr = scriptEnv.Get<LuaFunction>("OnErr");
        luaOnDisconnect = scriptEnv.Get<LuaFunction>("OnDisconnect");
    }

    void OnQuit()
    {
        LuaFunction[] lfs = { luaOnOpen, luaOnTxtMsg , luaOnRawMsg , luaOnErr , luaOnDisconnect };
        for( int i =0; i < lfs.Length; i++)
        {
            LuaFunction lf = lfs[i];
            if (lf != null)
            {
                lf.Dispose();
            }
        }
        scriptEnv.Dispose();

    }
    public void OnOpen(string url) {
        if (luaOnOpen != null)
        {
            luaOnOpen.Call(scriptEnv, url);
        }
    }
    public bool OnTxtMsg(string text, int handle_count) {
        bool ret = true;
        if (luaOnTxtMsg!= null)
        {
            object[] result = luaOnTxtMsg.Call(scriptEnv, text, handle_count);
            if (null != result && result.Length > 0)
            {
                ret = (bool)result[0];
            }
        }
        return ret;
    }
    public bool OnRawMsg(byte[] msg, int handle_count) {
        bool ret = true;
        if (luaOnRawMsg != null)
        {
            object[] result = luaOnRawMsg.Call(scriptEnv, msg, handle_count);
            if (result.Length > 0)
            {
                ret = (bool)result[0];
            }
        }
        return ret;
    }

    public void OnErr(ErrorEventArgs e) {
        if (luaOnErr != null)
        {
            luaOnErr.Call(scriptEnv, e.Message);
        }
    }

    public void OnDisconnect(int reason, string str) {
        if (luaOnDisconnect != null)
        {
           luaOnDisconnect.Call(scriptEnv, reason, str);
        }
    }

}

