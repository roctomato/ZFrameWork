
using UnityEngine;
using Zby;
using XLua;

[LuaCallCSharp]
public class LuaGameMgr : LuaObjMgr
{
    private static LuaGameMgr m_Instance;
    public static LuaGameMgr Instance
    {
        get
        {
            return m_Instance;
        }
    }

    TmplLoaderBase _resLoader;
    string _path;

    public static LuaGameMgr InitByCreate(string name)
    {
        if (m_Instance == null)
        {
            GameObject cvs = new GameObject(name);
            m_Instance = new LuaGameMgr(cvs);
        }
        return m_Instance;
    }

    public static LuaGameMgr InitByFind(string name)
    {
        if (m_Instance == null)
        {
            GameObject go = GameObject.Find(name);
            m_Instance = new LuaGameMgr(go);
        }
        return m_Instance;
    }

    public string  InitPath(string path)
    {
        string old = _path;
        _path = path;
        return old;
    }

    private LuaGameMgr(GameObject root) : base(root)
    {
        _resLoader = new TmplLoaderBase("game");
        _resLoader.LoadFunc = LoadRes;
    }
   
    UnityEngine.Object LoadRes(string name)
    {
        return UnityEditor.AssetDatabase.LoadMainAssetAtPath( _path + name);
    }

    public override GameObject FindUIRes(string name)
    {
        return _resLoader.FindTmpl(name);
    }
    ///////////////
}