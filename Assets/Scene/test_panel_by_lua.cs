using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Zby;

public class test_panel_by_lua : MonoBehaviour
{
    LogFile _logFile = null;

    // Start is called before the first frame update
    void Start()
    {
        _logFile = new LogFile();

        LuaMain luaenv = LuaMain.InitLuaEvn(this.gameObject, OnQuit, 1);

        string[] folds ={
            Application.dataPath +"/lualogic/lua/"
            ,Application.dataPath+"/ReuseScript/lua/"};

            luaenv.InitNormalFileLoader(folds);
        
        luaenv.StartLua("test", "Main");
        Debug.Log("load ok");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnQuit()
    {
        LuaEnv luaenv = LuaMain.MyLuaEnv;
        LuaFunction quit = luaenv.Global.Get<LuaFunction>("Quit");
        if (quit != null)
        {
            quit.Call();
            quit.Dispose();
        }
        if (_logFile != null)
        {
            _logFile.Close();
        }
    }
}
