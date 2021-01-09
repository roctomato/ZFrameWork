using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Zby;
public class chat_test : MonoBehaviour
{
    LogFile _logFile = null;

    // Start is called before the first frame update
    void Start()
    {
        _logFile = new LogFile();

        LuaMain luaenv = LuaMain.InitLuaEvn(this.gameObject, OnQuit, 1);

        string[] folds ={
        Application.dataPath +"/chat_prj/script/"
        ,Application.dataPath+"/ReuseScript/lua/"};

        luaenv.InitNormalFileLoader(folds);
        
        luaenv.StartLua("chat_test", "Main");
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
