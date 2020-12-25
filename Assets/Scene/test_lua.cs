using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Zby;

public class test_lua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaEnv luaenv = LuaMain.InitLuaEvn(this.gameObject, LuaCustomLoader.LoaderFromLoacalFile, 1);
        luaenv.DoString("require 'main'");

        LuaFunction main = luaenv.Global.Get<LuaFunction>("Main");
        main.Call();
        main.Dispose();
        Debug.Log("load ok");

        //CnUIMgr.Instance.Load<LuaViewBase>("panel", "test_panel");
        //CnUIMgr.Instance.LoadPanelShow<LuaPanelBase>("panel", "test_panel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
