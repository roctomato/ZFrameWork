using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;
using Zby;

public class lua_attach : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaMain luaenv = LuaMain.InitLuaEvn(this.gameObject,  OnQuit, 1);
        string[] folds ={  
            Application.dataPath +"/lualogic/lua/"
            ,Application.dataPath+"/ReuseScript/lua/"};

        luaenv.InitNormalFileLoader(folds);
        luaenv. StartLua("test_attach","Main");
        Debug.Log("load ok");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnQuit(){
        LuaEnv luaenv = LuaMain.MyLuaEnv;
        LuaFunction quit = luaenv.Global.Get<LuaFunction>("Quit");
        if ( quit != null ){
            quit.Call();
            quit.Dispose();
        }
       
    }
}
